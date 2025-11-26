namespace Bomberman;

class SolidBlock : IBlock
{
    private readonly int x, y;
    public int X => x; //? ska den ha en setter?
    public int Y  => y;

    public bool Collidible => true; // Har alltid kollision

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
        Console.SetCursorPosition(cx, cy);
        Console.Write("██▖████");
        Console.SetCursorPosition(cx, cy + 1);
        Console.Write("▆▆▆▆▆▖▆");
        Console.SetCursorPosition(cx, cy + 2);
        Console.Write("▆▆▖▆▆▆▆");
        Console.BackgroundColor = ConsoleColor.Black;
    }
}
    