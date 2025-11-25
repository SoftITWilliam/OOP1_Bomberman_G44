
namespace Bomberman;

public class Program 
{
    static void Main(string[] args) 
    {
        Game game = new Game();

        Player p1 = new(0, 0, 
            controls: new KeyboardControlScheme(ControlType.Wasd)) 
        { 
            Name = "Human", 
            Color = ConsoleColor.Blue 
        };

        Player p2 = new(game.Level.Width - 1, game.Level.Height - 1, 
            controls: new KeyboardControlScheme(ControlType.Arrows)) 
        { 
            Name = "Bot", 
            Color = ConsoleColor.Red 
        };

        KeyInput.Start();

        game.AddPlayer(p1);
        game.AddPlayer(p2);

        Console.Clear();
        game.Start();

    
        
    }
}