using System.Text.RegularExpressions;

Console.WriteLine("Day 13: Claw Contraption");

string input = File.ReadAllText("input.txt").Trim();
string[] blocks = input.Split("\r\n\r\n");

int minTokensPt1 = 0;
long minTokensPt2 = 0;
foreach (string block in blocks)
{
    string[] lines = block.Split("\r\n");

    MatchCollection mc = Regex.Matches(block, @"\d+");
    List<int> machine = mc.Select(x => int.Parse(x.Value)).ToList();

    minTokensPt1 += MinTokensToWinPt1(machine);
    minTokensPt2 += MinTokensToWinPt2(machine);
}

int answerPt1 = minTokensPt1;

// ----------------------------------------------------------------------------

long answerPt2 = minTokensPt2;

Console.WriteLine($"Part 1: {answerPt1}");
Console.WriteLine($"Part 2: {answerPt2}");

// ============================================================================

// Part 1: brute force, only up to 100 presses of A and B
int MinTokensToWinPt1(List<int> machine)
{
    for (int aPresses = 0; aPresses < 100; aPresses++)
    {
        for (int bPresses = 0; bPresses < 100; bPresses++)
        {
            int aPos = aPresses * machine[0] + bPresses * machine[2];
            int bPos = aPresses * machine[1] + bPresses * machine[3];
            if (aPos == machine[4] && bPos == machine[5])
                return aPresses * 3 + bPresses;
        }
    }

    return 0;
}

// Part 2: algebraic solution
long MinTokensToWinPt2(List<int> machine)
{
    long Ax = machine[0];
    long Ay = machine[1];
    long Bx = machine[2];
    long By = machine[3];
    long Px = machine[4] + 10000000000000;
    long Py = machine[5] + 10000000000000;
    
    long Ap = 0; long Bp = 0;          // button presses

    // algebraic solution for
    // Ax * Ap + Bx * Bp = Px
    // Ay * Ap + By * Bp = Py

    // solve for Ap
    // By(AxAp + BxBp) = ByPx --> AxApBy + BxBpBy = ByPx
    // Bx(AyAp + ByBp) = BxPy --> AyApBx + ByBpBx = BxPy
    //
    // AxApBy + BxBpBy - AyApBx - ByBpBx = ByPx - BxPy --> Ap(AxBy - AyBx) = ByPx - BxPy
    //
    Ap = (Px * By - Py * Bx) / (Ax * By - Ay * Bx);
    Bp = (Px - Ax * Ap) / Bx;

    if (Ax * Ap + Bx * Bp == Px && Ay * Ap + By * Bp == Py)
        return Ap * 3 + Bp;

    return 0;
}