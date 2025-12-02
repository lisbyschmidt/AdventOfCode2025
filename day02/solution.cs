var allIds = File
    .ReadAllText(args[0]).Split(',')
    .Select(rangeString => rangeString.Split('-'))
    .Select(rangeSplit => Range(from: long.Parse(rangeSplit[0]), to: long.Parse(rangeSplit[1])))
    .SelectMany(ids => ids);

Console.WriteLine($"Part 1: {allIds.Where(x => IsInvalidId(x.ToString(), 2)).Sum()}");
Console.WriteLine($"Part 2: {allIds.Where(x => IsInvalidId(x.ToString(), int.MaxValue)).Sum()}");

static IEnumerable<long> Range(long from, long to)
{
    for (var i = from; i <= to; i++)
        yield return i;
}

static bool IsInvalidId(string digits, int maxRepeats)
{
    for (var seqLength = digits.Length / 2; seqLength >= 1; seqLength--)
    {
        var repeats = 1;
        while (repeats < maxRepeats &&
               digits.Take(seqLength).SequenceEqual(digits.Skip(repeats * seqLength).Take(seqLength)))
        {
            repeats++;
            if (repeats * seqLength == digits.Length)
                return true;
        }
    }

    return false;
}