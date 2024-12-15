Console.WriteLine("Day 09: Disk Fragmenter");

string input = File.ReadAllText("input.txt").Trim();

List<int> blockMapPt1 = UnpackToBlockMapPt1(input);

DefragmentPt1(blockMapPt1);

// calculate checksum 
long answerPt1 = blockMapPt1.Select((fileID, index) => (long) fileID * index).Sum();
Console.WriteLine($"Part 1: {answerPt1}");

// ----------------------------------------------------------------------------

List<int> blockMapPt2 = UnpackToBlockMapPt1(input);

long answerPt2 = DefragmentPt2(input);
Console.WriteLine($"Part 2: {answerPt2}");

// ============================================================================

List<int> UnpackToBlockMapPt1(string input)
{
    int fileID = 0;
    List<int> blockMap = [];
    List<int> packedInput = input.Select(c => int.Parse(c.ToString())).ToList();

    for (int i = 0; i < packedInput.Count; i++)
    {
        // even indexes have data, odd indexes are free space
        if (i % 2 == 0)
        {
            for (int j = 0; j < packedInput[i]; j++)
                blockMap.Add(fileID);
            fileID++;
        }
        else
        {
            for (int j = 0; j < packedInput[i]; j++)
                blockMap.Add(-1);
        }
    }

    return blockMap;
}

void DefragmentPt1 (List<int> diskMap)
{
    int index = 0;
    int lastIndex = diskMap.Count - 1;

    while (index < lastIndex)
    {
        if (diskMap[index] == -1)
        {
            diskMap[index] = diskMap[lastIndex];
            diskMap[lastIndex] = -1;

            while (diskMap[lastIndex] == -1)
            {
                diskMap.RemoveAt(lastIndex);
                lastIndex--;
            }
        }
        index++;
    }
}

long DefragmentPt2 (string input)
{
    List<(int, int)> freeSpace = [];
    Dictionary<int, (int idx, int value)> fileBlocks = [];

    int index = 0;
    int fileID = 0;
    for (int i = 0; i < input.Length; i++)
    {
        int value = int.Parse(input[i].ToString());
        if (i % 2 == 0)
        {
            if (value == 0)
                Console.WriteLine("got a 0");
            fileBlocks[fileID] = (index, value);
            fileID++;
        }
        else
        {
            if (value != 0)
                freeSpace.Add((index, value));
        }
        index += value;
    }

    // work backwards from end of data list to try to fit contiguous blocks
    int fileSize = 0;
    while (fileID > 0)
    {
        fileID--;
        (index, fileSize) = fileBlocks[fileID];
        
        for (int i = 0; i < freeSpace.Count; i++)
        {
            (int start, int length) = freeSpace[i];

            if (start >= index)
            {
                freeSpace = freeSpace[..i];
                break;
            }

            if (fileSize <= length)
            {
                fileBlocks[fileID] = (start, fileSize);

                if (fileSize == length)
                    freeSpace.RemoveAt(i);
                else
                    freeSpace[i] = (start + fileSize, length - fileSize);

                break;
            }
        }
    }

    long total = 0;
    for (int i = 0; i < fileBlocks.Count; i++)
    {
        (index, fileSize) = fileBlocks[i];
        for (int j = index; j < index + fileSize; j++)
            total += i * j;
    }

    return total;
}