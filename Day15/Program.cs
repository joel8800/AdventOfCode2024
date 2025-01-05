using System.Diagnostics;

Console.WriteLine("Day 15: Warehouse Woes");

string input = File.ReadAllText("input.txt").Trim();
string[] blocks = input.Split("\r\n\r\n");
string wareHouse = blocks[0];

// remove carriage returns from moves
string moves = blocks[1].Replace("\r\n", "");

// create warehouse grid
List<List<char>> grid = [];
foreach (string line in wareHouse.Split("\r\n"))
    grid.Add([.. line.ToCharArray()]);

// get initial robot position
(int r, int c) robot = FindRobot(grid);

Dictionary<char, (int, int)> dirs = new()
{
    ['^'] = (-1, 0),
    ['>'] = (0, 1),
    ['v'] = (1, 0),
    ['<'] = (0, -1)
};

foreach (char move in moves)
{
    (int r, int c) curr = robot;
    (int r, int c) dir = dirs[move];

    List<(int r, int c)> movers = [];
    bool canMove = true;
    while (true)
    {
        curr = Next(curr, dir);

        if (grid[curr.r][curr.c] == '.')
            break;

        if (grid[curr.r][curr.c] == '#')
        {
            canMove = false;
            break;
        }

        if (grid[curr.r][curr.c] == 'O')
            movers.Add(curr);

    }
    if (canMove == false)
        continue;
    
    // move boxes
    foreach ((int r, int c) in movers)
        grid[r + dir.r][c + dir.c] = 'O';

    // move robot
    grid[robot.r][robot.c] = '.';
    robot = Next(robot, dir);

    //PrintGrid(grid, robot);
}

int answerPt1 = SumOfBoxCoordinates(grid);

// ----------------------------------------------------------------------------

// create wide warehouse grid
grid.Clear();
foreach (string line in wareHouse.Split("\r\n"))
{
    List<char> wideLine = [];

    foreach (char c in line)
    {
        switch (c)
        {
            case '@':
                wideLine.Add('@');
                wideLine.Add('.');
                break;
            case 'O':
                wideLine.Add('[');
                wideLine.Add(']');
                break;
            default:
                wideLine.Add(c);
                wideLine.Add(c);
                break;
        }
    }
    grid.Add(wideLine);
}

// get initial robot position
robot = FindRobot(grid);

foreach (char move in moves)
{
    (int r, int c) nextStep = robot;
    (int r, int c) dir = dirs[move]; 

    nextStep = Next(nextStep, dir);

    if (grid[nextStep.r][nextStep.c] == '#')    // wall, do nothing
        continue;

    if (grid[nextStep.r][nextStep.c] == '.')    // open space, move robot
    {
        grid[robot.r][robot.c] = '.';
        robot = nextStep;
        //PrintGrid(grid, robot);
        continue;
    }

    // box in the way
    Queue<(int r, int c)> q = [];
    q.Enqueue(robot);

    HashSet<(int r, int c)> seen = [];
    bool canMove = true;

    // find all affected boxes
    while (q.Count > 0)
    {
        (int r, int c) box = q.Dequeue();
        if (seen.Contains(box))
            continue;

        seen.Add(box);
        nextStep = Next(box, dir);
        
        if (grid[nextStep.r][nextStep.c] == '#')    // hit a wall, go to next move
        {
            canMove = false;
            break;
        }
       
        if (grid[nextStep.r][nextStep.c] == '[')    // found left half, add both halves
        {
            q.Enqueue(nextStep);
            q.Enqueue((nextStep.r, nextStep.c + 1));
        }

        if (grid[nextStep.r][nextStep.c] == ']')    // found right half, add both halves
        {
            q.Enqueue(nextStep);
            q.Enqueue((nextStep.r, nextStep.c - 1));
        }
    }

    if (canMove)
    {
        // move boxes from furthest to closest
        while (seen.Count > 0)
        {
            List<(int r, int c)> boxesToMove = [.. seen.OrderBy(x => x.r).ThenBy(x => x.c)];
            foreach ((int r, int c) in boxesToMove)
            {
                nextStep = Next((r, c), dir);
                if (seen.Contains(nextStep) == false)
                {
                    grid[nextStep.r][nextStep.c] = grid[r][c];
                    grid[r][c] = '.';
                    seen.Remove((r, c));
                }
            }
        }
        grid[robot.r][robot.c] = '.';
        robot = Next(robot, dir);
        //PrintGrid(grid, robot);
    }
}
int answerPt2 = SumOfBoxCoordinates(grid);

Console.WriteLine($"Part 1: {answerPt1}");
Console.WriteLine($"Part 2: {answerPt2}");

// ============================================================================

(int, int) Next((int, int) a, (int, int) b)
{
    return (a.Item1 + b.Item1, a.Item2 + b.Item2);
}

(int, int) FindRobot(List<List<char>> grid)
{
    (int, int) robot = (-1, -1);

    for (int r = 0; r < grid.Count; r++)
        for (int c = 0; c < grid[r].Count; c++)
            if (grid[r][c] == '@')
                robot = (r, c);

    return robot;
}

int SumOfBoxCoordinates(List<List<char>> grid)
{
    int sum = 0;
    for (int r = 0; r < grid.Count; r++)
        for (int c = 0; c < grid[r].Count; c++)
            if (grid[r][c] == 'O' || grid[r][c] == '[')
                sum += r * 100 + c;

    return sum;
}

void PrintGrid(List<List<char>> grid, (int r, int c) robot)
{
    Console.SetCursorPosition(0, 0);
    
    grid[robot.r][robot.c] = '@';
    foreach (List<char> row in grid)
        Console.WriteLine(string.Join("", row));
    Console.WriteLine();
    grid[robot.r][robot.c] = '.';
    
    Thread.Sleep(50);       // slow down for visual effect, take a long time with real input
}

