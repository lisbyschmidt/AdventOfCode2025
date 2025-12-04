var batteryBanks = File.ReadLines(args[0]).ToArray();
Console.WriteLine($"Part 1: {batteryBanks.Select(x => LargestJoltage(x, 2)).Sum()}");
Console.WriteLine($"Part 2: {batteryBanks.Select(x => LargestJoltage(x, 12)).Sum()}");

static int IndexOfMax(string digits) =>
    Enumerable.Range(0, digits.Length).MaxBy(x => digits[x]);

static long LargestJoltage(string digits, int length)
{
    var (index, result) = (-1, "");
    do
    {
        var digitsInScope = digits[(index + 1)..^(length - result.Length - 1)];
        index = IndexOfMax(digitsInScope) + index + 1;
        result += $"{digits[index]}";
    } while (result.Length < length);

    return long.Parse(result);
}