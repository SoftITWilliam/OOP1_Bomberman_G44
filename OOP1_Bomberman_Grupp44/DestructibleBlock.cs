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
    public int X { get => x; }
    public int Y { get => y; }

    public bool Collidible => isDestroyed == false;
    private bool isDestroyed = false;

    public void Destroy()
    {
        isDestroyed = false;
    }

    public void Draw()
    {
        // to do
    }
 
}