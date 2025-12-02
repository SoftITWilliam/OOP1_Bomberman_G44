using Bomberman.PlayerLogic;

namespace Bomberman.Powerups;

class RangePowerup : Powerup
{
    public RangePowerup(int x, int y)
    {
        (X, Y) = (x, y);
    }

    public override void Use(Player player, Level level, Game game)
    {
        HasBeenUsed = true;
        player.UpgradeBombs();
    }

    public override void DrawAt(int cx, int cy)
    {
        DrawBubble(cx, cy);

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