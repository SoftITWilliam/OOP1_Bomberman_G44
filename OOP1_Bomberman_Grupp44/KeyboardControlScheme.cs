using System.Net;
using System.Runtime.InteropServices;
using Bomberman;

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
                placebomb = ConsoleKey.Enter;
                break;
        }
    }
    public (int dx, int dy, bool placedBomb) GetDirection()
    {
        int dx = 0;
        int dy = 0;
        bool placedBomb = false;

        if (!Console.KeyAvailable) return (0, 0, false);

        var key = Console.ReadKey(true).Key;

        if (key == up) dy = 1;
        if (key == down) dy = -1;
        if (key == left) dx = -1;
        if (key == right) dx = 1;
        if (key == placebomb) placedBomb = true;

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