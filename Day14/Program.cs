using Day14;
using System.Text.RegularExpressions;

Console.WriteLine("Day 14: Restroom Redoubt");

string[] input = File.ReadAllLines("input.txt");

// grid size
int maxX = 101;   // sample 11, input 101
int maxY = 103;   // sample 7 , input 103

List<Robot> robotsPt1 = [];
List<Robot> robotsPt2 = [];
Robot.SetMax(maxX, maxY);

foreach (string line in input)
{
    MatchCollection mc = Regex.Matches(line, @"\-?\d+");
    int px = int.Parse(mc[0].Value);
    int py = int.Parse(mc[1].Value);
    int vx = int.Parse(mc[2].Value);
    int vy = int.Parse(mc[3].Value);

    Robot bot1 = new(px, py, vx, vy);
    Robot bot2 = new(px, py, vx, vy);
    robotsPt1.Add(bot1);
    robotsPt2.Add(bot2);
}

int[] quad = [0, 0, 0, 0, 0];   // [0] is for bots on center axes

foreach (Robot r in robotsPt1)
{
    r.MoveN(100);
    quad[r.GetQuadrant()]++;
}

//Console.WriteLine($"Q1:{quad[1]} Q2:{quad[2]} Q3:{quad[3]} Q4:{quad[4]}               ");
//Console.WriteLine($"part1 answer: {quad[1] * quad[2] * quad[3] * quad[4]}");
int answerPt1 = quad[1] * quad[2] * quad[3] * quad[4];

// ----------------------------------------------------------------------------

int answerPt2 = 0;

while (true)
{
    answerPt2++;
    quad = [0, 0, 0, 0, 0];
    foreach (Robot r in robotsPt2)
    {
        r.Move();
        quad[r.GetQuadrant()]++;
    }

    if (answerPt2 > 6350 || answerPt2 == 1)
    {
        //PrintGrid(robotsPt2, answerPt2);
        //Console.WriteLine($"Q1:{quad[1]} Q2:{quad[2]} Q3:{quad[3]} Q4:{quad[4]}               ");
        //Console.WriteLine($"quads multiplied: {quad[1] * quad[2] * quad[3] * quad[4]}");

        if (quad[4] > 300)  // majority of robots are in quad 4
            break;
    }

    if (answerPt2 >= 6400)
        break;
}

Console.WriteLine($"Part 1: {answerPt1}");
Console.WriteLine($"Part 2: {answerPt2}");

// ============================================================================

void PrintGrid(List<Robot> robots, int iteration)
{
    if (iteration == 1)
        Console.Clear();

    Console.SetCursorPosition(0, 0);
    Console.WriteLine($"Iteration: {iteration}        ");

    char[,] grid = new char[maxY, maxX];
    for (int y = 0; y < maxY; y++)
    {
        for (int x = 0; x < maxX; x++)
        {
            grid[y, x] = '.';
        }
    }
    foreach (Robot r in robots)
    {
        grid[r.PosY, r.PosX] = '#';
    }
    for (int y = 0; y < maxY; y++)
    {
        for (int x = 0; x < maxX; x++)
        {
            Console.Write(grid[y, x]);
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}   