using System.Numerics;
using System.Reflection.Metadata;
using Bomberman.Block;
using Bomberman.PlayerLogic;
using Bomberman.Powerups;

namespace Bomberman;

class Game
{
    private const int FPS = 10;
    private static int FrameDurationMs => 1000 / FPS;
    private static double PowerupChance = 0.25;
    private static Random random = new Random();

    // Definierar hur många tecken marginal som ska finnas på varje sida av spelet
    public static readonly (int Top, int Bottom, int Left, int Right) 
        LevelMargin =
    (
        Top: 7,
        Bottom: 1,
        Left: 2,
        Right: 5
    );

    // Definierar hur många tecken varje position i rutnätet består av
    public const int BlockCharWidth = 7;
    public const int BlockCharHeight = 3;

    private int MinConsoleWidth =>
        (Level.Width * BlockCharWidth) 
        + LevelMargin.Left 
        + LevelMargin.Right;

    private int MinConsoleHeight =>
        (Level.Height * BlockCharHeight) 
        + LevelMargin.Top 
        + LevelMargin.Bottom;

    private bool InvalidConsoleSize() =>
        Console.WindowHeight < MinConsoleHeight ||
        Console.WindowWidth < MinConsoleWidth;

    private EmptySpace emptySpace = new EmptySpace();

    /*
    1. KRAV 4: Objektkomposition
    2. Game har en egenskap av typen Level som håller koll på alla objekt (block, spelare, bomber, powerups)
    som ligger på spelplanen. 
    3. Detta gör vi för att förenkla Game-klassen, då Level har informationen om alla objekt på spelplanen,
    men game är å andra sidan ansvarig för gameloopen, att rita ut banan och hantera interaktionen mellan
    de olika objekten. Genom att ha Level som en separat klass minskar vi alltså Game-klassens ansvar
    eftersom den inte behöver instansiera banan själv, och vi tillåts återställa banan separat från game.
    */

    public Level Level { get; private set; }

    public Game(Level level)
    {
        this.Level = level;
    }

    public Player? GameLoop()
    {
        Console.Clear();

        if (InvalidConsoleSize())
            EnsureValidConsoleSize();

        DrawBorder();
        InitialDraw();
        DrawTitle();
        DrawScoreBoard();

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
                DrawScoreBoard();
            }

            List<string> input = KeyInput.ReadAll();

            UpdatePlayers(input);
            UpdateBombs();

            Level.Powerups.RemoveAll(powerup => powerup.HasBeenUsed);

            // Tillfällig break condition
            if (input.Contains(ConsoleKey.Escape.ToString()))
                Program.Quit();

            Thread.Sleep(FrameDurationMs);

