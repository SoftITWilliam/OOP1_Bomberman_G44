namespace Bomberman;

class DestructibleBlock : IBlock
{
    private int x;
    private int y;
    public DestructibleBlock(int X, int Y)
    {
        this.x = X;
        this.y = Y;
    }
    public int X => x; 
    public int Y  => y; 

    private bool isDestroyed = false;
    public bool HasCollision => isDestroyed == false;

    public void Destroy()
    {
        isDestroyed = true;
    }

    public void DrawAt(int cx, int cy)
    {
        if (isDestroyed)
            ConsoleUtils.DrawFullBlock(cx, cy, '░', ConsoleColor.DarkGray);
        else
            ConsoleUtils.DrawFullBlock(cx, cy, '▒', ConsoleColor.White);
    }
}