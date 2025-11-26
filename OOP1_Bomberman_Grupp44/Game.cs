using System.Diagnostics.CodeAnalysis;

namespace Bomberman;

class Game 
{
    public const int FPS = 10;    
    public static int FrameDurationMs => 1000 / FPS;

    // Definierar hur många tecken marginal som ska finnas på varje sida av spelet
    private readonly (int Top, int Bottom, int Left, int Right) LevelMargin = 
    (
        Top: 7, 
        Bottom: 1, 
        Left: 2, 
        Right: 2
    );

    // Definierar hur många tecken varje position i rutnätet består av
    public const int BlockCharWidth = 7;
    public const int BlockCharHeight = 3;

    private int MinConsoleWidth => 
        (level.Width * BlockCharWidth) + LevelMargin.Left + LevelMargin.Right;

    private int MinConsoleHeight =>
        (level.Height * BlockCharHeight) + LevelMargin.Top + LevelMargin.Bottom;

    private bool InvalidConsoleSize() =>
        Console.WindowHeight < MinConsoleHeight ||
        Console.WindowWidth < MinConsoleWidth;

    private EmptySpace emptySpace = new EmptySpace();

    private Level level;
    public Level Level => level;

    public Game()
    {
        //level = Level.CreateTestLevel();
        //level = Level.Classic();
        level = Level.StarPattern();
    }

    public void AddPlayer(Player player) 
    {
        level.Players.Add(player);
        Console.WriteLine($"Added player: {player.Name}");
    }

    public void Start()
    {
        Console.Clear();

        if (InvalidConsoleSize())
            EnsureValidConsoleSize();

        DrawBorder();
        InitialDraw();
        DrawTitle();

        while (true)
        {
            // Om konsolen ändras till en ogiltig storlek under spelets gång,
            // så pausas spelet tills att den är stor nog igen.
            if (InvalidConsoleSize())
            {
                EnsureValidConsoleSize();
                DrawBorder();
                DrawTitle();
                InitialDraw();
            }
            
            var input = KeyInput.ReadAll();

            // Uppdatera alla spelare
            foreach (Player player in level.Players)
            {
                // Spara spelarens position innan och efter inputhanteringen.
                (int x1, int y1) = (player.X, player.Y);
                player.HandleInput(input, level);
                (int x2, int y2) = (player.X, player.Y);

                // Om spelaren har rört på sig - rita om gamla och nya positionen.
                // Detta är för att undvika onödiga redraws.
                if (x1 != x2 || y1 != y2)
                {
                    RedrawPosition(x1, y1);
                    RedrawPosition(x2, y2);
                }
            }

            // Uppdatera alla bomber
            foreach (Bomb bomb in level.Bombs)
            {
                // Om bomben exploderar så returnerar Update-metoden
                // en lista med sprängda positioner.
                var affectedblocks = bomb.Update();
                if (affectedblocks == null) continue;

                // Ta bort alla block inom de sprängda positionerna
                foreach (var (x, y) in affectedblocks)
                {
                    if (level.IsOutOfBounds(x, y)) continue;

                    if (level.TryGetBlockAt(x, y, out IBlock? block))
                    {
                        block.Destroy();
                    }
                    RedrawPosition(x, y);
                }

                // Skada alla spelare inom de sprängda positionerna
                foreach (var (x, y) in affectedblocks)
                {
                    if (level.IsOutOfBounds(x, y)) continue;
                    if (level.TryGetPlayerAt(x, y, out Player? player))
                    {
                        player.TakeDamage();
                    }
                    RedrawPosition(x, y);
                }
            }

            // Tar bort färdigexploderade bomber från listan
            level.Bombs.RemoveAll(b =>
            {
                if (b.DoneExploding)
                    RedrawPosition(b.X, b.Y);
                return b.DoneExploding;
            });

            
            // Tillfällig break condition
            if (input.Contains(ConsoleKey.Escape.ToString()))
                break;

            Thread.Sleep(FrameDurationMs);
        }
    }

    private void EnsureValidConsoleSize()
    {
        Console.SetCursorPosition(0, 0);
        while (InvalidConsoleSize())
        {
            Console.Clear();

            Console.WriteLine("Fönstret är för litet!");
            Console.WriteLine("För att starta spelet, öka storleken");

            string txt1 = $"Bredd: {Console.WindowWidth}/{MinConsoleWidth}";

            ConsoleColor c1 = Console.WindowWidth < MinConsoleWidth 
                ? ConsoleColor.Red 
                : ConsoleColor.Green;

            string txt2 = $"Höjd: {Console.WindowHeight}/{MinConsoleHeight}";

            ConsoleColor c2 = Console.WindowHeight < MinConsoleHeight 
                ? ConsoleColor.Red 
                : ConsoleColor.Green;

            ConsoleUtils.WriteWithColor(txt1, c1);
            Console.Write(Environment.NewLine);
            ConsoleUtils.WriteWithColor(txt2, c2);

            Thread.Sleep(100);
        }

        Console.Clear();
        ConsoleUtils.WriteWithColor("Nice!", ConsoleColor.Green);
        Thread.Sleep(500);
        Console.Clear();
    }