            if (TryGetWinner(out Player? winner))
            {
                return winner; // Null om spelet är oavgjort
            }
        }
    }

    private bool TryGetWinner(out Player? winner)
    {
        winner = null;
        int aliveCount = 0;
        Player? lastAlive = null;
        foreach (Player player in Level.Players)
        {
            if (player.IsAlive)
            {
                aliveCount++;
                lastAlive = player;
            }
        }
        if (aliveCount == 1 && lastAlive != null)
        {
            winner = lastAlive;
            return true;
        }
        else if (aliveCount == 0) return true; // Oavgjort - returnera true men lämna winner som null
        else return false;
    }

    private void UpdatePlayers(List<string> input)
    {
        foreach (Player player in Level.Players.Where(player => player.IsAlive))
        {
            // Spara spelarens position innan och efter inputhanteringen.
            (int x1, int y1) = (player.X, player.Y);
            var bombCountBefore = player.AvailableBombs;
            player.HandleInput(input, Level);
            (int x2, int y2) = (player.X, player.Y);
            var bombCountAfter = player.AvailableBombs;
            if (bombCountBefore != bombCountAfter) DrawScoreBoard();

            // Om spelaren har rört på sig - rita om gamla och nya positionen.
            // Detta är för att undvika onödiga redraws.
            if (x1 != x2 || y1 != y2)
            {
                RedrawPosition(x1, y1);
                RedrawPosition(x2, y2);
            }

            if (Level.TryGetPowerupAt(player.X, player.Y, out var powerup) &&
                powerup.HasBeenUsed == false)
            {
                powerup.Use(player, Level, this);
                DrawScoreBoard();
            }
        }
    }

    private void UpdateBombs()
    {
        // Uppdatera alla bomber
        foreach (Bomb bomb in Level.Bombs)
        {
            // Om bomben exploderar så returnerar Update-metoden
            // en lista med sprängda positioner.
            bool explode = bomb.Update();
            if (!explode) continue;
            

            var affectedblocks = bomb.GetAffectedTiles(Level);

            // Ta bort alla block inom de sprängda positionerna
            foreach (var (x, y) in affectedblocks)
            {   
                if (Level.TryGetBlockAt(x, y, out IBlock? block))
                {
                    bool destroyed = block.TryDestroy();
                    
                    if (destroyed)
                        TrySpawnPowerup(x, y);
                }
                RedrawPosition(x, y);
            }

            // Skada alla spelare inom de sprängda positionerna
            foreach (var (x, y) in affectedblocks)
            {
                if (Level.TryGetPlayerAt(x, y, out Player? player))
                {
                    player.TakeDamage();
                }
                RedrawPosition(x, y);
            }
            DrawScoreBoard();
        }

        // Tar bort färdigexploderade bomber från listan
        Level.Bombs.RemoveAll(b =>
        {
            if (b.DoneExploding)
            {
                foreach ((int x, int y) in b.GetAffectedTiles(Level))
                {
                    RedrawPosition(x, y);
                }
            }
            return b.DoneExploding;
        });

        DrawExplosions();
    }

    private void TrySpawnPowerup(int x, int y)
    {
        if (Level.HasCollidibleBlockAt(x, y) ||
            Level.TryGetPowerupAt(x, y, out _))
            return;

        if (random.NextDouble() < PowerupChance)
        {
            Level.SpawnRandomPowerup(x, y);
        }
    }

    private void EnsureValidConsoleSize()
    {
        Console.Clear();
        while (InvalidConsoleSize())
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Fönstret är för litet!");
            Console.WriteLine("För att starta spelet, öka storleken");

            string txt1 = $"Bredd: {Console.WindowWidth}/{MinConsoleWidth}  ";

            ConsoleColor c1 = Console.WindowWidth < MinConsoleWidth
                ? ConsoleColor.Red
                : ConsoleColor.Green;

            string txt2 = $"Höjd: {Console.WindowHeight}/{MinConsoleHeight}  ";

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

    private void DrawTitle()
    {
        // Det här ser bra ut när vi är i spelet, lita på mig
        Console.ForegroundColor = ConsoleColor.White;
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

    private void DrawBorder()
    {
        Console.ForegroundColor = ConsoleColor.White;
        
        int frameWidth = (Level.Width * BlockCharWidth + 2);
        int frameHeight = (Level.Height * BlockCharHeight);

        string topFrame = string.Empty.PadLeft(frameWidth, '▄');
        string bottomFrame = string.Empty.PadLeft(frameWidth, '▀');

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
        frameY = LevelMargin.Top + Level.Height * BlockCharHeight;
        Console.SetCursorPosition(frameX, frameY);
        Console.Write(bottomFrame);
    }

    private void DrawScoreBoard()
    {
        int textHeight = 8;
        foreach(Player player in Level.Players)
        {
            Console.ForegroundColor = player.Color;
            ConsoleUtils.DrawMultiline((Level.Width * BlockCharWidth) +
            LevelMargin.Left + 3, textHeight,
            $"{player.Name}",
            " ",
            $"HP: {player.HP}",
            $"Blast range: {player.BlastRange}",
            $"Available bombs: {player.AvailableBombs}",
            " ", " "
            );
            Console.ResetColor();
            textHeight += 8;
        }
    }


    // Rita ut alla block och spelare. Körs en gång vid spelets start.
    private void InitialDraw()
    {
        for (int y = 0; y < Level.Height; y++)
        {
            for (int x = 0; x < Level.Width; x++)
            {
                RedrawPosition(x, y);
            }
        }
    }

    // Rita ut en position i spelet.
    public void RedrawPosition(int x, int y)
    {
        if (Level.IsOutOfBounds(x, y))
            return;

        // Rita bakgrundsobjekt (block och tomrum)
        if (Level.TryGetBlockAt(x, y, out var block))
        {
            DrawAt(x, y, block);
        }
        else
        {
            DrawAt(x, y, emptySpace);
        }

        // Rita förgrundsobjekt (explosioner, spelare, bomber, powerups)
        if (Level.TryGetPlayerAt(x, y, out var player) && player.IsAlive)
        {
            DrawAt(x, y, player);
        }
        else if (Level.TryGetBombAt(x, y, out var bomb) && !bomb.HasExploded)
        {
            DrawAt(x, y, bomb);
        }
        else if (Level.TryGetPowerupAt(x, y, out var powerup))
        {
            DrawAt(x, y, powerup);
        }
    }

    private void DrawExplosions()
    {
        foreach (Bomb bomb in Level.Bombs
            .Where(bomb => bomb.HasExploded && !bomb.DoneExploding))
        {
            bomb.DrawExplosion(Level);
        }
    }

    // Rita ut ett drawable-objekt på en position
    // (x och y refererar till positioner i spelets rutnät)
    private void DrawAt(int x, int y, IDrawable drawable)
    {
        (int cX, int cY) = ConsoleUtils.GetCursorPosition(x, y);
        try
        {
            Console.SetCursorPosition(cX, cY);
            drawable.DrawAt(cX, cY);
        }
        catch
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            ConsoleUtils.DrawMultiline(cX, cY,
                "[ERROR]",
                "[ERROR]",
                "[ERROR]");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}

internal class EmptySpace : IDrawable
{
    public void DrawAt(int cx, int cy) =>
        ConsoleUtils.DrawFullBlock(cx, cy, ' ');
}