using Bomberman;

class Player : IDrawable
{
    public required string Name { get; init; }
    public required ConsoleColor Color { get; init; }
    public int X { get; private set; }
    public int Y { get; private set; }

    private readonly IControlScheme controls;

    public Player(int startX, int startY, IControlScheme controls)
    {
        (X, Y) = (startX, startY);
        this.controls = controls;
    }

    public void Update()
    {
        var (dx, dy, placedBomb) = controls.GetDirection();
        X += dx;
        Y += dy;
    }
    
    public void PlaceBomb()
    {
        
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