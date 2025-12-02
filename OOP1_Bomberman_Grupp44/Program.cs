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

        Quit();
    }

    public static void Quit()
    {
        Console.Clear();
        Console.WriteLine("Tack för att Ni har spelat!");

        // Om vi inte stoppar KeyInput här så kommer den fortsätta köras i bakgrunden
        // tills konsolen stängs ner (Det blir rätt laggigt när det blir 20 stycken)
        KeyInput.Stop(); 

        Environment.Exit(0);
    }
}