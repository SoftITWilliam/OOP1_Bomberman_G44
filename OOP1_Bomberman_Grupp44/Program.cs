using Bomberman.PlayerLogic;

namespace Bomberman;

public class Program 
{
    static void Main(string[] args)
    {
        Console.CursorVisible = false;
        KeyInput.Start();

        Menu menu = new Menu();
        menu.StartingMenu(); 
        //var test = menu.ChooseLevel();
        //Game game = new Game(test);

        /*Player p1 = new(0, 0, 
            controls: new KeyboardControlScheme(ControlType.Wasd)) 
        { 
            Name = "Human", 
            Color = ConsoleColor.Blue 
        };

        Player p2 = new(game.Level.Width - 1, game.Level.Height - 1, 
            controls: new KeyboardControlScheme(ControlType.Arrows)) 
        { 
            Name = "Also human", 
            Color = ConsoleColor.Red 
        };

        game.Level.AddPlayer(p1);
        game.Level.AddPlayer(p2);

        Console.Clear();
        game.GameLoop();*/
    }
}