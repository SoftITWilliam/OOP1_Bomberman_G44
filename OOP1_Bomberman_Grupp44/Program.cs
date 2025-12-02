using Bomberman.PlayerLogic;

namespace Bomberman;

public class Program 
{
    static void Main(string[] args)
    {
        Console.CursorVisible = false;
        KeyInput.Start();

        Menu menu = new Menu();

        Game? game = null;
        bool continuePlaying = true;
        do
        {
            if (game is null)
                game = menu.StartingMenu();

            Console.Clear();
            var winner = game.GameLoop();

            (game, continuePlaying) = menu.GameOverMenu(winner, game);
        }
        while (continuePlaying);
    
        Console.Clear();
        Console.WriteLine("Tack för att Ni har spelat!");
        Environment.Exit(0);
    }
}