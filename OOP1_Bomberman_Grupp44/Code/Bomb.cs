using Bomberman;
using Bomberman.PlayerLogic;

class Bomb : IDrawable
{
    private readonly int blastRange;
    private readonly ConsoleColor color;
    private const int MsRemaining = 3000;
    private const int MsExplosionTime = 1000;
    private readonly Player? bombOwner;
    private readonly DateTime placedTime;
    private DateTime TimeOfExplosion { get; set; } //byt namn på variabeln?
    public bool HasExploded { get; private set; }
    public bool DoneExploding { get
        {
            if (!HasExploded) return false;
            var elapsedMs = (DateTime.Now - TimeOfExplosion).TotalMilliseconds;
            return elapsedMs >= MsExplosionTime;
        }}
    public int X { get; }
    public int Y { get; }

    //konstruktor för bomb beroende av spelare:
    public Bomb(Player player, int BlastRange)
    {
        this.blastRange = BlastRange;
        bombOwner = player;
        placedTime = DateTime.Now;
        HasExploded = false;
        X = player.X;
        Y = player.Y;
        color = ConsoleColor.DarkRed;
    }
    //konstruktor för game-genererade bomber:
    public Bomb(int X, int Y, int BlastRange, ConsoleColor Color)
    {
        this.blastRange = BlastRange;
        bombOwner = null;
        placedTime = DateTime.Now;
        HasExploded = false;
        this.X = X;
        this.Y = Y;
        this.color = Color;
    }

    // Körs varje frame.
    // Metoden kollar bombens timer. När bomben exploderar så returnerar den true
    public bool Update()
    {
        var elapsedMs = (DateTime.Now - placedTime).TotalMilliseconds;
        if (!HasExploded && elapsedMs >= MsRemaining)
        {
            Explode();
            return true;
        } 
        else return false; //returnera ingenting om tiden inte är ute
    }

    private void Explode()
    {
        HasExploded = true;
        TimeOfExplosion = DateTime.Now;
        bombOwner?.AddAvailableBomb();
    }

    // Returnerar alla koordinater som påverkas av explosionen.
    // Hanterar automatiskt koordinater som går utanför banan eller som blockeras av solida block
    public List<(int x, int y)> GetAffectedTiles(Level level)
    {
        List<(int x, int y)> InRange = new List<(int x, int Y)>();
        int px = X;
        int py = Y;

        InRange.Add((px, py)); //bombens egen ruta

        // Vänster
        for (int x = px; x >= px - blastRange; x--)
        {
            if (CheckExplosionIsBlocked(level, x, py)) break;
            InRange.Add((x, py));
        }
        // Höger
        for (int x = px; x <= px + blastRange; x++)
        {
            if (CheckExplosionIsBlocked(level, x, py)) break;
            InRange.Add((x, py));
        }
        // Upp
        for (int y = py; y >= py - blastRange; y--)
        {
            if (CheckExplosionIsBlocked(level, px, y)) break;
            InRange.Add((px, y));
        }
        // Ner
        for (int y = py; y <= py + blastRange; y++)
        {
            if (CheckExplosionIsBlocked(level, px, y)) break;
            InRange.Add((px, y));
        }
        return InRange;
    }

    // Rita ut bomben på banan
    public void DrawAt(int cx, int cy)
    {
        Console.SetCursorPosition(cx + 1, cy);
        ConsoleUtils.WriteWithColor("¤^\\", color);
        Console.SetCursorPosition(cx + 1, cy + 1);
        ConsoleUtils.WriteWithColor("(   )", color);
        Console.SetCursorPosition(cx + 2, cy + 2);
        ConsoleUtils.WriteWithColor("`-‘", color);
    }

    private static bool CheckExplosionIsBlocked(Level level, int x, int y) =>
        level.IsOutOfBounds(x, y) || level.HasSolidBlockAt(x, y);
    

    // Alla "bitar" av explosionen som ritas ut
    private Dictionary<string, string[]> kaboom = new()
    {
        { "ground-zero", ["\\\\|||//", "=BOOOM=", "//|||\\\\"] },
        { "horizontal", ["^^^^^^^", "=======", "vvvvvvv"] },
        { "vertical", ["<|||||>", "<|||||>", "<|||||>"] },

        { "up", ["/~~~~~\\", "\\|||||/", "\\|||||/"] },
        { "down", ["/|||||\\", "/|||||\\", "\\~~~~~/"] },
        { "left", [" /-^^^^", "(-=====", " \\-vvvv"] },
        { "right", ["^^^^-\\ ", "=====-)", "vvvv-/ "] },
    };

    // Rita ut hela explosionen på banan -- hanterar OutOfBounds och solida block
    public void DrawExplosion(Level level)
    {
        void DrawExplosion(int x, int y, int i, 
            string[] branchSprite, string[] edgeSprite)
        {
            (int cx, int cy) = ConsoleUtils.GetCursorPosition(x, y);
            var sprite = i == blastRange ? edgeSprite : branchSprite;
            
            ConsoleUtils.DrawMultiline(cx, cy, ConsoleUtils.AnsiOrange(sprite));
        }

        // Center
        (int cx, int cy) = ConsoleUtils.GetCursorPosition(X, Y);
        string[] sprite = kaboom["ground-zero"];
        ConsoleUtils.DrawMultiline(cx, cy, ConsoleUtils.AnsiOrange(sprite));

        // Vänster gren
        for (int i = 1; i <= blastRange; i++)
        {
            int x = X - i;
            if (CheckExplosionIsBlocked(level, x, Y)) break;
            DrawExplosion(x, Y, i, kaboom["horizontal"], kaboom["left"]);
        }

        // Höger gren
        for (int i = 1; i <= blastRange; i++)
        {
            int x = X + i;
            if (CheckExplosionIsBlocked(level, x, Y)) break;
            DrawExplosion(x, Y, i, kaboom["horizontal"], kaboom["right"]);
        }

        // Uppåt gren
        for (int i = 1; i <= blastRange; i++)
        {
            int y = Y - i;
            if (CheckExplosionIsBlocked(level, X, y)) break;
            (cx, cy) = ConsoleUtils.GetCursorPosition(X, y);
            DrawExplosion(X, y, i, kaboom["vertical"], kaboom["up"]);
        }

        // Neråt gren
        for (int i = 1; i <= blastRange; i++)
        {
            int y = Y + i;
            if (CheckExplosionIsBlocked(level, X, y)) break;
            DrawExplosion(X, y, i, kaboom["vertical"], kaboom["down"]);
        }
    }
}