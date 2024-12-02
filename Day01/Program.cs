Console.WriteLine("Day 01: Historian Hysteria");

string[] input = File.ReadAllLines("input.txt");

int totalPt1 = 0;
List<int> left = [];
List<int> right = [];

foreach (string line in input)
{
    string[] nums = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    left.Add(int.Parse(nums[0]));
    right.Add(int.Parse(nums[1]));
}

left.Sort();
right.Sort();

for (int i = 0; i < left.Count; i++)
    totalPt1 += Math.Abs(right[i] - left[i]);

// ----------------------------------------------------------------------------

int scorePt2 = 0;

for (int i = 0; i < left.Count; i++)
    scorePt2 += left[i] * right.Where(x => x == left[i]).Count();

Console.WriteLine($"Part 1: {totalPt1}");
Console.WriteLine($"Part 2: {scorePt2}");

// ============================================================================