Console.WriteLine("Day 11: Plutonian Pebbles");

string input = File.ReadAllText("input.txt").Trim();
string[] nums = input.Split();

List<long> stones = [];
foreach (string s in nums)
    stones.Add(long.Parse(s));

int blinksPt1 = 25;     // this method does not scale to part 2
long stoneCount = 0;
foreach (long stone in stones)
{
    List<long> newStones = [stone];
    for (int blink = 0; blink < blinksPt1; blink++)
    {
        List<long> tmpStones = [];
        foreach (long s in newStones)
        {
            if (s == 0)
                tmpStones.Add(1);
            else if (s.ToString().Length % 2 == 0)
            {
                (long l, long r) = SplitNumber(s);
                tmpStones.Add(l);
                tmpStones.Add(r);
            }
            else
                tmpStones.Add(s * 2024);
        }
        newStones = tmpStones;
    }
    
    stoneCount += newStones.Count;
}

long answerPt1 = stoneCount;
Console.WriteLine($"Part 1: {answerPt1}");

// ----------------------------------------------------------------------------

int blinksPt2 = 75;
long totalStones = 0;
Dictionary<(long, int), long> cache = [];
foreach (long stone in stones)
    totalStones += CountStones(stone, blinksPt2);

long answerPt2 = totalStones;
Console.WriteLine($"Part 2: {answerPt2}");

// ============================================================================

(long, long) SplitNumber(long num)
{
    string numStr = num.ToString();

    int mid = numStr.Length / 2;
    long left = Convert.ToInt64(numStr[..mid]);
    long right = Convert.ToInt64(numStr[mid..]);

    return (left, right);
}

long CountStones(long stone, int blinks)
{
    if (blinks == 0)
        return 1;

    if (stone == 0)
    {
        if (cache.ContainsKey((1, blinks - 1)))
            return cache[(1, blinks - 1)];
        else
            return CountStones(1, blinks - 1);
    }

    if (stone.ToString().Length % 2 == 0)
    {
        long left;
        long right;
        (long l, long r) = SplitNumber(stone);

        if (cache.ContainsKey((l, blinks - 1)))
            left = cache[(l, blinks - 1)];
        else
        {
            left = CountStones(l, blinks - 1);
            cache.Add((l, blinks - 1), left);
        }

        if (cache.ContainsKey((r, blinks - 1)))
            right = cache[(r, blinks - 1)];
        else
        {
            right = CountStones(r, blinks - 1);
            cache.Add((r, blinks - 1), right);
        }
        
        return left + right;
    }

    if (cache.ContainsKey((stone * 2024, blinks - 1)))
        return cache[(stone * 2024, blinks - 1)];
    else
        return CountStones(stone * 2024, blinks - 1);
}