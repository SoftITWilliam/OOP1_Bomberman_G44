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

    public void StartingMenu()
    {
        int index = MenuLoop("alternativ", StartOptions);
        if (index == 0) Instructions();
        else ChooseLevel();
    }
    private void Instructions()
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
        ChooseLevel();
    }
    public void ChooseLevel()
    {
        int index = MenuLoop("spelplan", LevelNames);
        ChoosePlayers(LevelTypes[index]);
    }
    public void ChoosePlayers(Level level)
    {
        Game game = new Game(level);

        int index = MenuLoop("antal spelare", PlayerOptions);
        if (index == 0)
        {
            Console.Clear();
            string name = InputName();
            Player p1 = new Player(game.Level.Width - 1, game.Level.Height - 1,
                controls: new KeyboardControlScheme(ControlType.Arrows))
            {
                Name = name,
                Color = ConsoleColor.Red
            };
            //skapa här 3st AI players
            game.Level.AddPlayer(p1);
            Console.Clear();
            game.GameLoop();
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
            Player p2 = new(game.Level.Width - 1, game.Level.Height - 1,
            controls: new KeyboardControlScheme(ControlType.Arrows))
            { Name = name2, Color = ConsoleColor.Red };
            //skapa här 2st AI players
            game.Level.AddPlayer(p1); game.Level.AddPlayer(p2);
            Console.Clear();
            game.GameLoop();
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
            Console.Clear();
            game.GameLoop();
        }
    }
    private int MenuLoop(string type, List<string> optionsList)
    {
        int selectedIndex = 0;

        bool enter = false;
        Console.Clear();
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

  