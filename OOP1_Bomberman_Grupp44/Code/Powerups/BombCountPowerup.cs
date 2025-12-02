using Bomberman.PlayerLogic;

namespace Bomberman.Powerups;

class BombCountPowerup : Powerup
{
    public BombCountPowerup(int x, int y)
    {
        (X, Y) = (x, y);
    }

    public override void Use(Player player, Level level, Game game)
    {
        HasBeenUsed = true;
        player.AddAvailableBomb();
    }

    public override void DrawAt(int cx, int cy)
    {
        DrawBubble(cx, cy);
        
        Console.SetCursorPosition(cx + 2, cy);
        ConsoleUtils.WriteWithColor("+1", ConsoleColor.Green);
        Console.SetCursorPosition(cx + 2, cy + 1);
        ConsoleUtils.WriteWithColor("▟█▙", ConsoleColor.DarkGray);
        Console.Write(ConsoleUtils.AnsiOrange("¤"));
        Console.SetCursorPosition(cx + 2, cy + 2);
        ConsoleUtils.WriteWithColor("▜█▛ ", ConsoleColor.DarkGray);
    }
}