using Bomberman;

class Player : IDrawable
{
    public required string Name { get; init; }
    public required ConsoleColor Color { get; init; }
    public int X { get; private set; }
    public int Y { get; private set; }
    private int BlastRange = 1; //för att skicka in i bomb

    private readonly IControlScheme controls;

    public Player(int startX, int startY, IControlScheme controls)
    {
        (X, Y) = (startX, startY);
        this.controls = controls;
    }

    public void HandleInput(IEnumerable<string> keys, Level level)
    {
        var (dx, dy, placedBomb) = controls.GetDirection(keys);

        if (!level.IsOutOfBounds(X + dx, Y) &&
            !level.HasCollidibleBlockAt(X + dx, Y) &&
            !level.HasBombAt(X + dx, Y))
        {
            X += dx;
        }

        if (!level.IsOutOfBounds(X, Y + dy) &&
            !level.HasCollidibleBlockAt(X, Y + dy) &&
            !level.HasBombAt(X, Y + dy))
        {
            Y += dy;
        }
        if(placedBomb)
        {
            var bomb = PlaceBomb();
            level.AddBomb(bomb);
        }
    }
    
    public Bomb PlaceBomb()
    {
        return new Bomb(this, BlastRange);
    }

    public void DrawAt(int cx, int cy)
    {
        Console.SetCursorPosition(cx + 1, cy);
        ConsoleUtils.WriteWithColor("[@ @]", Color);
        Console.SetCursorPosition(cx + 1, cy + 1);
        ConsoleUtils.WriteWithColor("/( )\\", Color);
        Console.SetCursorPosition(cx + 2, cy + 2);
        ConsoleUtils.WriteWithColor("/ \\", Color);
    }
}