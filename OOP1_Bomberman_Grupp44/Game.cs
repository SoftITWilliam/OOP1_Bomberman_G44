using System.Diagnostics.CodeAnalysis;

namespace Bomberman;

class Game 
{
    public const int FPS = 20;    
    public static int FrameDurationMs => 1000 / FPS;

    private const string EmptyBlock = "       ";

    private List<Player> Players = new List<Player>();
    
    private Level level;
    public Level Level => level;

    public Game()
    {
        //level = Level.CreateTestLevel();
        level = Level.Classic();
    }

    public void AddPlayer(Player player) 
    {
        Players.Add(player);
        Console.WriteLine($"Added player: {player.Name}");
    }

    public void Start()
    {
        while (true)
        {
            var input = KeyInput.ReadAll();

            foreach (Player player in Players)
            {
                player.HandleInput(input, level);
            }

            // Tillfällig break condition
            if (input.Contains(ConsoleKey.Escape.ToString()))
                break;

            Console.SetCursorPosition(0, 0);
            //Console.Clear();
            DrawEverything();
            Thread.Sleep(FrameDurationMs);
        }
    }

    public void DrawEverything()
    {
        // Ram - topprad
        Console.Write("▄▄");
        for (int i = 0; i < level.Width; i++) {
            Console.Write("▄▄▄▄▄▄▄");
        }
        Console.Write(Environment.NewLine);

        // Rita alla spelets rader
        // Varje ruta i spelet är 3x7 i ascii, så varje rad måste ritas 3 gånger.
        for (int y = 0; y < level.Height; y++)
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
        for (int i = 0; i < level.Width; i++) {
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
        
        for (int x = 0; x < level.Width; x++)
        {
            if (TryGetPlayerAt(x, y, out var player))
            {
                draw(player, line);
            }
            else if (level.TryGetBlockAt(x, y, out var block))
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

    public bool TryGetPlayerAt(int x, int y,
        [NotNullWhen(true)] out Player? player)
    {
        player = Players.Find(p => p.X == x && p.Y == y);
        return player != null;
    }
}