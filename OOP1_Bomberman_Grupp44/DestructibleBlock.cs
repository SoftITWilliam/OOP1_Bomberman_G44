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
        isDestroyed = true;
    }

    public void DrawLine1() => draw();
    public void DrawLine2() => draw();
    public void DrawLine3() => draw();

    private void draw()
    {
        if (isDestroyed)
        {
            ConsoleUtils.WriteWithColor("░░░░░░░", ConsoleColor.DarkGray);
        }
        else
        {
            ConsoleUtils.WriteWithColor("▒▒▒▒▒▒▒", ConsoleColor.White);
        }
    }
 
}