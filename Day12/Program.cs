Console.WriteLine("Day 12:  Garden Groups");

string[] input = File.ReadAllLines("inputSample.txt");

int numRows = input.Length;
int numCols = input[0].Length;

// fill grid
List<List<char>> grid = [];
for (int row = 0; row < numRows; row++)
{
    List<char> gridRow = [.. input[row]];
    grid.Add(gridRow);
}



int answerPt1 = 0;

// ----------------------------------------------------------------------------

int answerPt2 = 0;

Console.WriteLine($"Part 1: {answerPt1}"); // 1930, 1449902
Console.WriteLine($"Part 2: {answerPt2}"); // 1206, 908042

// ============================================================================
