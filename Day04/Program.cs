using System.Text.RegularExpressions;

Console.WriteLine("Day 04: Ceres Search");

string[] input = File.ReadAllLines("input.txt");

int answerPt1 = 0;
int answerPt2 = 0;

// put characters in a 2D grid
List<List<char>> grid = [];
int rowSize = input.Length;
int colSize = input[0].Length;

for (int y = 0; y < rowSize; y++)
{
    List<char> row = [];

    foreach (char c in input[y])
        row.Add(c);

    grid.Add(row);
}

// get all the possible lines in the grid (horizontal, vertical, NW-SE, NE-SW)
List<string> allLines = [];

// horizontal: just grab each row
foreach (List<char> row in grid)
{
    string line = new([.. row]);
    allLines.Add(line);
}

// vertical: start at row 0, build a string for each column
for (int x = 0; x < colSize; x++)
{
    List<char> col = [];
    for (int y = 0; y < rowSize; y++)
        col.Add(grid[y][x]);
    
    string line = new([.. col]);
    allLines.Add(line);
}

// NW-SE: start with row 0, for each char in that row walk diagonally southeast
for (int sCol = 0; sCol < colSize; sCol++)
{
    List<char> diag = [];
    int r = 0, c = sCol;

    while (r < rowSize && c < colSize)
    {
        diag.Add(grid[r][c]);
        r++;
        c++;
    }

    string line = new([.. diag]);
    allLines.Add(line);    
}
// then start with column 0, row 1, for each char in that column,
// walk diagonally southeast
for (int sRow = 1; sRow < rowSize; sRow++)
{
    List<char> diag = [];
    int r = sRow, c = 0;
    while (r < rowSize && c < colSize)
    {
        diag.Add(grid[r][c]);
        r++;
        c++;
    }
    string line = new([.. diag]);
    allLines.Add(line);
}

// NE-SW: start with the last column on row 0, for each char in that row,
// walk diagonally southwest
for (int sCol = colSize - 1; sCol >= 0; sCol--)
{
    List<char> diag = [];
    int r = 0, c = sCol;

    while (r < rowSize && c >= 0)
    {
        diag.Add(grid[r][c]);
        r++;
        c--;
    }

    string line = new([.. diag]);
    allLines.Add(line);
}
// then start with the last column on row 1, for each char in that column,
// walk diagonally southwest
for (int sRow = 1; sRow < rowSize; sRow++)
{
    List<char> diag = [];
    int r = sRow, c = colSize - 1;

    while (r < rowSize && c >= 0)
    {
        diag.Add(grid[r][c]);
        r++;
        c--;
    }
    
    string line = new ([.. diag]);
    allLines.Add(line);
}

string fwdPattern = "XMAS";     // check forward and backward possibilities
string bwdPattern = "SAMX";

foreach (string line in allLines)
{
    MatchCollection fwdMatches = Regex.Matches(line, fwdPattern);
    MatchCollection bwdMatches = Regex.Matches(line, bwdPattern);
    answerPt1 += fwdMatches.Count + bwdMatches.Count;
}

// ----------------------------------------------------------------------------

// walk through every character in the grid
// grab the 5x5 grid down and right of it
// check to see if it's a valid XMAS pattern
for (int row = 0; row < rowSize - 2; row++)
{
    for (int col = 0; col < colSize - 2; col++)
    {
        string trial = Get5of9(grid, row, col);
        if (IsXmas(trial))
            answerPt2++;
    }
}

Console.WriteLine($"Part 1: {answerPt1}");
Console.WriteLine($"Part 2: {answerPt2}");

// ============================================================================

// Get the 5 chars in a 3x3 grid (center + 4 corners)
string Get5of9(List<List<char>> grid, int row, int col)
{
    List<char> xmas = [];
    
    xmas.Add(grid[row][col]);
    xmas.Add(grid[row][col + 2]);
    xmas.Add(grid[row + 1][col + 1]);
    xmas.Add(grid[row + 2][col]);
    xmas.Add(grid[row + 2][col + 2]);

    return new string([.. xmas]);
}

// only four possible valid patterns
bool IsXmas(string xmas3x3)
{
    switch (xmas3x3)
    {
        case "MMASS":
        case "SSAMM":
        case "MSAMS":
        case "SMASM":
            return true;
        default:
            return false;
    }
}