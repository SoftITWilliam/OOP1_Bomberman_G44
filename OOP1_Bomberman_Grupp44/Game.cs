namespace Bomberman;

class Game 
{
    private List<IPlayer> Players = new List<IPlayer>();
    private List<IBlock> Blocks = new List<IBlock>();

    public void AddPlayer(IPlayer player) 
    {
        Players.Add(player);
        Console.WriteLine($"Added player: {player.Name}");
    }

    public void CreateLevel_Placeholder()
    {
        Blocks.Add(new SolidBlock(1, 1));
        Blocks.Add(new SolidBlock(1, 3));
        Blocks.Add(new SolidBlock(3, 1));
        Blocks.Add(new SolidBlock(3, 3));
    }

    public void DrawEverything()
    {
        Console.WriteLine("öhhh");
    }
}