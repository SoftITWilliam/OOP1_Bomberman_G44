namespace Bomberman.Block;

public interface IBlock : IDrawable
{
    public int X{ get; }
    public int Y { get; }
    bool HasCollision { get; }

    public void Destroy() { }
}