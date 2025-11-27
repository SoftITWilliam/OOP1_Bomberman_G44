/*using System.Diagnostics.CodeAnalysis;
using Bomberman.Block;
using Bomberman.PlayerLogic;

namespace Bomberman;

static class LevelFactory
{
    public static Level TestLevel()
    {
        Level lvl = new Level(7, 5);

        AddBlock(new SolidBlock(1, 1), lvl);
        lvl.AddBlock(new SolidBlock(1, 2));
        lvl.AddBlock(new SolidBlock(1, 3));

        lvl.AddBlock(new DestructibleBlock(3, 1));
        lvl.AddBlock(new DestructibleBlock(3, 2));
        lvl.AddBlock(new DestructibleBlock(3, 3));

        lvl.AddBlock(new DestructibleBlock(5, 1));
        lvl.AddBlock(new DestructibleBlock(5, 2));
        lvl.AddBlock(new DestructibleBlock(5, 3));

        IBlock? b;
        if (lvl.TryGetBlockAt(5, 1, out b)) b.Destroy();
        if (lvl.TryGetBlockAt(5, 2, out b)) b.Destroy();
        if (lvl.TryGetBlockAt(5, 3, out b)) b.Destroy();

        return lvl;
    }
    public static Level ClassicLevel()
    {
        Level lvl = new Level(15, 11);

        for (int x = 1; x < lvl.Width; x += 2)
        {
            for (int y = 1; y < lvl.Height; y += 2)
            {
                lvl.AddBlock(new SolidBlock(x, y));
            }
        }
        for (int x = 0; x < lvl.Width; x++)
        {
            for (int y = 0; y < lvl.Height; y++)
            {
                if ((x + y) % 2 == 0)
                    continue;

                lvl.AddBlock(new DestructibleBlock(x, y));
            }
        }

        IBlock? b;
        int X, Y;

        (X, Y) = (X, Y) = lvl.GetCornerPosition(Corners.TopLeft);
        if (lvl.TryGetBlockAt(X + 1, Y, out b)) b.Destroy();
        if (lvl.TryGetBlockAt(X, Y + 1, out b)) b.Destroy();

        (X, Y) = lvl.GetCornerPosition(Corners.BottomLeft);
        if (lvl.TryGetBlockAt(X, Y - 1, out b)) b.Destroy();
        if (lvl.TryGetBlockAt(X + 1, Y, out b)) b.Destroy();

        (X, Y) = lvl.GetCornerPosition(Corners.TopRight);
        if (lvl.TryGetBlockAt(X - 1, Y, out b)) b.Destroy();
        if (lvl.TryGetBlockAt(X, Y + 1, out b)) b.Destroy();

        (X, Y) = lvl.GetCornerPosition(Corners.BottomRight);
        if (lvl.TryGetBlockAt(X - 1, Y, out b)) b.Destroy();
        if (lvl.TryGetBlockAt(X, Y - 1, out b)) b.Destroy();

        return lvl;
    }
    public static Level StarLevel()
    {
        Level lvl = new Level(15, 11);

        var SolidBlockPositions = new (int x, int y)[]
        {
            (0,4), (0,6), (1,1), (1,4), (1,6), (1,9), (2,2),
            (2,8), (3,0), (3,10), (4,4), (4,6), (5,1), (5,2),
            (5,4), (5,6), (5,8), (5,9), (7,0), (7,3), (7,5),
            (7,7), (7,10), (9,1), (9,2), (9,4), (9,6), (9,8),
            (9,9), (10, 4), (10,6), (11, 0), (11,10), (12, 2),
            (12, 8), (13, 1), (13, 4), (13,6), (13,9),(14,4), (14,6)
        };

        var DestrucBlockPositions = new (int x, int y)[]
        {
            (0,2), (0,8), (1,5), (2,0), (2,4), (2,6), (2,10),
            (3,1), (3,3), (3,7), (3,9), (4,1), (4,9), (5,3), (5,7),
            (6,2), (6,4), (6,5), (6,6), (6,8), (7,1), (7,4), (7,6),
            (7,9), (8,2), (8,4), (8,5), (8,6), (8,8), (9,3), (9,7),
            (10,1), (10,9), (11, 1), (11,3), (11,7), (11,9), (12,0),
            (12,4), (12,6), (12,10), (13,5), (14,2), (14,8)
        };
        foreach (var (x, y) in SolidBlockPositions)
            lvl.AddBlock(new SolidBlock(x, y));
        foreach (var (x, y) in DestrucBlockPositions)
            lvl.AddBlock(new DestructibleBlock(x, y));
        return lvl;
    }

    private static void AddBlock(IBlock block, Level level)
    {
        if (level.IsOutOfBounds(block.X, block.Y))
        {
            throw new Exception("Block is out of bounds");
        }
        level.Blocks.Add(block);
    }

}*/