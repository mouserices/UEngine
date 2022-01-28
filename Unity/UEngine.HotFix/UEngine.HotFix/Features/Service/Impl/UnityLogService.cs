using UnityEngine;

[Service]
public class UnityLogService : ILogService
{
    public void Log(string log)
    {
        Debug.Log(log);
    }

    public void LogError(string error)
    {
        Debug.LogError(error);
    }
}