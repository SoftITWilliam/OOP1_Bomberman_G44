namespace Bomberman;

public interface IBlock
{
    public int X{ get; }
    public int Y { get; }
    bool Collidible { get; }

    public void Draw() { }
    public void Destroy() { }
}