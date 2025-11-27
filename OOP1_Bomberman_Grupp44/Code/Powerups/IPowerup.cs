
using Bomberman.PlayerLogic;

namespace Bomberman.Powerups;

interface IPowerup : IDrawable
{
    int X { get; }
    int Y { get; }
    bool HasBeenUsed { get; }

    // Powerup behöver se nästan allt i spelet för att kunna göra saker
    void Use(Player player, Level level, Game game);

    void DrawBubble(int cx, int cy)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.SetCursorPosition(cx, cy);
        ConsoleUtils.DrawMultiline(cx, cy,
            "▞     ▚",
            "▌     ▐",
            "▚     ▞");
        Console.ForegroundColor = ConsoleColor.White;
    }
}