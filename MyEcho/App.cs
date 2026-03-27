using System;
using System.IO;

namespace MyEcho;

public static class App
{
    public static int Run(string[] args, TextReader inReader, TextWriter outWriter, TextWriter errorWriter)
    {
        try
        {
            if (args.Length > 0 && args[0].StartsWith("-"))
            {
                errorWriter.WriteLine($"my_echo: unknown option '{args[0]}'");
                return 2; 
            }

            if (args.Length == 0)
            {
                string input = inReader.ReadToEnd();
                outWriter.Write(input);
            }
            else
            {
                outWriter.WriteLine(string.Join(" ", args));
            }

            return 0; 
        }
        catch (Exception ex)
        {
            errorWriter.WriteLine($"my_echo: error - {ex.Message}");
            return 1; 
        }
    }
}