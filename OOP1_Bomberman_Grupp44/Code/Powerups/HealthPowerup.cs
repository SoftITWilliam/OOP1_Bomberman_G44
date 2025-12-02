using Bomberman.PlayerLogic;

namespace Bomberman.Powerups;

class HealthPowerup : Powerup
{
    public HealthPowerup(int x, int y)
    {
        (X, Y) = (x, y);
    }

    public override void Use(Player player, Level level, Game game)
    {
        HasBeenUsed = true;
        player.TakeDamage(-1);
    }

    public override void DrawAt(int cx, int cy)
    {
        DrawBubble(cx, cy);

        Console.ForegroundColor = ConsoleColor.Red;
        ConsoleUtils.DrawMultiline(cx + 1, cy,
        " ▄ ▄ ",
        "▝███▘",
        "  ▀ ");
    }
}