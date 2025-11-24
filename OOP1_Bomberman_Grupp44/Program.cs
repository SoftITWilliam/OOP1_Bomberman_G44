
namespace Bomberman;

public class Program 
{
    static void Main(string[] args) 
    {
        Console.WriteLine("woah");
        
        
        Game game = new Game();


        Player p1 = new Player(0, 0, new KeyboardControlScheme(ControlType.Wasd)) 
        { 
            Name = "Human", 
            Color = ConsoleColor.Blue 
        };

        Player p2 = new Player(Game.LevelWidth - 1, Game.LevelHeight - 1, 
            new KeyboardControlScheme(ControlType.Arrows)) 
        { 
            Name = "Bot", 
            Color = ConsoleColor.Red 
        };

        KeyInput.Start();

        game.AddPlayer(p1);
        game.AddPlayer(p2);

        game.CreateLevel_Placeholder();

        Console.Clear();
        game.Start();
    }
}