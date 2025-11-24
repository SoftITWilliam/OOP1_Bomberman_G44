// ...existing code...
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Bomberman;

// Hela denna klass är från Copilot
class KeyInput
{
    private static readonly ConcurrentQueue<string> queue = new();
    private static CancellationTokenSource? cts;
    private static Task? inputTask;

    // Start the background reader (call once at app start)
    public static void Start()
    {
        if (inputTask != null && !inputTask.IsCompleted) return;
        cts = new CancellationTokenSource();
        var token = cts.Token;
        inputTask = Task.Run(() =>
        {
            while (!token.IsCancellationRequested)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    queue.Enqueue(key.Key.ToString());
                }
                else
                {
                    Thread.Sleep(5); // throttle to avoid busy loop
                }
            }
        }, token);
    }

    // Stop the background reader (call on shutdown)
    public static void Stop()
    {
        if (cts == null) return;
        cts.Cancel();
        try { inputTask?.Wait(100); } catch { }
        cts = null;
        inputTask = null;
    }

    // Drain and return all keys collected since last call
    public static List<string> ReadAll()
    {
        var list = new List<string>();
        while (queue.TryDequeue(out var k)) list.Add(k);
        return list;
    }

    // Convenience: return a single string (or null) representing inputs
    public static string? ReadAsString()
    {
        var all = ReadAll();
        if (all.Count == 0) return null;
        return string.Join(",", all);
    }
}