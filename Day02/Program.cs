Console.WriteLine("Day 02: Red-Nosed Reports");

string[] input = File.ReadAllLines("input.txt");

int safeCountPt1 = 0;
int safeCountPt2 = 0;

foreach (string line in input)
{
    List<int> levels = line.Split(' ').Select(int.Parse).ToList();
    if (IsReportSafe(levels))
        safeCountPt1++;
}

// ----------------------------------------------------------------------------

foreach (string line in input)
{
    List<int> levels = line.Split(' ').Select(int.Parse).ToList();
    if (ProblemDampener(levels))
        safeCountPt2++;
}

Console.WriteLine($"Part 1: {safeCountPt1}");
Console.WriteLine($"Part 2: {safeCountPt2}");

// ============================================================================

bool IsReportSafe(List<int> levels)
{
    // create sub lists of the levels, removing the first and last elements
    List<int> l1 = levels.Take(levels.Count - 1).ToList();
    List<int> l2 = levels.TakeLast(levels.Count - 1).ToList();

    // differentiate the two lists
    List<int> diff = l1.Zip(l2, (a, b) => b - a).ToList();

    // if any of the differences are zero, the report is not safe
    if (diff.Any(x => x == 0))
        return false;

    // if any of the differences are greater than 3, the report is not safe
    if (diff.Any(x => x > 3) || diff.Any(x => x < -3))
        return false;

    // if the differences are not all positive or all negative, the report is not safe
    if (diff.Any(x => x > 0) && diff.Any(x => x < 0))
        return false;

    return true;
}

bool ProblemDampener(List<int> levels)
{
    // if the report is safe, we're done
    if (IsReportSafe(levels))
        return true;

    // try removing each level in turn
    for (int i = 0; i < levels.Count; i++)
    {
        List<int> newLevels = new(levels);
        newLevels.RemoveAt(i);

        // if we get a safe report, we're done
        if (IsReportSafe(newLevels))
            return true;
    }   

    return false;
}