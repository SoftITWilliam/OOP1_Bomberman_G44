
using Bomberman.PlayerLogic;

namespace Bomberman.Powerups;

/*
    1. KRAV 7: Subtypspolymorfism #2
    2. 
    3.
*/

abstract class Powerup : IDrawable
{
    public int X { get; protected set; }
    public int Y { get; protected set; }
    public bool HasBeenUsed { get; protected set; }

    // Powerup behöver se nästan allt i spelet för att kunna göra saker
    public abstract void Use(Player player, Level level, Game game);
    public abstract void DrawAt(int cx, int cy);

    protected void DrawBubble(int cx, int cy)
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