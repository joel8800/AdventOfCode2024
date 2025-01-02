Console.WriteLine("Day 12: Garden Groups");

string[] input = File.ReadAllLines("input.txt");

int numRows = input.Length;
int numCols = input[0].Length;

// fill grid
List<List<char>> grid = [];
foreach (string row in input)
{
    List<char> gridRow = [.. row.ToList()];
    grid.Add(gridRow);
}

List<(int, int)> dirs = [(0, 1), (1, 0), (0, -1), (-1, 0)];

// get regions
List<HashSet<(int, int)>> regions = [];
HashSet<(int, int)> visited = [];
for (int row = 0; row < numRows; row++)
{
    for (int col = 0; col < numCols; col++)
    {
        if (visited.Contains((row, col)))
            continue;

        visited.Add((row, col));
        char plant = grid[row][col];
        HashSet<(int, int)> region = [(row, col)];

        Queue<(int, int)> q = [];
        q.Enqueue((row, col));

        while (q.Count > 0)
        {
            (int r, int c) = q.Dequeue();
            foreach ((int dr, int dc) in dirs)
            {
                int newRow = r + dr;
                int newCol = c + dc;
                if (IsOutOfBounds(numRows, numCols, newRow, newCol))
                    continue;
                if (region.Contains((newRow, newCol)))
                    continue;
                if (grid[newRow][newCol] != plant)
                    continue;
                q.Enqueue((newRow, newCol));
                region.Add((newRow, newCol));
            }
        }
        visited.UnionWith(region);
        regions.Add(region);
    }
}

int answerPt1 = 0;
foreach (HashSet<(int, int)> region in regions)
    answerPt1 += GetPerimeter(region) * region.Count;

// ----------------------------------------------------------------------------

int answerPt2 = 0;
foreach (HashSet<(int, int)> region in regions)
    answerPt2 += GetSides(grid, region) * region.Count;

Console.WriteLine($"Part 1: {answerPt1}");
Console.WriteLine($"Part 2: {answerPt2}"); // 1206, 908042

// ============================================================================

bool IsOutOfBounds(int numRows, int numCols, int row, int col)
{
    return row < 0 || row >= numRows || col < 0 || col >= numCols;
}

// count sides of each square that are not in the region
int GetPerimeter(HashSet<(int, int)> region)
{
    List<(int, int)> dirs = [(0, 1), (1, 0), (0, -1), (-1, 0)];

    int perimeter = 0;
    foreach ((int row, int col) in region)
    {
        foreach ((int dr, int dc) in dirs)
        {
            int newRow = row + dr;
            int newCol = col + dc;
            if (region.Contains((newRow, newCol)) == false)
                perimeter++;
        }
    }
    return perimeter;
}

// find corners of region. number of corners = number of sides
// walk through grid and create 2x2 blocks.
// corners are where the there are 1 or 3 squares in the region
// edge case: two opposing corners are possible
int GetSides(List<List<char>> grid, HashSet<(int, int)> region)
{
    // patterns for corners
    // bit pattern 0x8 = (row, col)
    //             0x4 = (row, col + 1)
    //             0x2 = (row + 1, col)
    //             0x1 = (row + 1, col + 1)
    List<int> oneCorner = [0x8, 0x4, 0x2, 0x1, 0xe, 0xd, 0xb, 0x7];
    List<int> twoCorner = [0x9, 0x6];
    
    // walk through grid and create 2x2 blocks. look for corners
    int sides = 0;
    for (int row = -1; row < grid.Count; row++)
    {
        for (int col = -1; col < grid[0].Count; col++)
        {
            int block = 0;
            if (region.Contains((row, col)))
                block |= 8;
            if (region.Contains((row, col + 1)))
                block |= 4;
            if (region.Contains((row + 1, col)))
                block |= 2;
            if (region.Contains((row + 1, col + 1)))
                block |= 1;

            if (oneCorner.Contains(block))
                sides += 1;
            else if (twoCorner.Contains(block))
                sides += 2;
        }
    }
    return sides;
}