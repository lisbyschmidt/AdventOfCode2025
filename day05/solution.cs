var inputLines = File.ReadLines(args[0]).ToArray();

var mergedIdRanges = inputLines
    .TakeWhile(inputLine => inputLine.Length != 0)
    .Select(rangeString => rangeString.Split('-'))
    .Select(rangeSplit => new Range(long.Parse(rangeSplit[0]), long.Parse(rangeSplit[1])))
    .Aggregate<Range, ISet<Range>>(new HashSet<Range>(), MergeRangeIntoRanges);

var ids = inputLines
    .SkipWhile(inputLine => inputLine.Length != 0)
    .Skip(1)
    .Select(long.Parse);

Console.WriteLine($"Part 1: {ids.Count(id => mergedIdRanges.Any(idRange => idRange.Contains(id)))}");
Console.WriteLine($"Part 2: {mergedIdRanges.Select(idRange => idRange.Count).Sum()}");

static ISet<Range> MergeRangeIntoRanges(ISet<Range> ranges, Range r)
{
    var overlappingRanges = ranges.Where(x =>
        x.Overlaps(r) || r.Overlaps(x)
    ).ToArray();

    if (overlappingRanges.Length == 0)
    {
        ranges.Add(r);
        return ranges;
    }

    foreach (var overlappingRange in overlappingRanges)
        ranges.Remove(overlappingRange);

    return MergeRangeIntoRanges(ranges, new Range(
        long.Min(r.Start, overlappingRanges.Min(x => x.Start)),
        long.Max(r.End, overlappingRanges.Max(x => x.End))
    ));
}

record Range(long Start, long End)
{
    public bool Overlaps(Range range) => Contains(range.Start) || Contains(range.End);
    public bool Contains(long value) => Start <= value && value <= End;
    public long Count => End - Start + 1;
}