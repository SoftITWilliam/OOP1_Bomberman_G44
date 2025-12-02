using Bomberman.PlayerLogic;

namespace Bomberman.Powerups;

public class BombRainPowerup : IPowerup
{
    private const int MinBombCount = 5;
    private const int MaxBombCount = 10;

    public int X { get; }
    public int Y { get; }
    public bool HasBeenUsed { get; private set; }

    public BombRainPowerup(int x, int y)
    {
        (X, Y) = (x, y);
    }

    void IPowerup.Use(Player player, Level level, Game game)
    {
        HasBeenUsed = true;

        Random rand = new Random();
        int bombCount = rand.Next(MinBombCount, MaxBombCount + 1);
        // Extra break-villkor så att vi inte kan fastna i all oändlighet:
        int iterations = 0; 
        while (bombCount > 0 && iterations < 1000)
        {
            iterations++;

            int x = rand.Next(0, level.Width);
            int y = rand.Next(0, level.Height);

            bool hasBlock = level.HasCollidibleBlockAt(x, y);

            bool tooCloseToPlayer = 
                player.X - x <= 2 && player.X - x >= -2 &&
                player.Y - y <= 2 && player.Y - y >= -2;

            if (hasBlock || tooCloseToPlayer)
                continue;

            level.AddBomb(
                new Bomb(x, y, player.BlastRange, ConsoleColor.Yellow));

            game.RedrawPosition(x, y);
            bombCount--;
        }
    }

    public void DrawAt(int cx, int cy)
    {
        (this as IPowerup).DrawBubble(cx, cy);   

        Console.SetCursorPosition(cx + 1, cy);
        ConsoleUtils.WriteWithColor("▟███▙", ConsoleColor.DarkGray);
        Console.SetCursorPosition(cx + 1, cy + 1);
        ConsoleUtils.WriteWithColor(" ::: ", ConsoleColor.Blue);
        Console.SetCursorPosition(cx + 1, cy + 2);
        ConsoleUtils.WriteWithColor(" ::: ", ConsoleColor.Blue);
    }
}