namespace MediatorPattern;

public static class ColorConsole
{
    public static void WriteLineCommandHandler(string print) => WriteLineColored(print, ConsoleColor.Blue);
    public static void WriteLineQueryHandler(string print) => WriteLineColored(print, ConsoleColor.Yellow);
    public static void WriteLineProgram(string print) => WriteLineColored(print, ConsoleColor.Green);
    public static void WriteLineRepository(string print) => WriteLineColored(print, ConsoleColor.DarkMagenta);
    
    private  static void WriteLineColored(string toPrint, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(toPrint);
        Console.ResetColor();
    }
}