using Bomberman.PlayerLogic;

namespace Bomberman.Powerups;

public class RangePowerup : IPowerup
{
    public int X { get; }
    public int Y { get; }
    public bool HasBeenUsed { get; private set; }

    public RangePowerup(int x, int y)
    {
        (X, Y) = (x, y);
    }

    void IPowerup.Use(Player player, Level level, Game game)
    {
        HasBeenUsed = true;
        player.UpgradeBombs();
    }

    public void DrawAt(int cx, int cy)
    {
        (this as IPowerup).DrawBubble(cx, cy);

        
        //ConsoleUtils.WriteWithColor("+1", ConsoleColor.Green);

        Console.SetCursorPosition(cx + 2, cy);
        Console.Write(ConsoleUtils.AnsiOrange("▖▄▗"));
        Console.SetCursorPosition(cx + 1, cy + 1);
        Console.Write("←");
        Console.Write(ConsoleUtils.AnsiOrange("▐█▌"));
        Console.Write("→");
        Console.SetCursorPosition(cx + 2, cy + 2);
        Console.Write(ConsoleUtils.AnsiOrange("▘▀▝"));
    }
}