namespace SharpContainerProg.AAPublic;

public static class StaticOkAndError
{
    public static void Error(
        string msg,
        Exception? ex = null)
    {
        ConsoleColor bg = Console.BackgroundColor;
        ConsoleColor fg = Console.ForegroundColor;
        Console.BackgroundColor = ConsoleColor.DarkRed;
        Console.ForegroundColor = ConsoleColor.White;
        
        Console.WriteLine($"-e- {msg}");

        Console.BackgroundColor = bg;
        Console.ForegroundColor = fg;
        
        Console.WriteLine(ex?.Message);
    }
    
    public static void Ok(
        string msg,
        bool ommit = false)
    {
        if (ommit)
        {
            return;
        }
        
        Console.WriteLine($"--- {msg}");
    }
}
