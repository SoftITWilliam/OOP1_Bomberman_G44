namespace Bomberman.PlayerLogic;

class AIControlScheme : IControlScheme
{
    public (int dx, int dy, bool placedBomb) GetDirection(IEnumerable<string> keys)
    { 
        // Todo make bro smart
        return (0,0, false); 
    }

    public void HandleInput(IEnumerable<string> keys)
    {
        // Ignore input
    }
}