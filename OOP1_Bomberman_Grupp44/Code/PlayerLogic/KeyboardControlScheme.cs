namespace Bomberman.PlayerLogic;

public enum ControlType
{
    Wasd, Arrows
}

class KeyboardControlScheme : IControlScheme
{
    private ConsoleKey up;
    private ConsoleKey down;
    private ConsoleKey left;
    private ConsoleKey right;
    private ConsoleKey placebomb;

    public KeyboardControlScheme(ControlType type)
    {
        switch(type)
        {
            case ControlType.Wasd:
                up = ConsoleKey.W;
                down = ConsoleKey.S;
                left = ConsoleKey.A;
                right = ConsoleKey.D;
                placebomb = ConsoleKey.Spacebar;
                break;
            case ControlType.Arrows:
                up = ConsoleKey.UpArrow;
                down = ConsoleKey.DownArrow;
                left = ConsoleKey.LeftArrow;
                right = ConsoleKey.RightArrow;
                placebomb = ConsoleKey.OemPeriod;
                break;
        }
    }
    public (int dx, int dy, bool placedBomb) GetDirection(IEnumerable<string> keys)
    {
        var keysList = keys.ToList();

        int dx = 0;
        int dy = 0;
        bool placedBomb = false;

        if (keysList.Contains(up.ToString())) dy = -1;
        if (keysList.Contains(down.ToString())) dy = 1;
        if (keysList.Contains(left.ToString())) dx = -1;
        if (keysList.Contains(right.ToString())) dx = 1;
        if (keysList.Contains(placebomb.ToString())) placedBomb = true;

        return (dx, dy, placedBomb);
    }

    /*public bool PlacedBomb() //om man vill ha placed bomb separat?
{
   bool placedBomb = false;
   var key = Console.ReadKey(true).Key;
   if (key == placebomb) placedBomb = true;
   return placedBomb;
}*/
}