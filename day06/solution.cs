using System.Diagnostics;

Console.WriteLine($"Part 1: {MathProblems(args[0], RowNumberStrategy).Sum(x => x.Solve())}");
Console.WriteLine($"Part 2: {MathProblems(args[0], ColumnNumberStrategy).Sum(x => x.Solve())}");

static IEnumerable<MathProblem> MathProblems(string path, Func<char[][], long[]> numberStrategy)
{
    var grid = File.ReadLines(path).Select(x => x.ToArray()).ToArray();
    var operatorRow = grid.Last().Append(' ').ToArray();

    var columnIndex = 0;
    while (columnIndex < operatorRow.Length)
    {
        var problemWidth = operatorRow.Skip(columnIndex + 1).TakeWhile(c => c is ' ').Count();

        var subGrid = grid.SkipLast(1).Select(row => row.Skip(columnIndex).Take(problemWidth).ToArray()).ToArray();

        yield return MathProblem.Create(
            @operator: operatorRow[columnIndex],
            numbers: numberStrategy(subGrid)
        );

        columnIndex += problemWidth + 1;
    }
}

static long[] RowNumberStrategy(char[][] grid) =>
    grid.Select(row => long.Parse(row)).ToArray();

static long[] ColumnNumberStrategy(char[][] grid) =>
    Enumerable.Range(0, grid.First().Length).Select(columnIndex =>
        Enumerable.Range(0, grid.Length).Select(rowIndex =>
            grid[rowIndex][columnIndex])).Select(digits =>
        long.Parse(digits.ToArray())).ToArray();

[DebuggerDisplay("{ToString()} = {Solve()}")]
abstract record MathProblem(long[] Numbers)
{
    public long Solve() => Numbers.Aggregate(ApplyOperator);
    protected abstract long ApplyOperator(long x, long y);

    public static MathProblem Create(char @operator, long[] numbers) =>
        @operator switch
        {
            '+' => new Addition(numbers),
            '*' => new Multiplication(numbers),
            _ => throw new NotSupportedException($"Unknown operator '{@operator}'")
        };
}

record Addition(long[] Numbers) : MathProblem(Numbers)
{
    protected override long ApplyOperator(long x, long y) => x + y;
    public override string ToString() => string.Join(" + ", Numbers);
}

record Multiplication(long[] Numbers) : MathProblem(Numbers)
{
    protected override long ApplyOperator(long x, long y) => x * y;
    public override string ToString() => string.Join(" * ", Numbers);
}