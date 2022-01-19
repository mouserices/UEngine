using System;

[Service]
public class ConsoleLogService : ILogService
{
    public void Log(string log)
    {
        Console.WriteLine($"info: {log}");
    }

    public void LogError(string error)
    {
        Console.WriteLine($"Error: {error}");
    }
}