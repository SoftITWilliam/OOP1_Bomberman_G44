

using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

namespace Bomberman;

class Level
{
    public enum Corners
    {
        TopLeft, TopRight, BottomLeft, BottomRight
    }

    public int Width { get; }
    public int Height { get; }

    public List<Player> Players = new List<Player>();
    public List<IBlock> Blocks { get; } = new List<IBlock>();
    public List<Bomb> Bombs { get; } = new List<Bomb>();

    private Level(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public static Level TestLevel()
    {
        Level lvl = new Level(7, 5);

        lvl.AddBlock(new SolidBlock(1, 1));
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

    public static Level Classic()
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

    public static Level Test()
    {
        Level lvl = new Level(15, 11);


        var SolidBlockPositions = new (int x, int y)[]
        {
            (0,3), (0,7), (1,1), (1,5), (2,2), (2,5), (4,0), (4,1), (4,4),
            (4,5),
        };

        foreach (var (x, y) in SolidBlockPositions)
        {
            lvl.AddBlock(new SolidBlock(x, y));
        }

        var DestrucBlockPositions = new (int x, int y)[]
        {
            (0,1), (1,3), (1,4), (1,7), (2,0), (2,6), (3,3), (3,5), (4,2),
        };
        foreach(var(x,y) in DestrucBlockPositions)
        {
            lvl.AddBlock(new DestructibleBlock(x, y));
        }

        return lvl;
    }








    private void AddBlock(IBlock block)
    {
        if (IsOutOfBounds(block.X, block.Y))
        {
            throw new Exception("Block is out of bounds");
        }
        Blocks.Add(block);
    }
    public void AddBomb(Bomb bomb)
    {
        if (IsOutOfBounds(bomb.X, bomb.Y))
        {
            throw new Exception("Bomb is out of bounds");
        }
        Bombs.Add(bomb);
    }

    public (int x, int y) GetCornerPosition(Corners corner) => corner switch
    {
        Corners.TopLeft => (0, 0),
        Corners.TopRight => (Width - 1, 0),
        Corners.BottomLeft => (0, Height - 1),
        Corners.BottomRight => (Width - 1, Height - 1),
        _ => throw new ArgumentException(null, nameof(corner)),
    };

    public bool IsOutOfBounds(int x, int y) => 
        (x < 0 || x >= Width || y < 0 || y >= Height);


    public bool TryGetPlayerAt(int x, int y,
        [NotNullWhen(true)] out Player? player)
    {
        player = Players.Find(p => p.X == x && p.Y == y && p.IsAlive);
        return player != null;
    }
    public bool TryGetBlockAt(int x, int y,
        [NotNullWhen(true)] out IBlock? block)
    {
        block = Blocks.Find(b => b.X == x && b.Y == y);
        return block != null;
    }
    public bool TryGetBombAt(int x, int y,
        [NotNullWhen(true)] out Bomb? bomb)
    {
        bomb = Bombs.Find(b => b.X == x && b.Y == y);
        return bomb != null;
    }
    public bool HasCollidibleBlockAt(int x, int y) =>
        TryGetBlockAt(x, y, out var block) && block.HasCollision;
    public bool HasBombAt(int x, int y) =>
        TryGetBombAt(x, y, out var bomb);
    public bool HasPlayerAt(int x, int y) =>
        TryGetPlayerAt(x, y, out var player);
}