namespace Bomberman;

public interface IBlock : IDrawable
{
    public int X{ get; }
    public int Y { get; }
    bool Collidible { get; }

    public void Destroy() { }
}