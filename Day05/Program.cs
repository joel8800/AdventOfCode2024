Console.WriteLine("Day 05: Print Queue");

string allInput = File.ReadAllText("input.txt");
string nl = Environment.NewLine;
string nl2x = $"{nl}{nl}";
string[] blocks = allInput.Split(nl2x);
string[] ruleLines = blocks[0].Split(nl, StringSplitOptions.RemoveEmptyEntries);
string[] updateLines = blocks[1].Split(nl, StringSplitOptions.RemoveEmptyEntries);

int answerPt1 = 0;
int answerPt2 = 0;

List<(int, int)> rules = [];
List<List<int>> updates = [];
List<List<int>> badUpdates = [];
List<int> goodMedians = [];
List<int> badMedians = [];

foreach (string ruleLine in ruleLines)
{
    string[] parts = ruleLine.Split("|");
    int rule1 = int.Parse(parts[0]);
    int rule2 = int.Parse(parts[1]);
    rules.Add((rule1, rule2));
}

foreach (string updateLine in updateLines)
{
    List<int> pages = [];
    string[] parts = updateLine.Split(",");
    foreach (string part in parts)
    {
        pages.Add(int.Parse(part));
    }
    updates.Add(pages);
}

foreach (List<int> pages in updates)
{
    bool correct = true;

    foreach ((int rule1, int rule2) in rules)
    {
        if (EvaluateRule(rule1, rule2, pages) == false)
        {
            correct = false;
            break;
        }
    }

    if (correct)
    {
        goodMedians.Add(GetMedian(pages));   
    }
    else
        badUpdates.Add(pages);
}

answerPt1 = goodMedians.Sum();

// ----------------------------------------------------------------------------

foreach (List<int> pages in badUpdates)
{
    bool isFixed = false;

    while (isFixed == false)
    {
        bool correct = true;

        foreach ((int rule1, int rule2) in rules)
        {
            if (EvaluateRule(rule1, rule2, pages) == false)
            {
                if (FixPageByRule(rule1, rule2, pages))
                {
                    //Console.WriteLine($"swapped {rule1} and {rule2}");
                    correct = false;
                    break;
                }
            }
        }

        if (correct)
        {
            isFixed = true;
        }
    }
    badMedians.Add(GetMedian(pages));
}

answerPt2 = badMedians.Sum();


Console.WriteLine($"Part 1: {answerPt1}");
Console.WriteLine($"Part 2: {answerPt2}");

// ============================================================================

bool EvaluateRule(int rule1, int rule2, List<int> pages)
{
    if ((pages.Contains(rule1) && pages.Contains(rule2)) == false)
        return true;

    if (pages.IndexOf(rule1) < pages.IndexOf(rule2))
        return true;
    else
        return false;
}

int GetMedian(List<int> pages)
{
    int mid = pages.Count / 2;
    return pages[mid];
}

bool FixPageByRule(int rule1, int rule2, List<int> pages)
{
    if (pages.Contains(rule1) && pages.Contains(rule2))
    {
        int index1 = pages.IndexOf(rule1);
        int index2 = pages.IndexOf(rule2);
        if (index1 > index2)
        {
            (pages[index2], pages[index1]) = (pages[index1], pages[index2]);
            return true;
        }
    }
    return false;
}