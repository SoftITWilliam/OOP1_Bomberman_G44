using System.Transactions;
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

    public void StartingMenu()
    {
        int index = MenuLoop("alternativ", StartOptions);
        if (index == 0) Instructions();
        else ChooseLevel(); //senare - chooseplayer
    }
    public void Instructions()
    {
        Console.Clear();
        Console.WriteLine("Såhär spelar man. blabla. instruktion");
        Console.WriteLine("Tryck enter för att fortsätta");
        bool enter;
        do
        {
            List<string> keys = KeyInput.ReadAll();
            var (dx, dy, pressedEnter) = controls.GetDirection(keys);
            enter = pressedEnter;
        } while (!enter);
        ChooseLevel(); //senare - chooseplayer;
    }
    public Level ChooseLevel()
    {
        int index = MenuLoop("spelplan", LevelNames);
        return LevelTypes[index];
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


    public void ChoosePlayers()
    {

    }
}

  