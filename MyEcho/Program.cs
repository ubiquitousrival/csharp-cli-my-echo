using System;

namespace MyEcho;

public class Program
{
    public static int Main(string[] args)
    {
        return App.Run(args, Console.In, Console.Out, Console.Error);
    }
}
