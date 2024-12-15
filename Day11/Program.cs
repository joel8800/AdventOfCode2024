Console.WriteLine("Day 11: Plutonian Pebbles");

string input = File.ReadAllText("input.txt").Trim();
string[] nums = input.Split();

List<long> stones = [];
foreach (string s in nums)
    stones.Add(long.Parse(s));

Console.WriteLine(string.Join(" ", stones));

int maxBlinks = 25; // does not scale to part 2
for (int blink = 0; blink < maxBlinks; blink++)
{
    int i = 0;
    while (i < stones.Count)
    {
        if (stones[i] == 0)
            stones[i] = 1;
        else if (stones[i].ToString().Length % 2 == 0)
        {
            (long l, long r) = SplitNumber(stones[i]);
            stones.RemoveAt(i);
            stones.Insert(i, r);
            stones.Insert(i, l);
            i++;
        }
        else
            stones[i] *= 2024;

        i++;
    }
}

int answerPt1 = stones.Count;
Console.WriteLine($"Part 1: {answerPt1}");

// ----------------------------------------------------------------------------

int answerPt2 = 0;

Console.WriteLine($"Part 2: {answerPt2}");

// ============================================================================

(long, long) SplitNumber(long num)
{
    string numStr = num.ToString();
    if (numStr.Length == 0 || numStr.Length % 2 != 0)
    {
        Console.WriteLine("Error: number not even length: " + numStr);
    }

    int mid = numStr.Length / 2;
    long left = Convert.ToInt64(numStr.Substring(0, mid));
    long right = Convert.ToInt64(numStr.Substring(mid));

    return (left, right);
}