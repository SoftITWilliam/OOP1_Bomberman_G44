using Bomberman.PlayerLogic;

namespace Bomberman.Powerups;

public class BombCountPowerup : IPowerup
{
    public int X { get; }
    public int Y { get; }
    public bool HasBeenUsed { get; private set; }

    public BombCountPowerup(int x, int y)
    {
        (X, Y) = (x, y);
    }

    void IPowerup.Use(Player player, Level level, Game game)
    {
        HasBeenUsed = true;
        player.AddAvailableBomb();
    }

    public void DrawAt(int cx, int cy)
    {
        (this as IPowerup).DrawBubble(cx, cy);
        
        Console.SetCursorPosition(cx + 2, cy);
        ConsoleUtils.WriteWithColor("+1", ConsoleColor.Green);
        Console.SetCursorPosition(cx + 2, cy + 1);
        ConsoleUtils.WriteWithColor("▟█▙", ConsoleColor.DarkGray);
        Console.Write(ConsoleUtils.AnsiOrange("¤"));
        Console.SetCursorPosition(cx + 2, cy + 2);
        ConsoleUtils.WriteWithColor("▜█▛ ", ConsoleColor.DarkGray);
    }
}