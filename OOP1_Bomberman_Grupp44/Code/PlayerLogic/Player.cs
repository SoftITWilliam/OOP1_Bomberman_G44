using Microsoft.VisualBasic;

namespace Bomberman.PlayerLogic;

class Player : IDrawable
{
    public required string Name { get; init; }
    public required ConsoleColor Color { get; init; }
    public int X { get; private set; }
    public int Y { get; private set; }
    private int startX, startY;
    public int HP { get; private set; }

    /*
    1. KRAV 3: Computed properties
    
    2. Vår bool IsAlive räknar ut huruvida spelaren lever baserat på dennes HP. Den returnerar true
    så länge HP är större än 0. 

    3. Vi använder en computed property för att garantera ett logiskt tillstånd, eftersom vi inte vill att
    IsAlive ska kunna vara true om HP är 0, eller tvärtom. Dessutom blir koden mer begriplig av att ha både
    IsAlive och HP som egenskaper, eftersom det är tydligare att utanför klassen läsa av boolen istället
    för att läsa av värdet på HP.
    */

    public bool IsAlive => HP > 0;
    public int BlastRange { get; private set; } = 1; //för att skicka in i bomb
    public int AvailableBombs = 1;

    private readonly IControlScheme controls;
    
    /*
    1. KRAV 5: Beroendeinjektion

    2. I konstruktorn för player injicerar vi ett kontrollschema som är en subtyp till IControlScheme
    som avgör hur spelaren styrs. I HandleInput() anropas metoden GetDirection() på det injicerade kontrollschemat,
    vilket kontrollerar spelarens handlingar. Beroende på vilket kontrollschema vi väljer kan styrningen ske
    antingen med tangentbordet eller automatiskt.

    3. Användningen av injektion här tillåter oss att skilja åt mänskliga- och botspelare, utan att
    skapa subtyper beroende på hur spelaren styrs, och detta är en lämplig lösning eftersom styrningen är det enda
    som skiljer botspelare och mänskliga spelare åt. I annat fall hade vi behövt ha två nästintill identiska klasser
    för att uppfylla samma syfte. Beroendeinjektionen gör också koden mer flexibel eftersom vi möjliggör för att i
    framtiden kunna lägga till ytterligare kontrollscheman utan att behöva ändra i Player-koden.
    */

    public Player(int startX, int startY, IControlScheme controls)
    {
        (this.startX, this.startY) = (startX, startY);
        this.controls = controls;
        Reset();
    }

    public void Reset()
    {
        HP = 2;
        BlastRange = 1;
        AvailableBombs = 1;
        (X, Y) = (this.startX, this.startY);        
    }

    public void HandleInput(IEnumerable<string> keys, Level level)
    {
        var (dx, dy, placedBomb) = controls.GetDirection(keys);

        if (placedBomb)
        {
            var bomb = PlaceBomb();
            if (bomb != null) level.AddBomb(bomb);
        }

        if (!level.IsOutOfBounds(X + dx, Y) &&
            !level.HasCollidibleBlockAt(X + dx, Y) &&
            !level.HasBombAt(X + dx, Y) &&
            !level.HasPlayerAt(X + dx, Y))
        {
            X += dx;
        }

        if (!level.IsOutOfBounds(X, Y + dy) &&
            !level.HasCollidibleBlockAt(X, Y + dy) &&
            !level.HasBombAt(X, Y + dy) &&
            !level.HasPlayerAt(X, Y + dy))
        {
            Y += dy;
        }
    }

    public void TakeDamage(int amount = 1)
    {
        HP = Math.Max(0, HP - amount);
    }

    public void AddAvailableBomb()
    {
        AvailableBombs += 1;
    }

    public void UpgradeBombs()
    {
        BlastRange += 1;
    }

    public Bomb? PlaceBomb()
    {
        if (AvailableBombs <= 0 || !IsAlive) 
            return null;

        AvailableBombs -= 1;
        return new Bomb(this, BlastRange);
    }

    public void DrawAt(int cx, int cy)
    {
        Console.SetCursorPosition(cx + 1, cy);
        ConsoleUtils.WriteWithColor("[@ @]", Color);
        Console.SetCursorPosition(cx + 1, cy + 1);
        ConsoleUtils.WriteWithColor("/( )\\", Color);
        Console.SetCursorPosition(cx + 2, cy + 2);
        ConsoleUtils.WriteWithColor("/ \\", Color);
    }
}