namespace Bomberman.Block;

class DestructibleBlock : IBlock
{
    private readonly int x, y;
    public int X => x; 
    public int Y  => y; 
    public DestructibleBlock(int X, int Y)
    {
        x = X;
        y = Y;
    }
    private bool isDestroyed = false;
    public bool HasCollision => isDestroyed == false;
    public bool TryDestroy()
    {
        if (isDestroyed) return false;

        isDestroyed = true;
        return true;
    }
    public void DrawAt(int cx, int cy)
    {
        if (isDestroyed)
            ConsoleUtils.DrawFullBlock(cx, cy, '░', ConsoleColor.DarkGray);
        else
            ConsoleUtils.DrawFullBlock(cx, cy, '▒', ConsoleColor.White);
    }
}