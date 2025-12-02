var numberSequences = File
    .ReadLines(args[0])
    .Select(inputLine => new Rotation(inputLine[0], int.Parse(inputLine[1..])))
    .Aggregate<Rotation, IEnumerable<int>[]>(
        seed: [[50]],
        func: (numberSequences, rotation) =>
            [..numberSequences, rotation.Numbers(from: numberSequences.Last().Last())]
    );

Console.WriteLine($"Part 1: {numberSequences.Count(x => x.Last() is 0)}");
Console.WriteLine($"Part 2: {numberSequences.SelectMany(x => x).Count(x => x is 0)}");

record Rotation(char Direction, int Distance)
{
    public IEnumerable<int> Numbers(int from)
    {
        var current = from;
        for (var i = 0; i < Distance; i++)
        {
            current = Direction switch
                {
                    'L' => current - 1,
                    'R' => current + 1,
                    _ => throw new ArgumentOutOfRangeException($"{nameof(Direction)} '{Direction}'")
                } switch
                {
                    < 0 => 99,
                    > 99 => 0,
                    var x => x
                };
            yield return current;
        }
    }
}