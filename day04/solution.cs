using Position = (int row, int col);

var grid = File.ReadLines(args[0]).Select(x => x.ToArray()).ToArray();

Console.WriteLine($"Part 1: {RemovePaper(1).Single()}");
Console.WriteLine($"Part 2: {RemovePaper(int.MaxValue).Sum()}");

IEnumerable<int> RemovePaper(int maxIterations)
{
    var positionsWithPaper = Positions().Where(x => grid[x.row][x.col] == '@').ToHashSet();

    while (maxIterations > 0)
    {
        maxIterations--;

        var removablePositions = positionsWithPaper.Where(x =>
            AdjacentPositions(x).Count(y => positionsWithPaper.Contains(y)) < 4
        );

        var removed = removablePositions.Select(x => positionsWithPaper.Remove(x)).Count();
        if (removed == 0)
            yield break;
        yield return removed;
    }
}

IEnumerable<Position> Positions() =>
    Enumerable.Range(0, grid.Length).SelectMany(row => Enumerable.Range(0, grid[row].Length).Select(col => (row, col)));

IEnumerable<Position> AdjacentPositions(Position p)
{
    foreach (var row in Enumerable.Range(p.row - 1, 3))
    foreach (var col in Enumerable.Range(p.col - 1, 3))
    {
        var adjacent = (row, col);
        if (IsInBounds(adjacent) && adjacent != p)
            yield return adjacent;
    }
}

bool IsInBounds(Position p) => p.row >= 0 && p.row < grid.Length && p.col >= 0 && p.col < grid[0].Length;