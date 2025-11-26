namespace Bomberman;

class SolidBlock : IBlock
{
    private readonly int x, y;
    public int X => x; //? ska den ha en setter?
    public int Y  => y;

    public bool HasCollision => true; // Har alltid kollision

    public SolidBlock(int X, int Y)
    {
        x = X;
        y = Y;
    }

    public void Destroy()
    {
        // Kan inte förstöras - gör ingenting här
    }

    public void DrawAt(int cx, int cy)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.BackgroundColor = ConsoleColor.Gray;

        if (this.Y % 2 == 0)
        {
            ConsoleUtils.DrawMultiline(cx, cy,
                "▆▆▖▆▆▆▆",
                "▆▆▆▆▆▖▆",
                "▆▆▖▆▆▆▆"
            );
        }
        else
        {
            ConsoleUtils.DrawMultiline(cx, cy,
                "▆▆▆▆▆▖▆",
                "▆▆▖▆▆▆▆",
                "▆▆▆▆▆▖▆"
            );
        }
        
        Console.BackgroundColor = ConsoleColor.Black;
    }
}
    