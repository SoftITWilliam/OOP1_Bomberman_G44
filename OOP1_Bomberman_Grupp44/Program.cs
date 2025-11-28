using Bomberman.PlayerLogic;

namespace Bomberman;

public class Program 
{
    static void Main(string[] args)
    {
        Console.CursorVisible = false;
        KeyInput.Start();

        Menu menu = new Menu();
        var game = menu.StartingMenu();
        Console.Clear();
        game.GameLoop();
    }
}