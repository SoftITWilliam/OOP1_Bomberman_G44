using Bomberman.Block;
using Bomberman.Powerups;

namespace Bomberman.PlayerLogic;

class AIControlScheme : IControlScheme
{
    private Level level;
    private Player player;

    private Random random = new Random();
    private int frameTick = 0;

    private Queue<Move> recentMoves = new Queue<Move>();
    private const int queueLength = 3;

// Detta är lungt - vi kör alltid SetPlayer efter vi har initialiserat denna. 
// Vi kan inte skicka in player i konstruktorn, eftersom den här klassen skickas in i players konstruktor!

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public AIControlScheme(Level level)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
        this.level = level;
    }

    public void SetPlayer(Player player) =>
        this.player = player;

    // Hämta bottens senaste position
    private (int x, int y) GetPreviousPosition()
    {
        var last = recentMoves.LastOrDefault(new Move(player, 0, 0));
        return (player.X - last.Dx, player.Y - last.Dy);
    }

    // Hämta alla positioner bredvid den givna positionen, 
    // som inte ligger utanför banan.
    private List<(int x, int y)> GetAdjacentPositions(int x, int y)
    {
        List<(int x, int y)> positions =
        [
            (x + 1, y),
            (x - 1, y),
            (x, y + 1),
            (x, y - 1),
        ];

        return positions
            .Where((pos) => !level.IsOutOfBounds(pos.x, pos.y))
            .ToList();
    }

    // Hämta alla block som ligger bredvid botten 
    private List<IBlock> GetAdjacentBlocks()
    {
        List<IBlock> blocks = [];
        foreach ((int px, int py) in GetAdjacentPositions(player.X, player.Y))
        {
            if (level.TryGetBlockAt(px, py, out IBlock? b)) 
                blocks.Add(b);  
        }
        return blocks;
    }

    // Hämta alla andra spelare som ligger bredvid botten
    private List<Player> GetAdjacentPlayers()
    {
        List<Player> players = [];
        foreach ((int px, int py) in GetAdjacentPositions(player.X, player.Y))
        {
            if (level.TryGetPlayerAt(px, py, out Player? p)) 
                players.Add(p);  
        }
        return players;
    }

    // Hämta alla powerups som ligger bredvid botten
    private List<Powerup> GetAdjacentPowerups()
    {
        List<Powerup> powerups = [];
        foreach ((int px, int py) in GetAdjacentPositions(player.X, player.Y))
        {
            if (level.TryGetPowerupAt(px, py, out Powerup? p)) 
                powerups.Add(p);  
        }
        return powerups;
    }
    
    // Hämta alla giltiga förflyttningar från spelarens nuvarande position
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

    // Returnerar en slumpmässig move från listan
    private Move GetRandomMove(List<Move> moves)
    {
        if (moves.Count == 0) 
            return new Move(player, 0, 0);
            
        int i = random.Next(0, moves.Count);
        return moves[i];
    }

    // Tar emot alla moves som är möjliga, och returnerar ett "smart" move.
    // Denna metod sköter också bombplaceringen.
    private Move GetWeightedMove(List<Move> moves)
    {
        if (moves.Count == 1)
        {
            // Återvändsgränd - placera ut en bomb
            moves[0].bomb = true;
            return moves[0];
        }

        // Under normala omständigheter kan botten inte gå tillbaks 
        // till positionen den precis varit på
        var lastPos = GetPreviousPosition();
        moves.RemoveAll(move => 
            move.NewX == lastPos.x && move.NewY == lastPos.y);

        // Om botten står bredvid en powerup så *måste* botten gå till den
        // (jag orkar inte göra riktig pathfinding)
        var adjacentPowerups = GetAdjacentPowerups();
        if (adjacentPowerups.Count > 0)
        {
            moves.RemoveAll(move => 
                level.TryGetPowerupAt(move.NewX, move.NewY, out _) == false);
        }

        // Efter vi filtrerat bort "ej optimala" moves, så väljer vi en slumpmässig.
        var move = GetRandomMove(moves);

        // Antal sprängbara block bredvid botten
        var adjacentBlowableBlocks = GetAdjacentBlocks()
            .Count(b => b is DestructibleBlock && b.HasCollision);

        // Antal andra spelare bredvid botten
        var adjacentPlayers = GetAdjacentPlayers().Count;

        // Chans att sätta ut en bomb baserat på hur många 
        // sprängbara block och spelare som finns bredvid
        var bombChance = 
            (adjacentBlowableBlocks * 0.2) +
            (adjacentPlayers * 0.5); 

        if (random.NextDouble() < bombChance)
            move.bomb = true;

        return move;
    }

    public (int dx, int dy, bool placedBomb) GetDirection(
        IEnumerable<string> keys)
    { 
        frameTick = (frameTick + 1) % 5;
        if (frameTick != 0)
            return (0, 0, false);
        
        List<Move> moves = GetLegalMoves();

        // brorsan sitter fast
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