

using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

namespace Bomberman;

class Level
{
    public int Width { get; }
    public int Height { get; }

    private List<IBlock> Blocks = new List<IBlock>();

    public Level(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public static Level CreateTestLevel()
    {
        Level lvl = new Level(7, 5);

        lvl.Blocks.Add(new SolidBlock(1, 1));
        lvl.Blocks.Add(new SolidBlock(1, 2));
        lvl.Blocks.Add(new SolidBlock(1, 3));

        lvl.Blocks.Add(new DestructibleBlock(3, 1));
        lvl.Blocks.Add(new DestructibleBlock(3, 2));
        lvl.Blocks.Add(new DestructibleBlock(3, 3));

        lvl.Blocks.Add(new DestructibleBlock(5, 1));
        lvl.Blocks.Add(new DestructibleBlock(5, 2));
        lvl.Blocks.Add(new DestructibleBlock(5, 3));

        IBlock? b;
        if (lvl.TryGetBlockAt(5, 1, out b)) b.Destroy();
        if (lvl.TryGetBlockAt(5, 2, out b)) b.Destroy();
        if (lvl.TryGetBlockAt(5, 3, out b)) b.Destroy();

        return lvl;
    }

    public bool IsOutOfBounds(int x, int y) => 
        (x < 0 || x >= Width || y < 0 || y >= Height);

    public bool TryGetBlockAt(int x, int y, 
        [NotNullWhen(true)] out IBlock? block)
    {
        block = Blocks.Find(b => b.X == x && b.Y == y);
        return block != null;
    }

    public bool HasCollidibleBlockAt(int x, int y) =>
        TryGetBlockAt(x, y, out var block) && block.Collidible;
}