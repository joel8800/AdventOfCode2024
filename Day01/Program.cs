
Console.WriteLine("Day 01: ");

string[] input = File.ReadAllLines("input.txt");

int total = 0;
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
{
    int l = left[i];
    int r = right[i];

    Console.WriteLine($"{l} - {r} = {r - l}");
    total += Math.Abs(r - l);
}

int score = 0;

for (int i = 0; i < left.Count; i++)
{
    int l = left[i];
    int appearances = right.Where(x => x == l).Count();

    Console.WriteLine($"{l} appears {appearances} times");
    score += l * appearances;
}


Console.WriteLine($"Part 1: {total}");
Console.WriteLine($"Part 2: {score}");
