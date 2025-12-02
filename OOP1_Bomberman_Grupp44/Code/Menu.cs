using System.Security.Cryptography;
using System.Transactions;
using System.Xml;
using Bomberman.Block;
using Bomberman.PlayerLogic;

namespace Bomberman;

class Menu
{
    private KeyboardControlScheme controls
    = new KeyboardControlScheme(ControlType.MenuControls);
    
    private readonly ConsoleColor[] PlayerColors = [
        ConsoleColor.Blue,
        ConsoleColor.Red,
        ConsoleColor.Green,
        ConsoleColor.Yellow,
    ];
    private List<string> LevelNames = new List<string>()
    {
        "Classic", "Star", "Test level(obs ta bort)", "Crosshair", "King of the hill"
    };
    private List<string> StartOptions = new List<string>()
    {
        "Instruktioner för spelet", "Starta spelet"
    };
    private List<string> PlayerOptions = new List<string>()
    {
        "En spelare mot en datorspelare", "En spelare mot tre datorspelare", 
        "Två spelare mot två datorspelare", "Två spelare utan datorspelare"
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
        Console.WriteLine("de sprängbara blocken ser ut såhär:\n\n");
        destructible.DrawAt(11, 6);
        Console.WriteLine("\n");
        Console.WriteLine("...men de här kan du inte spränga:");
        solid.DrawAt(11, 12); Console.ResetColor();
        Console.WriteLine("\n\nDet finns också powerups som kan hjälpa dig,");
        Console.WriteLine("plocka upp dem och se vad som händer ;)\n\n");
        Console.WriteLine("Bomber sprängs efter 3 sekunder. Kom ihåg att akta dig!\n\n");
        Console.ForegroundColor = PlayerColors[0];
        Console.Write("Spelare 1"); Console.ResetColor();
        Console.Write(" styr med pilarna och placerar bomber med punkt.\n");
        Console.ForegroundColor = PlayerColors[1];
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
        List<Level> LevelTypes = new List<Level>()
        {
            Level.ClassicLevel(), Level.StarLevel(), Level.TestLevel(),
            Level.CrossHairLevel(), Level.KingOfTheHill()
        };
        Console.Clear();
        int index = MenuLoop("spelplan", LevelNames);


        return ChoosePlayers(LevelTypes[index]);
        
    }
    private Game ChoosePlayers(Level level)
    {
        Game game = new Game(level);

        BotNameGenerator botNames = new BotNameGenerator();

        Console.Clear();
        int index = MenuLoop("antal spelare", PlayerOptions);

        Player CreateHumanPlayer(Level.Corners corner, ConsoleColor color,
        string name, ControlType controlType)
        {
            (int x, int y) = level.GetCornerPosition(corner);

            return new Player(x, y,
                controls: new KeyboardControlScheme(ControlType.Arrows))
            {
                Name = name,
                Color = color
            };
        }

        Player CreateBotPlayer(Level.Corners corner, ConsoleColor color)
        {
            (int x, int y) = level.GetCornerPosition(corner);
            AIControlScheme controlScheme = new AIControlScheme(level);
        
            Player bot = new Player(x, y, controlScheme)
            {
                Name = botNames.GetRandomName(),
                Color = color
            };
            controlScheme.SetPlayer(bot);
            return bot;
        }

        Console.Clear();

        if (index == 0) //"En spelare mot en datorspelare"
        {
            Console.WriteLine("Spelare 1");
            string name1 = InputName();

            Player player1 = CreateHumanPlayer(
                corner: Level.Corners.BottomRight, 
                color: PlayerColors[0], 
                name: name1,
                controlType: ControlType.Arrows);
                
            Player bot1 = CreateBotPlayer(
                Level.Corners.TopLeft, PlayerColors[1]);

            game.Level.AddPlayers(player1, bot1);
            return game;
        }
        else if (index == 1) //"En spelare mot tre datorspelare"
        {
            Console.WriteLine("Spelare 1");
            string name1 = InputName();

            Player player1 = CreateHumanPlayer(
                corner: Level.Corners.BottomRight, 
                color: PlayerColors[0], 
                name: name1,
                controlType: ControlType.Arrows);

            Player bot1 = CreateBotPlayer(
                Level.Corners.TopLeft, PlayerColors[1]);
                
            Player bot2 = CreateBotPlayer(
                Level.Corners.TopRight, PlayerColors[2]);

            Player bot3 = CreateBotPlayer(
                Level.Corners.BottomLeft, PlayerColors[3]);

            game.Level.AddPlayers(player1, bot1, bot2, bot3);
        }
        else if(index == 2) // "Två spelare mot två datorspelare"
        {
            Console.WriteLine("Spelare 1");
            string name1 = InputName();

            Player player1 = CreateHumanPlayer(
                corner: Level.Corners.BottomRight, 
                color: PlayerColors[0], 
                name: name1,
                controlType: ControlType.Arrows);

            Console.Clear();
            Console.WriteLine("Spelare 2");
            string name2 = InputName();
            
            Player player2 = CreateHumanPlayer(
                corner: Level.Corners.BottomRight, 
                color: PlayerColors[1], 
                name: name2,
                controlType: ControlType.Arrows);

            Player bot1 = CreateBotPlayer(
                Level.Corners.TopRight, PlayerColors[2]);
                
            Player bot2 = CreateBotPlayer(
                Level.Corners.BottomLeft, PlayerColors[3]);
                
            game.Level.AddPlayers(player1, player2, bot1, bot2);
        }
        else //"Två spelare utan datorn"
        {
            Console.WriteLine("Spelare 1");
            string name1 = InputName();

            Player player1 = CreateHumanPlayer(
                corner: Level.Corners.BottomRight, 
                color: PlayerColors[0], 
                name: name1,
                controlType: ControlType.Arrows);

            Console.Clear();
            Console.WriteLine("Spelare 2");
            string name2 = InputName();
            
            Player player2 = CreateHumanPlayer(
                corner: Level.Corners.BottomRight, 
                color: PlayerColors[1], 
                name: name2,
                controlType: ControlType.Arrows);

            game.Level.AddPlayers(player1, player2);
        }
        return game;
    }

    public (Game? game, bool continuePlaying) GameOverMenu(Player winner, Game game)
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
            return (game, continuePlaying: true);
        }
        if (index == 1)
        {
            return (null, continuePlaying: true);
        }
        else
        {
            return (null, continuePlaying: false);
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
            //wrap around:
            if (selectedIndex < 0) selectedIndex = optionsList.Count - 1;

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

  