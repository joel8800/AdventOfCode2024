Console.WriteLine("Day 08: Resonant Collinearity");

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

Dictionary<char, List<(int r, int c)>> antennas = [];

for (int row = 0; row < numRows; row++)
{
    for (int col = 0; col < numCols; col++)
    {
        if (grid[row][col] != '.')
        {
            char freq = grid[row][col];
            if (!antennas.ContainsKey(freq))
                antennas.Add(freq, []);
            antennas[freq].Add((row, col));
        }
    }
}

HashSet<(int r, int c)> antinodesPt1 = [];

foreach (List<(int r, int c)> antenna in antennas.Values)
{
    //Console.WriteLine(string.Join(',', antenna));

    for (int i = 0; i < antenna.Count - 1; i++)
    {
        for (int j = i + 1; j < antenna.Count; j++)
        {
            (int r1, int c1) = antenna[i];
            (int r2, int c2) = antenna[j];
            (int r, int c) newAntinode1 = (2 * r1 - r2, 2 * c1 - c2);
            (int r, int c) newAntinode2 = (2 * r2 - r1, 2 * c2 - c1);

            if (IsInBounds(newAntinode1, numRows, numCols))
                antinodesPt1.Add(newAntinode1);

            if (IsInBounds(newAntinode2, numRows, numCols))
                antinodesPt1.Add(newAntinode2);
        }
    }
}

//Console.WriteLine(string.Join(',', antinodesPt1));

int answerPt1 = antinodesPt1.Count;

// ----------------------------------------------------------------------------

HashSet<(int r, int c)> antinodesPt2 = [];

foreach (List<(int c, int r)> antenna in antennas.Values)
{
    //Console.WriteLine(string.Join(',', antenna));

    for (int i = 0; i < antenna.Count; i++)
    {
        for (int j = 0; j < antenna.Count; j++)
        {
            if (antenna[i] == antenna[j])
                continue;

            (int r1, int c1) = antenna[i];
            (int r2, int c2) = antenna[j];
            int dr = r2 - r1;
            int dc = c2 - c1;
            int r = r1;
            int c = c1;
            while (r >= 0 && r < numRows && c >= 0 && c < numCols)
            {
                antinodesPt2.Add((r, c));
                r += dr;
                c += dc;
            }
            // antinodes.add((2 * r1 - r2, 2 * c1 - c2))
            // antinodes.add((2 * r2 - r1, 2 * c2 - c1))
        }
    }
}

int answerPt2 = antinodesPt2.Count;

Console.WriteLine($"Part 1: {answerPt1}");
Console.WriteLine($"Part 2: {answerPt2}");

// ============================================================================

bool IsInBounds((int r, int c) node, int rMax, int cMax)
{
    return (node.r >= 0 && node.r < rMax && node.c >= 0 && node.c < cMax);
}

