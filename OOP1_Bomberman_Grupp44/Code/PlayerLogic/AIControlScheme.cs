namespace Bomberman.PlayerLogic;

class AIControlScheme : IControlScheme
{
    private Level level;
    private Player player;

    private Random random = new Random();
    private int frameTick = 0;

    private Queue<Move> recentMoves = new Queue<Move>();
    private const int queueLength = 3;

    public AIControlScheme(Level level)
    {
        // Todo make bro smart

        // - Weighted förflyttning: prioritera senaste riktningen.
        // - I vanliga fall, kan den inte gå riktningen den kom från. Tänk snake. (Undantag dead-ends och bomber)
        // - Vid egen bomb placering: backtracka senaste förflyttningarna
        // - Undvik rutor som är på väg att sprängas

        this.level = level;
    }

    public void SetPlayer(Player player) =>
        this.player = player;

    private (int x, int y) GetPreviousPosition()
    {
        var last = recentMoves.LastOrDefault(new Move(player, 0, 0));
        return (player.X - last.Dx, player.Y - last.Dy);
    }
    
    private List<Move> GetLegalMoves()
    {
        return new List<Move>()
        {
            new Move(player, 1, 0),
            new Move(player, 0, 1),
            new Move(player, -1, 0),
            new Move(player, 0, -1),
        }
        .Where(move => level.CheckValidMove(move.NewX, move.NewY))
        .ToList();
    }

    private Move GetRandomMove(List<Move> moves)
    {
        int i = random.Next(0, moves.Count);
        return moves[i];
    }

    private void WriteMoves(IEnumerable<Move> moves)  
    {
        Console.Write($"Count: {moves.Count()} | ");
        var strings = moves.Select(move => $"{move.Dx}, {move.Dy}");
        Console.Write(string.Join(" ; ", strings) + "                              ");
    }

    private Move GetWeightedMove(List<Move> moves)
    {
        if (moves.Count == 1)
        {
            moves[0].bomb = true;
            return moves[0];
        }

        var lastPos = GetPreviousPosition();

        moves.RemoveAll(move => 
            move.NewX == lastPos.x && move.NewY == lastPos.y);

        return GetRandomMove(moves);
    }

    public (int dx, int dy, bool placedBomb) GetDirection(
        IEnumerable<string> keys)
    { 
        frameTick = (frameTick + 1) % 5;
        if (frameTick != 0)
            return (0, 0, false);
        
        List<Move> moves = GetLegalMoves();

        // bro sitter fast
        if (moves.Count == 0)
            return (0, 0, false);

        var move = GetWeightedMove(moves);
        recentMoves.Enqueue(move);

        // håll koll på de senaste förflyttningarna i en queue
        if (recentMoves.Count > queueLength)
            recentMoves.Dequeue();

        return (move.Dx, move.Dy, move.bomb);
    }
}

class Move
{
    public int Dx { get; }
    public int Dy { get; }
    public int NewX { get; }
    public int NewY { get; }
    public bool bomb { get; set; }

    public Move(Player player, int dx, int dy)
    {
        (this.Dx, this.Dy) = (dx, dy);
        this.NewX = player.X + dx;
        this.NewY = player.Y + dy;
    }
}