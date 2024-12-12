Console.WriteLine("Day 07: Bridge Repair");

string[] input = File.ReadAllLines("input.txt");
List<long> testValues = [];
List<List<long>> operands = [];

foreach (string line in input)
{
    string[] parts = line.Split(':');
    testValues.Add(long.Parse(parts[0]));

    string[] opStrings = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
    List<long> opValues = [];
    foreach (string opString in opStrings)
        opValues.Add(long.Parse(opString));
    operands.Add(opValues);
}

long answerPt1 = ValidateOperationsPt1(testValues, operands);

Console.WriteLine($"Part 1: {answerPt1}");

// ----------------------------------------------------------------------------

// part 1 solution doesn't scale to part 2
// change method to work backwards from last operand
long answerPt2 = ValidateOperationsPt2(testValues, operands);

Console.WriteLine($"Part 2: {answerPt2}");

// ============================================================================

// use a bitfield to get all combinations of + or *
long ValidateOperationsPt1(List<long> testValues, List<List<long>> operands)
{
    long total = 0;

    for (int i = 0; i < testValues.Count; i++)
    {
        long valid = 0;
        int bitCount = operands[i].Count - 1;
        int iterations = Convert.ToInt32(Math.Pow(2, bitCount));

        for (int j = 0; j < iterations; j++)
        {
            string operations = Convert.ToString(j, 2).PadLeft(operands[i].Count - 1, '0');

            if (RunOperations(operands[i], operations) == testValues[i])
            {
                valid = testValues[i];
                break;  // for (j...)
            }
        }

        total += valid;
    }

    return total;
}

long RunOperations(List<long> operands, string ops)
{
    long result = operands[0];

    for (int i = 0; i < ops.Length; i++)
    {
        if (ops[i] == '0')
            result += operands[i + 1];
        else
            result *= operands[i + 1];
    }

    return result;
}

// methodology change for part 2
// start from the last operand and see if it
//     1. evenly divides into the testValue or 
//     2. is less than the testValue or
//     3. matches the last digits of the testValue
// check each operand until the last one matches testValue, or it fails 
// one of the three tests above
long ValidateOperationsPt2(List<long> testValues, List<List<long>> operandList)
{
    long total = 0;

    for (int i = 0; i < testValues.Count; i++)
    {
        long testValue = testValues[i];
        List<long> operands = operandList[i];
        if (MatchTestValue(testValue, operands))
            total += testValue;
    }

    return total;
}

// recursively call to verify that each operand keeps the equation valid
bool MatchTestValue(long testValue, List<long> operands)
{
    // valid if last operand == testValue
    if (operands.Count == 1)
        return testValue == operands[0];

    // valid if testValue is divisible by last operand
    if (testValue % operands.Last() == 0 && MatchTestValue(testValue / operands.Last(), operands.GetRange(0, operands.Count - 1)))
        return true;

    // valid if testValue is greater than last operand
    if (testValue > operands.Last() && MatchTestValue(testValue - operands.Last(), operands.GetRange(0, operands.Count - 1)))
        return true;

    // valid if last digits of testValue match digits of last operand
    string testValStr = $"{testValue}";
    string lastValStr = $"{operands.Last()}";
    int tvLen = testValStr.Length;
    int lvLen = lastValStr.Length;
    if (testValStr.EndsWith(lastValStr) && tvLen > lvLen && 
        MatchTestValue(long.Parse(testValStr.Substring(0, tvLen - lvLen)), operands.GetRange(0, operands.Count - 1)))
        return true;

    return false;
}