using System.Diagnostics.CodeAnalysis;

namespace Bomberman;

class Game 
{
    public const int FPS = 10;    
    public static int FrameDurationMs => 1000 / FPS;
    private const int LevelCursorOffsetX = 5;
    private const int LevelCursorOffsetY = 3;
    private const int BlockCharWidth = 7;
    private const int BlockCharHeight = 3;

    private int MinConsoleWidth => 
        (level.Width * BlockCharWidth) + (LevelCursorOffsetX * 2);

    private int MinConsoleHeight =>
        (level.Height * BlockCharHeight) + (LevelCursorOffsetY * 2);

    private bool InvalidConsoleSize() =>
        Console.WindowHeight < MinConsoleHeight ||
        Console.WindowWidth < MinConsoleWidth;

    private EmptySpace emptySpace = new EmptySpace();

    private Level level;
    public Level Level => level;

    public Game()
    {
        //level = Level.CreateTestLevel();
        level = Level.Classic();
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

        while (true)
        {
            if (InvalidConsoleSize())
            {
                EnsureValidConsoleSize();
                DrawBorder();
                RedrawAll();
            }
            
            var input = KeyInput.ReadAll();

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
            foreach (Bomb bomb in level.Bombs)
            {
                var affectedblocks = bomb.Update();
                if (affectedblocks == null) continue;
                foreach (var (x, y) in affectedblocks)
                {
                    if (level.IsOutOfBounds(x, y)) continue;

                    if (level.TryGetBlockAt(x, y, out IBlock block))
                    {
                        block.Destroy();
                    }
                    RedrawPosition(x, y);
                }
                
            }

            //tar bort färdigexploderade bomber från listan
            level.Bombs.RemoveAll(b =>
            {
                if (b.DoneExploding)
                    RedrawPosition(b.X, b.Y);
                return b.DoneExploding;
            });
            
            // Tillfällig break condition
            if (input.Contains(ConsoleKey.Escape.ToString()))
                break;

            //Redraw();
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

    public void DrawBorder()
    {
        int frameWidth = (level.Width * BlockCharWidth + 2);
        int frameHeight = (level.Height * BlockCharHeight);

        string topFrame = string.Empty.PadLeft(frameWidth,'▄');
        string bottomFrame = string.Empty.PadLeft(frameWidth,'▀');
        
        // Draw top of frame
        int frameX = LevelCursorOffsetX - 1;
        int frameY = LevelCursorOffsetY - 1;
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
        frameY = LevelCursorOffsetY + level.Height * BlockCharHeight;
        Console.SetCursorPosition(frameX, frameY);
        Console.Write(bottomFrame);
    }

    public void InitialDraw()
    {
        foreach (IBlock block in level.Blocks)
        {
            DrawAt(block.X, block.Y, block);
        }
        foreach (Player player in level.Players)
        {
            DrawAt(player.X, player.Y, player);
        }
    }

    public void RedrawAll()
    {
        for (int y = 0; y < level.Height; y++)
        {
            for (int x = 0; x < level.Width; x++)
            {
                RedrawPosition(x, y);
            }
        }
    }

    private void RedrawPosition(int x, int y)
    {
        if (level.TryGetBlockAt(x, y, out var block))
        {
            DrawAt(x, y, block);
        }
        else
        {
            DrawAt(x, y, emptySpace);
        }

        if (level.TryGetPlayerAt(x, y, out var player))
        {
            DrawAt(x, y, player);
        }
        else if(level.TryGetBombAt(x, y, out var bomb) && !bomb.HasExploded)
        {
            DrawAt(x, y, bomb);
        }
    } 

    private void DrawAt(int x, int y, IDrawable drawable)
    {
        (int cX, int cY) = GetCursorPosition(x, y);

        Console.SetCursorPosition(cX, cY);
        drawable.DrawAt(cX, cY);
    }

    // Returnerar konsolens cursor-position för level-koordinaten
    public (int cX, int cY) GetCursorPosition(int x, int y)
    {
        int cX = LevelCursorOffsetX + (x * BlockCharWidth);
        int cY = LevelCursorOffsetY + (y * BlockCharHeight);
        return (cX, cY);
    }

    public (int cX, int cY) GetCursorPosition(IBlock block) => 
        GetCursorPosition(block.X, block.Y);
}

internal class EmptySpace : IDrawable
{
    private string space = string.Empty.PadLeft(7, ' ');
    public void DrawAt(int cx, int cy)
    {
        for (int i = 0; i < 3; i++)
        {
            Console.SetCursorPosition(cx, cy + i);
            Console.Write(space);
        }
    }
}