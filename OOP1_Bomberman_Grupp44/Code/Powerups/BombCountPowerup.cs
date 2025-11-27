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
    }
}