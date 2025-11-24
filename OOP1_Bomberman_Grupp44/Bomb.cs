using System.Security.Cryptography.X509Certificates;
using Bomberman;

class Bomb
{
    private int BlastRange;
    public Bomb(Player player, int BlastRange)
    {
        this.BlastRange = BlastRange = 1;
    }
    
    
    
    public List<(int x, int y)> ExplosionRange(Player player, int BlastRange)
    {
        List<(int x, int y)> InRange = new List<(int x, int Y)>();
        int px = player.X;
        int py = player.Y;

        InRange.Add((px, py)); //bombens ruta
        for(int i = 1; i <= BlastRange; i++)
        {
            InRange.Add((px - i, py));
            InRange.Add((px + i, py));
            InRange.Add((px, py - i));
            InRange.Add((px, py + i));
        }
        return InRange;
    }
}