namespace Bomberman;

public static class ConsoleUtils
{
    public static void WriteWithColor(object? value, ConsoleColor color)
    {
        ConsoleColor current = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(value);
        Console.ForegroundColor = current;
    }
} 