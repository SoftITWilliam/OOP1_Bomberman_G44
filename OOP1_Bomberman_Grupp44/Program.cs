
namespace Bomberman;

public class Program 
{
    static void Main(string[] args) 
    {
        Console.WriteLine("woah");
        
        
        Game game = new Game();
        game.AddPlayer(new HumanPlayer() { Name = "Human" });
        game.AddPlayer(new AIPlayer() { Name = "Bot" });

        game.CreateLevel_Placeholder();


    }
}