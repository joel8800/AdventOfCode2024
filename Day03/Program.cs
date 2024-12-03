using System.Text.RegularExpressions;

Console.WriteLine("Day 03: Mull It Over");

string input = File.ReadAllText("input.txt");

Regex mult = new(@"mul\(\d+,\d+\)");
MatchCollection mc1 = mult.Matches(input);

char[] delimiters = "(,)".ToCharArray();

int answerPt1 = 0;
int answerPt2 = 0;
bool enabled = true;

foreach (Match m in mc1)
    answerPt1 += MultResult(m.Value);

// ----------------------------------------------------------------------------

Regex doMult = new(@"(mul\(\d+,\d+\))|(do\(\))|(don't\(\))");
MatchCollection mc2 = doMult.Matches(input);

foreach (Match m in mc2)
{
    if (m.Value == "do()")
    {
        enabled = true;
        continue;
    }
    
    if (m.Value == "don't()")
    {
        enabled = false;
        continue;
    }

    if (enabled)
        answerPt2 += MultResult(m.Value);
}

Console.WriteLine($"Part 1: {answerPt1}");
Console.WriteLine($"Part 2: {answerPt2}");

// ============================================================================

int MultResult(string multStr)
{
    string[] nums = multStr.Split(',');
    return int.Parse(nums[0][4..]) * int.Parse(nums[1][..^1]);
}