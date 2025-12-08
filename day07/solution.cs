using System.Collections.Concurrent;
using Position = (int row, int col);

var rows = File.ReadLines(args[0]).ToArray();

Console.WriteLine($"Part 1: {Splits(rows, [], (0, rows[0].IndexOf('S')))}");
Console.WriteLine($"Part 2: {Timelines(rows, [], (0, rows[0].IndexOf('S')))}");

static long Splits(string[] rows, HashSet<(int, int)> cache, Position start) =>
    NextSplitter(rows, start) is { } splitter && cache.Add(splitter)
        ? Splits(rows, cache, (splitter.row, splitter.col - 1)) +
          Splits(rows, cache, (splitter.row, splitter.col + 1)) +
          1
        : 0;

static long Timelines(string[] rows, ConcurrentDictionary<(int, int), long> cache, Position start) =>
    cache.GetOrAdd(start, pos =>
        NextSplitter(rows, pos) is { } splitter
            ? Timelines(rows, cache, (splitter.row, splitter.col - 1)) +
              Timelines(rows, cache, (splitter.row, splitter.col + 1))
            : 1
    );

static Position? NextSplitter(string[] rows, Position start) => Enumerable
        .Range(start.row + 1, rows.Length - start.row - 1)
        .TakeWhile(row => rows[row][start.col] is '.')
        .Last() + 1
    is var endRow && endRow < rows.Length
    ? (endRow, start.col)
    : null;