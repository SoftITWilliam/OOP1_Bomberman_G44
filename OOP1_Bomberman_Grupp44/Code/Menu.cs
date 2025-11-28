using System.Transactions;
using System.Xml;
using Bomberman.Block;
using Bomberman.PlayerLogic;

namespace Bomberman;

class Menu
{
    private KeyboardControlScheme controls = new KeyboardControlScheme(ControlType.MenuControls);
    private List<Level> LevelTypes = new List<Level>()
    {
        Level.ClassicLevel(), Level.StarLevel(), Level.TestLevel()
    };
    private List<string> LevelNames = new List<string>()
    {
        "Classic level", "Star Level", "Test level"
    };
    private List<string> StartOptions = new List<string>()
    {
        "Instruktioner för spelet", "Starta spelet"
    };
    private List<string> PlayerOptions = new List<string>()
    {
        "En spelare mot datorn", "Två spelare mot datorn", "Två spelare utan datorn" //kalla det något annat än datorn?
    };
    private List<string> EndOptions = new List<string>()
    {
        "En omgång till!!!", "Starta ett nytt spel", "Avsluta"
    };

    public Game StartingMenu()
    {
        Console.Clear();
        int index = MenuLoop("alternativ", StartOptions);
        if (index == 0) return Instructions();
        else return ChooseLevel();
    }
    private Game Instructions()
    {
        DestructibleBlock destructible = new DestructibleBlock(0, 0);
        SolidBlock solid = new SolidBlock(0, 0);

        Console.Clear();
        Console.WriteLine("Målet är att vinna.");
        Console.WriteLine("Spräng dina motståndare, sista levande spelare vinner.\n");
        Console.WriteLine("För att komma åt fienden behöver du spränga dig fram,");
        Console.WriteLine("de sprängbara blocken ser ut såhär:\n\n"); //visa solida också
        destructible.DrawAt(11, 6);
        Console.WriteLine("\n");
        Console.WriteLine("...men de här kan du inte spränga:");
        solid.DrawAt(11, 12); Console.ResetColor();
        Console.WriteLine("\n\nDet finns också powerups som kan hjälpa dig,");
        Console.WriteLine("plocka upp dem och se vad som händer ;)\n\n");
        Console.WriteLine("Bomber sprängs efter 3 sekunder. Kom ihåg att akta dig!\n\n");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("Spelare 1"); Console.ResetColor();
        Console.Write(" styr med pilarna och placerar bomber med punkt.\n");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("Spelare 2"); Console.ResetColor();
        Console.Write(" styr med WASD och placerar bomber med mellanslag.\n\n");

        Console.WriteLine("Tryck enter för att fortsätta...");
        bool enter;
        do
        {
            List<string> keys = KeyInput.ReadAll();
            var (dx, dy, pressedEnter) = controls.GetDirection(keys);
            enter = pressedEnter;
        } while (!enter);
        return ChooseLevel();
    }
    private Game ChooseLevel()
    {
        Console.Clear();
        int index = MenuLoop("spelplan", LevelNames);
        return ChoosePlayers(LevelTypes[index]);
        
    }
    private Game ChoosePlayers(Level level)
    {
        Game game = new Game(level);
        Console.Clear();
        int index = MenuLoop("antal spelare", PlayerOptions);
        if (index == 0)
        {
            Console.Clear();
            string name = InputName();
            Player p1 = new Player(game.Level.Width - 1, game.Level.Height - 1,
                controls: new KeyboardControlScheme(ControlType.Arrows))
            {
                Name = name,
                Color = ConsoleColor.Blue
            };

            AIControlScheme aics1 = new AIControlScheme(level);
            Player p2 = new Player(0, 0, aics1) 
            {
                Name = "Bot",
                Color = ConsoleColor.Red,
            };
            aics1.SetPlayer(p2);

            AIControlScheme aics2 = new AIControlScheme(level);
            (int x, int y) = level.GetCornerPosition(Level.Corners.TopRight);
            Player p3 = new Player(x, y, aics2)
            {
                Name = "Bro",
                Color = ConsoleColor.Green,
            };
            aics2.SetPlayer(p3);

            AIControlScheme aics3 = new AIControlScheme(level);
            (int x1, int y1) = level.GetCornerPosition(Level.Corners.BottomLeft);
            Player p4 = new Player(x1, y1, aics3)
            {
                Name = "AI_Moa",
                Color = ConsoleColor.Yellow,
            };
            aics3.SetPlayer(p4);

            game.Level.AddPlayers(p1, p2, p3, p4);
            return game;
        }
        else if (index == 1)
        {
            Console.Clear();
            Console.WriteLine("Spelare 1");
            string name1 = InputName();
            Player p1 = new Player(game.Level.Width - 1, game.Level.Height - 1,
            controls: new KeyboardControlScheme(ControlType.Arrows))
            { Name = name1, Color = ConsoleColor.Red };

            Console.Clear();
            Console.WriteLine("Spelare 2");
            string name2 = InputName();
            Player p2 = new(0, 0,
            controls: new KeyboardControlScheme(ControlType.Wasd))
            { Name = name2, Color = ConsoleColor.Blue };
            //skapa här 2st AI players

            AIControlScheme aics1 = new AIControlScheme(level);
            (int x1, int y1) = level.GetCornerPosition(Level.Corners.BottomLeft);
            Player p3 = new Player(x1, y1, aics1) 
            {
                Name = "Bro",
                Color = ConsoleColor.Yellow,
            };
            aics1.SetPlayer(p3);

            AIControlScheme aics2 = new AIControlScheme(level);
            (int x2, int y2) = level.GetCornerPosition(Level.Corners.TopRight);
            Player p4 = new Player(x2, y2, aics2)
            {
                Name = "AI_Klara",
                Color = ConsoleColor.Green,
            };
            aics2.SetPlayer(p4);

            game.Level.AddPlayer(p1);
            game.Level.AddPlayer(p2);
            game.Level.AddPlayer(p3);
            game.Level.AddPlayer(p4);
            return game;

            
        }
        else
        {
            Console.Clear();
            Console.WriteLine("Spelare 1");
            string name1 = InputName();
            Player p1 = new Player(game.Level.Width - 1, game.Level.Height - 1,
            controls: new KeyboardControlScheme(ControlType.Arrows))
            { Name = name1, Color = ConsoleColor.Red };

            Console.Clear();
            Console.WriteLine("Spelare 2");
            string name2 = InputName();
            Player p2 = new(game.Level.Width - 1, game.Level.Height - 1,
            controls: new KeyboardControlScheme(ControlType.Arrows))
            { Name = name2, Color = ConsoleColor.Red };

            game.Level.AddPlayer(p1); game.Level.AddPlayer(p2);
            return game;

        }
    }

