using Bomberman;

class Player : IDrawable
{
    public required string Name { get; init; }
    public required ConsoleColor Color { get; init; }
    public int X => x;
    public int Y => y;

    private int x, y;
    
    public Player(int x, int y)
    {
        (this.x, this.y) = (x, y);
    }

    public void DrawLine1()
    {
        ConsoleUtils.WriteWithColor(" [@ @] ", Color);
    }

    public void DrawLine2()
    {
        ConsoleUtils.WriteWithColor(" /( )\\ ", Color);
    }

    public void DrawLine3()
    {
        ConsoleUtils.WriteWithColor("  / \\  ", Color);
    }
}