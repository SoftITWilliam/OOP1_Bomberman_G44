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
    public bool Collidible => isDestroyed == false;

    public void Destroy()
    {
        isDestroyed = false;
    }

    public void Draw()
    {
        // to do
    }
 
}