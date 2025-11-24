using System.Diagnostics.CodeAnalysis;

namespace Bomberman;

class Game 
{
    private const int LevelWidth = 7;
    private const int LevelHeight = 5;
    private const string EmptyBlock = "  ";

    private List<IPlayer> Players = new List<IPlayer>();
    private List<IBlock> Blocks = new List<IBlock>();

    public void AddPlayer(IPlayer player) 
    {
        Players.Add(player);
        Console.WriteLine($"Added player: {player.Name}");
    }

    public void CreateLevel_Placeholder()
    {
        Blocks.Add(new SolidBlock(1, 1));
        Blocks.Add(new SolidBlock(1, 2));
        Blocks.Add(new SolidBlock(1, 3));

        Blocks.Add(new DestructibleBlock(3, 1));
        Blocks.Add(new DestructibleBlock(3, 2));
        Blocks.Add(new DestructibleBlock(3, 3));

        Blocks.Add(new DestructibleBlock(5, 1));
        Blocks.Add(new DestructibleBlock(5, 2));
        Blocks.Add(new DestructibleBlock(5, 3));

        IBlock? b;

        if (TryGetBlockAt(5, 1, out b)) b.Destroy();
        if (TryGetBlockAt(5, 2, out b)) b.Destroy();
        if (TryGetBlockAt(5, 3, out b)) b.Destroy();

    }

    public void DrawEverything()
    {
        // Ram - topprad
        for (int i = 0; i < LevelWidth + 1; i++) {
            Console.Write("▄▄");
        }
        Console.Write(Environment.NewLine);

        // Rita alla block
        for (int y = 0; y < LevelHeight; y++)
        {
            // Ram - vänster sida
            Console.Write("█");

            for (int x = 0; x < LevelWidth; x++)
            {
                // Om det finns ett block på positionen, rita ut det. Annars rita ett tomt utrymme.
                if (TryGetBlockAt(x, y, out var block))
                {
                    block.Draw();
                }
                else
                {
                    Console.Write(EmptyBlock);
                }

                // Draw-processen ska se ut så här när spelet är färdigt:
                // if: Explosion
                // else if: Spelare
                // else if: Block
                // else: Tomt utrymme

            }

            // Ram - höger sida
            Console.Write("█");

            // Ny rad
            Console.Write(Environment.NewLine);
        }

        // Ram - bottenrad
        for (int i = 0; i < LevelWidth + 1; i++) {
            Console.Write("▀▀");
        }
        Console.Write(Environment.NewLine);
    }

    public bool TryGetBlockAt(int x, int y, 
        [NotNullWhen(true)] out IBlock? block)
    {
        block = Blocks.Find(b => b.X == x && b.Y == y);
        return block != null;
    }
}