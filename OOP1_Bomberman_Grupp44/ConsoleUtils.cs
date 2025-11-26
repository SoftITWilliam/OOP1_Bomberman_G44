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

    public static void DrawFullBlock(int cx, int cy, char blockChar, ConsoleColor? color = null)
    {
        for (int i = 0; i < Game.BlockCharHeight; i++)
        {
            Console.SetCursorPosition(cx, cy + i);
            string line = string.Empty.PadLeft(Game.BlockCharWidth, blockChar);
            WriteWithColor(
                value: line, 
                color: color ?? Console.ForegroundColor);
        }
    }

    public static void DrawMultiline(int cx, int cy, params string[] lines)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            Console.SetCursorPosition(cx, cy + i);
            Console.Write(lines[i]);
        }
    }
} 