    public void GameOverMenu(Player winner, Game game)
    {
        Console.Clear();
        Console.ForegroundColor = winner.Color;
        ConsoleUtils.DrawMultiline(30, 0,
        " ______     __         __  __     ______  ",
        "/\\  ___\\   /\\ \\       /\\ \\/\\ \\   /\\__  _\\ ",
        "\\ \\___  \\  \\ \\ \\____  \\ \\ \\_\\ \\  \\/_/\\ \\/ ",
        " \\/\\_____\\  \\ \\_____\\  \\ \\_____\\    \\ \\_\\ ",
        "  \\/_____/   \\/_____/   \\/_____/     \\/_/ "
        );
        Console.SetCursorPosition(45, 6);
        Console.WriteLine($"{winner.Name} vinner!");
        Console.ResetColor();
        int index = MenuLoop("alternativ", EndOptions);
        if (index == 0)
        {
            foreach (Player player in game.Level.Players)
            {
                player.Reset();
            }
            game.Level.Reset();
            game.GameLoop();
        }
        if (index == 1)
        {
            StartingMenu();
        }
        else
        {
            Console.Clear();
            Console.WriteLine("ok hejdå");
            Environment.Exit(0);
        }
        
    }
    

     
    private int MenuLoop(string type, List<string> optionsList)
    {
        int selectedIndex = 0;

        bool enter = false;
        
        do
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"Välj {type}:\n");

            for (int i = 0; i < optionsList.Count; i++)
            {
                if (i == selectedIndex)
                {
                    Console.WriteLine($"> {optionsList[i]}");
                }
                else
                {
                    Console.WriteLine($"  {optionsList[i]}");
                }
            }

            List<string> keys = KeyInput.ReadAll();
            var (dx, dy, pressedEnter) = controls.GetDirection(keys);

            selectedIndex += dy;
            if (selectedIndex >= optionsList.Count) selectedIndex = 0;
            if (selectedIndex < 0) selectedIndex = optionsList.Count - 1; //wrap around

            enter = pressedEnter;
        } while (!enter);
        return selectedIndex;
    }
    private string InputName() //hjälp av copilot
    {
        string? name;
        do
        {
            Console.WriteLine("Ange ditt namn: ");
            name = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Du måste ange ett namn!");
            }
            else if (name.Length > 20)
            {
                Console.WriteLine("Namnet är för långt (max 20 tecken).");
            }
            else return name;
        } while (true);
    }   
}

  