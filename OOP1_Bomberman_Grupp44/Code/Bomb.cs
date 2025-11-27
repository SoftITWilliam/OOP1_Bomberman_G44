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

    public List<(int x, int y)>? Update()
    {
        var elapsedMs = (DateTime.Now - placedTime).TotalMilliseconds;
        if (!HasExploded && elapsedMs >= MsRemaining) return Explode();
        else return null; //returnera ingenting om tiden inte är ute
    }

    private List<(int x, int y)> Explode()
    {
        HasExploded = true;
        TimeOfExplosion = DateTime.Now;
        bombOwner?.AddAvailableBomb();
        return ExplosionRange();
    }

    private List<(int x, int y)> ExplosionRange()
    {
        List<(int x, int y)> InRange = new List<(int x, int Y)>();
        int px = X;
        int py = Y;

        InRange.Add((px, py)); //bombens egen ruta
        for (int i = 1; i <= blastRange; i++)
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
        ConsoleUtils.WriteWithColor("¤^\\", color);
        Console.SetCursorPosition(cx + 1, cy + 1);
        ConsoleUtils.WriteWithColor("(   )", color);
        Console.SetCursorPosition(cx + 2, cy + 2);
        ConsoleUtils.WriteWithColor("`-‘", color);
    }
}