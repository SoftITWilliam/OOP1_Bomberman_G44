namespace Bomberman.Block;

/*
    1. KRAV 6: Subtypspolymorfism #1
    
    2. Vi har en gemensam interface som vi använder för båda våra blocktyper (dvs SolidBlock och DestructibleBlock)
    Blocktyperna skiljs åt i hur de ritas ut, och hur de hanterar att bli sprängda; det vill säga inte enbart i sin data.
    När en bomb sprängs behandlas alla påverkade block som IBlocks när TryDestroy() anropas. Dynamic Dispatch avgör hur
    blocket hanterar explosionen; DestructibleBlocks förstörs, och SolidBlocks är opåverkade.

    3. Genom att använda subtypspolymorfism på det här sättet undviker vi conditionals i resten av spelets kod. 
    På det här sättet blir koden tydligare och lättare att läsa. Koden blir också mer flexibel då vi enkelt kan lägga till
    nya blocktyper utan att ändra i övrig kod.
*/

public interface IBlock : IDrawable
{
    public int X{ get; }
    public int Y { get; }
    bool HasCollision { get; }

    bool TryDestroy();
}