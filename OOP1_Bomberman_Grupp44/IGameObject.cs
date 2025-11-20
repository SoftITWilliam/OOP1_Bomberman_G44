namespace Bomberman;

public interface IGameObject
{
    public int X { get; }
    public int Y { get; }
    void Draw();
}