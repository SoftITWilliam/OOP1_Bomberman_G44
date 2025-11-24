
namespace Bomberman;

public class Program 
{
    static void Main(string[] args) 
    {
        Console.WriteLine("woah");
        
        
        Game game = new Game();

        Player p1 = new Player(0, 0) 
        { 
            Name = "Human", 
            Color = ConsoleColor.Blue 
        };
        Player p2 = new Player(Game.LevelWidth - 1, Game.LevelHeight - 1) 
        { 
            Name = "Bot", 
            Color = ConsoleColor.Red 
        };
        game.AddPlayer(p1);
        game.AddPlayer(p2);

        game.CreateLevel_Placeholder();


        game.DrawEverything();
    }
}