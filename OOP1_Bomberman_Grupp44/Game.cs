using System.Diagnostics.CodeAnalysis;

namespace Bomberman;

class Game 
{
    public const int LevelWidth = 7;
    public const int LevelHeight = 5;
    private const string EmptyBlock = "       ";

    private List<Player> Players = new List<Player>();
    private List<IBlock> Blocks = new List<IBlock>();

    public void AddPlayer(Player player) 
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
        //Blocks.Add(new DestructibleBlock(3, 2));
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
        Console.Write("▄▄");
        for (int i = 0; i < LevelWidth; i++) {
            Console.Write("▄▄▄▄▄▄▄");
        }
        Console.Write(Environment.NewLine);

        // Rita alla spelets rader
        // Varje ruta i spelet är 3x7 i ascii, så varje rad måste ritas 3 gånger.
        for (int y = 0; y < LevelHeight; y++)
        {
            // Ram - vänster sida
            Console.Write("█");

            drawLine(y, 1);

            // Ram - höger + vänster sida
            Console.Write("█" + Environment.NewLine + "█");

            drawLine(y, 2);
            
            // Ram - höger + vänster sida
            Console.Write("█" + Environment.NewLine + "█");

            drawLine(y, 3);

            // Ram - höger sida
            Console.Write("█" + Environment.NewLine);
        }

        // Ram - bottenrad
        Console.Write("▀▀");
        for (int i = 0; i < LevelWidth; i++) {
            Console.Write("▀▀▀▀▀▀▀");
        }
        Console.Write(Environment.NewLine);
    }

    private void drawLine(int y, int line) 
    {
        // Draw-processen ska se ut så här när spelet är färdigt:
        // if: Explosion
        // else if: Spelare
        // else if: Block
        // else: Tomt utrymme
        
        for (int x = 0; x < LevelWidth; x++)
        {
            if (TryGetPlayerAt(x, y, out var player))
            {
                draw(player, line);
            }
            else if (TryGetBlockAt(x, y, out var block))
            {
                draw(block, line);
            }
            else
            {
                Console.Write(EmptyBlock);
            }
        }
    }
    private void draw(IDrawable drawable, int n)
    {
        switch (n)
        {
            case 1: drawable.DrawLine1(); break;
            case 2: drawable.DrawLine2(); break;
            case 3: drawable.DrawLine3(); break;
        }
    }

    public bool TryGetBlockAt(int x, int y, 
        [NotNullWhen(true)] out IBlock? block)
    {
        block = Blocks.Find(b => b.X == x && b.Y == y);
        return block != null;
    }

    public bool TryGetPlayerAt(int x, int y,
        [NotNullWhen(true)] out Player? player)
    {
        player = Players.Find(p => p.X == x && p.Y == y);
        return player != null;
    }
}