    public void DrawTitle()
    {
        // Det här ser bra ut när vi är i spelet, lita på mig
        ConsoleUtils.DrawMultiline(LevelMargin.Left, 0,
            "               _—",
            " ______     __<___     __    __     ______     ______     ______     __    __     ______     __   __    ",
            "/\\  == \\   /\\     \\   /\\ '-./  \\   /\\  == \\   /\\  ___\\   /\\  == \\   /\\ '-./  \\   /\\  __ \\   /\\ '-.\\ \\   ",
            "\\ \\  __<   \\ \\     \\  \\ \\ \\-./\\ \\  \\ \\  __<   \\ \\  __\\   \\ \\  __<   \\ \\ \\-./\\ \\  \\ \\  __ \\  \\ \\ \\-.  \\  ",
            " \\ \\_____\\  \\ \\_____\\  \\ \\_\\ \\ \\_\\  \\ \\_____\\  \\ \\_____\\  \\ \\_\\ \\_\\  \\ \\_\\ \\ \\_\\  \\ \\_\\ \\_\\  \\ \\_\\\\'\\_\\ ",
            "  \\/_____/   \\/_____/   \\/_/  \\/_/   \\/_____/   \\/_____/   \\/_/ /_/   \\/_/  \\/_/   \\/_/\\/_/   \\/_/ \\/_/ "
        );
        Console.SetCursorPosition(LevelMargin.Left + 17, 0);
        ConsoleUtils.WriteWithColor("¤", ConsoleColor.Red);
    }

    public void DrawBorder()
    {
        int frameWidth = (level.Width * BlockCharWidth + 2);
        int frameHeight = (level.Height * BlockCharHeight);

        string topFrame = string.Empty.PadLeft(frameWidth,'▄');
        string bottomFrame = string.Empty.PadLeft(frameWidth,'▀');
        
        // Draw top of frame
        int frameX = LevelMargin.Left - 1;
        int frameY = LevelMargin.Top - 1;
        Console.SetCursorPosition(frameX, frameY);
        Console.Write(topFrame);

        // Draw left and right side of frame
        for (int y = 1; y <= frameHeight; y++)
        {
            Console.SetCursorPosition(frameX, frameY + y);
            Console.Write("█");
            Console.SetCursorPosition(frameX + frameWidth - 1, frameY + y);
            Console.Write("█");
        }

        // Draw bottom of frame
        frameY = LevelMargin.Top + level.Height * BlockCharHeight;
        Console.SetCursorPosition(frameX, frameY);
        Console.Write(bottomFrame);
    }

    // Rita ut alla block och spelare. Körs en gång vid spelets start.
    public void InitialDraw()
    {
       for (int y = 0; y < level.Height; y++)
        {
            for (int x = 0; x < level.Width; x++)
            {
                RedrawPosition(x, y);
            }
        }
    }

    // Rita ut en position i spelet.
    private void RedrawPosition(int x, int y)
    {
        // Rita bakgrundsobjekt (block och tomrum)
        if (level.TryGetBlockAt(x, y, out var block))
        {
            DrawAt(x, y, block);
        } 
        else
        {
            DrawAt(x, y, emptySpace);
        }

        // Rita förgrundsobjekt (spelare, bomber, TODO powerups)
        if (level.TryGetPlayerAt(x, y, out var player) && player.IsAlive)
        {
            DrawAt(x, y, player);
        }
        else if(level.TryGetBombAt(x, y, out var bomb) && !bomb.HasExploded)
        {
            DrawAt(x, y, bomb);
        }
    } 

    // Rita ut ett drawable-objekt på en position
    // (x och y refererar till positioner i spelets rutnät)
    private void DrawAt(int x, int y, IDrawable drawable)
    {
        (int cX, int cY) = GetCursorPosition(x, y);

        Console.SetCursorPosition(cX, cY);
        drawable.DrawAt(cX, cY);
    }

    // Returnerar konsolens cursor-position för level-koordinaten
    public (int cX, int cY) GetCursorPosition(int x, int y)
    {
        int cX = LevelMargin.Left + (x * BlockCharWidth);
        int cY = LevelMargin.Top + (y * BlockCharHeight);
        return (cX, cY);
    }
}

internal class EmptySpace : IDrawable
{
    public void DrawAt(int cx, int cy) =>
        ConsoleUtils.DrawFullBlock(cx, cy, ' ');
}