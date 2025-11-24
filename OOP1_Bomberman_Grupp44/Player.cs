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
    
    public void HandleInput(IEnumerable<string> keys)
    {
        var (dx, dy, placedBomb) = controls.GetDirection(keys);

        if (X + dx >= 0 && X + dx < Game.LevelWidth)
            X += dx;

        if (Y + dy >= 0 && Y + dy < Game.LevelHeight)
            Y += dy;
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