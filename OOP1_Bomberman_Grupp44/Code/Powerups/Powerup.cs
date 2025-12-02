
using Bomberman.PlayerLogic;

namespace Bomberman.Powerups;

/*
    1. KRAV 7: Subtypspolymorfism #2
    
    2. Vi använder en abstrakt klass för Powerups som våra övriga Powerup-klasser (dvs BombCountPowerup,
    HealthPowerup, BombRainPowerup och RangePowerup) ärver av. Superklassen innehåller objektets position
    och abstrakta metoder för att aktivera samt rita ut en powerup.

    3. De abstrakta medlemmarna i superklassen ger subklasserna kontroll över hur de ritas ut och vad som händer
    när en spelare plockar upp dem, och eftersom detta beteende varierar mellan olika Powerups vill vi att
    ansvaret ska ligga hos subtyperna. I spelets kod kan vi nu behandla alla Powerups som supertypen Powerup,
    vilket bidrar till en tydligare och mer läsbar kod. Att abstrahera med subtypspolymorfism på det här sättet tillåter
    oss också att enkelt lägga till nya Powerups. Eftersom vi använder oss av en abstrakt klass istället för en interface
    så kan vi kapsla in den gemensamma metoden DrawBubble(), samt att de inte behöver implementera egna definitioner för
    de konkreta medlemmarna. I det stora hela undviker vi onödig upprepning av kod.
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