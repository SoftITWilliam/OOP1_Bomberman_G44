namespace Bomberman;

public interface IBlock : IGameObject
{
    bool Collidible { get; }
}