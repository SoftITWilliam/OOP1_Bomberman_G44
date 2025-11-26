namespace Bomberman.PlayerLogic;

interface IControlScheme
{
    public (int dx, int dy, bool placedBomb) GetDirection(IEnumerable<string> keys);
}