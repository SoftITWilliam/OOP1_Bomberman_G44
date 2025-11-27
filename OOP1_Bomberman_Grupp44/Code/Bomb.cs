using Bomberman;
using Bomberman.PlayerLogic;

class Bomb : IDrawable
{
    private int BlastRange;
    public ConsoleColor Color;
    private const int MsRemaining = 3000;
    private const int MsExplosionTime = 1000;
    private Player? BombOwner;
    private readonly DateTime PlacedTime;
    public DateTime ExplodedTime { get; private set; }
    public bool HasExploded { get; private set; }
    public bool DoneExploding {
        get
        {
            if (!HasExploded) return false;
            var elapsedMs = (DateTime.Now - ExplodedTime).TotalMilliseconds;
            return elapsedMs >= MsExplosionTime;
        }}
    public int X { get; }
    public int Y { get; }

    //konstruktor för bomb beroende av spelare:
    public Bomb(Player player, int BlastRange)
    {
        this.BlastRange = BlastRange;
        BombOwner = player;
        PlacedTime = DateTime.Now;
        HasExploded = false;
        X = player.X;
        Y = player.Y;
        Color = ConsoleColor.DarkRed;
    }
    //konstruktor för game-genererade bomber:
    public Bomb(int X, int Y, int BlastRange, ConsoleColor Color)
    {
        this.BlastRange = BlastRange;
        BombOwner = null;
        PlacedTime = DateTime.Now;
        HasExploded = false;
        this.X = X;
        this.Y = Y;
        this.Color = Color;
    }

    public List<(int x, int y)>? Update()
    {
        var elapsedMs = (DateTime.Now - PlacedTime).TotalMilliseconds;
        if (!HasExploded && elapsedMs >= MsRemaining) return Explode();
        else return null; //returnera ingenting om tiden inte är ute
    }

    private List<(int x, int y)> Explode() //behöver både denna och Update vara public?
    {
        HasExploded = true;
        ExplodedTime = DateTime.Now;
        BombOwner?.AddAvailableBomb();
        return ExplosionRange();
    }


    public List<(int x, int y)> ExplosionRange()
    {
        List<(int x, int y)> InRange = new List<(int x, int Y)>();
        int px = X;
        int py = Y;

        InRange.Add((px, py)); //bombens ruta
        for (int i = 1; i <= BlastRange; i++)
        {
            InRange.Add((px - i, py));
            InRange.Add((px + i, py));
            InRange.Add((px, py - i));
            InRange.Add((px, py + i));
        }
        return InRange;
    }

    public void DrawAt(int cx, int cy)
    {
        Console.SetCursorPosition(cx + 1, cy);
        ConsoleUtils.WriteWithColor("¤^\\", Color);
        Console.SetCursorPosition(cx + 1, cy + 1);
        ConsoleUtils.WriteWithColor("(   )", Color);
        Console.SetCursorPosition(cx + 2, cy + 2);
        ConsoleUtils.WriteWithColor("`-‘", Color);
    }

    private Dictionary<string, string[]> kaboom = new Dictionary<string, string[]>()
    {
        { "ground-zero", ["\\\\|||//", "=BOOOM=", "//|||\\\\"] },
        { "horizontal", ["^^^^^^^", "=======", "vvvvvvv"] },
        { "vertical", ["<|||||>", "<|||||>", "<|||||>"] },

        { "up", ["/~~~~~\\", "\\|||||/", "\\|||||/"] },
        { "down", ["/|||||\\", "/|||||\\", "\\~~~~~/"] },
        { "left", [" /-^^^^", "(-=====", " \\-vvvv"] },
        { "right", ["^^^^-\\ ", "=====-)", "vvvv-/ "] },
    };

    public void DrawExplosion(Level level)
    {
        Console.ForegroundColor = ConsoleColor.Red;

        bool CheckExplosionIsBlocked(int x, int y)
        {
            return level.IsOutOfBounds(x, y) 
                || level.HasCollidibleBlockAt(x, y);
        }

        void DrawExplosion(int x, int y, int i, string[] branchSprite, string[] edgeSprite)
        {
            (int cx, int cy) = ConsoleUtils.GetCursorPosition(x, y);
            var sprite = i == BlastRange ? edgeSprite : branchSprite;
            ConsoleUtils.DrawMultiline(cx, cy, sprite);
        }

        // Center
        (int cx, int cy) = ConsoleUtils.GetCursorPosition(X, Y);
        ConsoleUtils.DrawMultiline(cx, cy, kaboom["ground-zero"]);

        // Vänster gren
        for (int i = 1; i <= BlastRange; i++)
        {
            int x = X - i;
            if (CheckExplosionIsBlocked(x, Y)) break;
            DrawExplosion(x, Y, i, kaboom["horizontal"], kaboom["left"]);
        }

        // Höger gren
        for (int i = 1; i <= BlastRange; i++)
        {
            int x = X + i;
            if (CheckExplosionIsBlocked(x, Y)) break;
            DrawExplosion(x, Y, i, kaboom["horizontal"], kaboom["right"]);
        }

        // Uppåt gren
        for (int i = 1; i <= BlastRange; i++)
        {
            int y = Y - i;
            if (CheckExplosionIsBlocked(X, y)) break;
            (cx, cy) = ConsoleUtils.GetCursorPosition(X, y);
            DrawExplosion(X, y, i, kaboom["vertical"], kaboom["up"]);
        }

        // Neråt gren
        for (int i = 1; i <= BlastRange; i++)
        {
            int y = Y + i;
            if (CheckExplosionIsBlocked(X, y)) break;
            DrawExplosion(X, y, i, kaboom["vertical"], kaboom["down"]);
        }

        Console.ForegroundColor = ConsoleColor.White;

    }
}