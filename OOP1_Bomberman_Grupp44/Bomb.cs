using System.Security.Cryptography.X509Certificates;
using Bomberman;

class Bomb : IDrawable
{
    private int BlastRange;
    private const int MsRemaining = 3000;
    private Player BombOwner;
    private readonly DateTime PlacedTime;
    public bool HasExploded { get; private set; }

    public Bomb(Player player, int BlastRange)
    {
        this.BlastRange = BlastRange;
        BombOwner = player;
        PlacedTime = DateTime.Now;
        HasExploded = false;
    }
    
    public List<(int x, int y)>? Update()
    {
        var elapsedMs = (DateTime.Now - PlacedTime).TotalMilliseconds;
        if (!HasExploded && elapsedMs >= MsRemaining) return Explode();
        else return null; //returnera ingenting om tiden inte är ute
    }

    public List<(int x, int y)> Explode() //behöver både denna och Update vara public?
    {
        HasExploded = true;
        return ExplosionRange();
    }


    public List<(int x, int y)> ExplosionRange()
    {
        List<(int x, int y)> InRange = new List<(int x, int Y)>();
        int px = BombOwner.X;
        int py = BombOwner.Y;

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

    public void DrawLine1()
    {
        Console.WriteLine("bomb");
    }
     public void DrawLine2()
    {
        Console.WriteLine("bomb");
    }
     public void DrawLine3()
    {
        Console.WriteLine("bomb");
    }

}