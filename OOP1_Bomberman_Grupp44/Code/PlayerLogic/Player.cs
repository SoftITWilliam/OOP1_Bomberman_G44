namespace Bomberman.PlayerLogic;

class Player : IDrawable
{
    public required string Name { get; init; } //nödvändig? förmodligen för scoreboard
    public required ConsoleColor Color { get; init; } //skulle kunna ändras t private senare!
    public int X { get; private set; }
    public int Y { get; private set; }
    public int HP { get; private set; }
    public bool IsAlive => HP > 0;
    public int BlastRange { get; private set; } = 1; //för att skicka in i bomb
    private int AvailableBombs = 1;
    private readonly IControlScheme controls;

    public Player(int startX, int startY, IControlScheme controls)
    {
        (X, Y) = (startX, startY);
        this.controls = controls;
        HP = 2;
    }

    public void HandleInput(IEnumerable<string> keys, Level level)
    {
        var (dx, dy, placedBomb) = controls.GetDirection(keys);

        if (!level.IsOutOfBounds(X + dx, Y) &&
            !level.HasCollidibleBlockAt(X + dx, Y) &&
            !level.HasBombAt(X + dx, Y) &&
            !level.HasPlayerAt(X + dx, Y))
        {
            X += dx;
        }

        if (!level.IsOutOfBounds(X, Y + dy) &&
            !level.HasCollidibleBlockAt(X, Y + dy) &&
            !level.HasBombAt(X, Y + dy) &&
            !level.HasPlayerAt(X, Y + dy))
        {
            Y += dy;
        }
        if (placedBomb)
        {
            var bomb = PlaceBomb();
            if (bomb != null) level.AddBomb(bomb);
        }
    }

    public void TakeDamage(int amount = 1)
    {
        HP = Math.Max(0, HP - amount);
    }

    public void AddAvailableBomb()
    {
        AvailableBombs += 1;
    }

    public void UpgradeBombs()
    {
        BlastRange += 1;
    }

    public Bomb? PlaceBomb()
    {
        if (AvailableBombs <= 0 || !IsAlive) 
            return null;

        AvailableBombs -= 1;
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