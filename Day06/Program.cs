Console.WriteLine("Day 06: Guard Gallivant");

string[] input = File.ReadAllLines("input.txt");

int numRows = input.Length;
int numCols = input[0].Length;

// fill grid
List<List<char>> grid = [];
for (int row = 0; row < numRows; row++)
{
    List<char> gridRow = [.. input[row]];
    grid.Add(gridRow);
}

(int r, int c) guard = (-1, -1);

// find the guard's starting position
for (int row = 0; row < numRows; row++)
{
    List<char> gridRow = grid[row];
    int col = gridRow.FindIndex(x => x != '.' && x != '#');
    if (col != -1)
        guard = (row, col);
}

// walk guard, turning right at each obstacle, until she exits
int answerPt1 = CountGuardPositions(grid, guard);
Console.WriteLine($"Part 1: {answerPt1}");

// ----------------------------------------------------------------------------

// place obstacles to try to get the guard to walk in a loop
int answerPt2 = CountObstaclePositions(grid, guard);
Console.WriteLine($"Part 2: {answerPt2}");

// ============================================================================

bool IsInBounds(List<List<char>> grid, (int r, int c) guard)
{
    if (guard.r >= 0 && guard.r < grid.Count && guard.c >= 0 && guard.c < grid[0].Count)
        return true;
    else
        return false;
}

// returns a count of distinct positions that guard walked through
int CountGuardPositions(List<List<char>> grid, (int r, int c) guard)
{
    int dir = 0;
    List<(int r, int c)> dirs = [(-1, 0), (0, 1), (1, 0), (0, -1)]; // N E S W
    HashSet<(int r, int c)> visited = [guard];

    // walk guard, turning right at each obstacle, until she exits
    while (IsInBounds(grid, guard))
    {
        // get next location
        (int r, int c) next = guard;
        next.r = guard.r + dirs[dir].r;
        next.c = guard.c + dirs[dir].c;

        if (IsInBounds(grid, next) == false)
            break;

        // turn right if there's an obstacle
        if (grid[next.r][next.c] == '#')
            dir = (dir + 1) % 4;
        else
        {
            guard.r += dirs[dir].r;
            guard.c += dirs[dir].c;
            visited.Add(guard);     // add new position to set
        }
        //Console.WriteLine($"guard is at [{guard.r},{guard.c}] facing {dir}");
    }

    return visited.Count;
}

// place obstacles at all open locations, count when guard loops
int CountObstaclePositions(List<List<char>> grid, (int r, int c) guard)
{
    int obstaclePositions = 0;
    for (int row = 0; row < numRows; row++)
    {
        for (int col = 0; col < numCols; col++)
        {
            if (grid[row][col] == '#' || grid[row][col] == '^')
                continue;

            // set obstacle
            grid[row][col] = '#';

            if (IsGuardLooping(grid, guard))
                obstaclePositions++;

            // clear obstacle
            grid[row][col] = '.';
        }
    }

    return obstaclePositions;
}

// check if obstacle causes guard to loop
bool IsGuardLooping(List<List<char>> grid, (int r, int c) guard)
{
    int dir = 0;
    List<(int r, int c)> dirs = [(-1, 0), (0, 1), (1, 0), (0, -1)]; // N E S W
    HashSet<(int r, int c, int dir)> visited = [(guard.r, guard.c, dir)];

    // walk guard, turning right at each obstacle, until she exits
    while (true)
    {
        visited.Add((guard.r, guard.c, dir));

        // get next location
        (int r, int c) next = guard;
        next.r = guard.r + dirs[dir].r;
        next.c = guard.c + dirs[dir].c;

        // if guard leaves the grid, she isn't in a loop
        if (IsInBounds(grid, next) == false)
            return false;

        // turn right if there's an obstacle
        if (grid[next.r][next.c] == '#')
            dir = (dir + 1) % 4;
        else
        {
            guard.r += dirs[dir].r;
            guard.c += dirs[dir].c;
        }

        // if next step and direction have been visited, she's looping
        if (visited.Contains((guard.r, guard.c, dir)))
            return true;
    }
}