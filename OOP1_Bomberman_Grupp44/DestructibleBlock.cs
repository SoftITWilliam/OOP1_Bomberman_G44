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
        for (int i = 0; i < 3; i++)
        {
            Console.SetCursorPosition(cx, cy + i);
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
}