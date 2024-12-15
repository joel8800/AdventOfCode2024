Console.WriteLine("Day 10: Hoof It");

string[] input = File.ReadAllLines("input.txt");

int numRows = input.Length;
int numCols = input[0].Length;

// fill int grid
List<List<int>> grid = [];
for (int row = 0; row < numRows; row++)
{
    List<int> gridRow = input[row].Select(x => int.Parse(x.ToString())).ToList();
    grid.Add(gridRow);
}

// find starting points
List<(int r, int c)> trailHeads = [];

for (int row = 0; row < numRows; row++)
    for (int col = 0; col < numCols; col++)
        if (grid[row][col] == 0)
            trailHeads.Add((row, col));

int numTrails = 0;
foreach ((int r, int c) in trailHeads)
    numTrails += BFS(grid, r, c);

int answerPt1 = numTrails;

// ----------------------------------------------------------------------------

int sumRatings = 0;
foreach ((int r, int c) in trailHeads)
    sumRatings += TrailHeadScore(grid, r, c);

int answerPt2 = sumRatings;

Console.WriteLine($"Part 1: {answerPt1}");
Console.WriteLine($"Part 2: {answerPt2}");

// ============================================================================

bool OutOfBounds(List<List<int>> grid, int row, int col)
{
    if (row < 0 || row >= grid.Count || col < 0 || col >= grid[0].Count)
        return true;
    else
        return false;
}

int BFS(List<List<int>> grid, int row, int col)
{
    List<(int r, int c)> dirs = [(-1, 0), (0, 1), (1, 0), (0, -1)];

    int peaks = 0;
    Queue<(int r, int c)> q = [];
    q.Enqueue((row, col));

    HashSet<(int r, int c)> visited = [(row, col)];

    while (q.Count > 0)
    {
        (int currRow, int currCol) = q.Dequeue();
        foreach ((int r, int c) in dirs) 
        {
            int nextRow = currRow + r;
            int nextCol = currCol + c;

            if (OutOfBounds(grid, nextRow, nextCol))
                continue;

            if (grid[nextRow][nextCol] != grid[currRow][currCol] + 1)
                continue;

            if (visited.Contains((nextRow, nextCol)))
                continue;

            visited.Add((nextRow, nextCol));

            if (grid[nextRow][nextCol] == 9)
                peaks += 1;
            else
                q.Enqueue((nextRow, nextCol));
        }
    }

    return peaks;
}

int TrailHeadScore(List<List<int>> grid, int row, int col)
{
    List<(int r, int c)> dirs = [(-1, 0), (0, 1), (1, 0), (0, -1)];

    int trails = 0;
    Queue<(int r, int c)> q = [];
    q.Enqueue((row, col));

    Dictionary<(int, int), int> visited = [];
    visited[(row, col)] = 1;

    while (q.Count > 0)
    {
        (int currRow, int currCol) = q.Dequeue();

        if (grid[currRow][currCol] == 9)
            trails += visited[(currRow, currCol)];

        foreach ((int r, int c) in dirs)
        {
            int nextRow = currRow + r;
            int nextCol = currCol + c;

            if (OutOfBounds(grid, nextRow, nextCol))
                continue;

            if (grid[nextRow][nextCol] != grid[currRow][currCol] + 1)
                continue;

            if (visited.ContainsKey((nextRow, nextCol)))
            {
                visited[(nextRow, nextCol)] += visited[(currRow, currCol)];
                continue;
            }
            
            visited[(nextRow, nextCol)] = visited[(currRow, currCol)];

            q.Enqueue((nextRow, nextCol));
        }
    }

    return trails;
}
