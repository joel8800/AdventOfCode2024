
Console.WriteLine("Day 01: Historian Hysteria");

string[] input = File.ReadAllLines("input.txt");

int totalPt1 = 0;
List<int> left = [];
List<int> right = [];

foreach (string line in input)
{
    int[] numbers = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
    left.Add(numbers[0]);
    right.Add(numbers[1]);
}

left.Sort();
right.Sort();

for (int i = 0; i < left.Count; i++)
    totalPt1 += Math.Abs(right[i] - left[i]);

// ----------------------------------------------------------------------------

int scorePt2 = 0;

for (int i = 0; i < left.Count; i++)
{
    int appearances = right.Where(x => x == left[i]).Count();

    scorePt2 += left[i] * appearances;
}

Console.WriteLine($"Part 1: {totalPt1}"); // 2756096
Console.WriteLine($"Part 2: {scorePt2}"); // 23117829

// ============================================================================