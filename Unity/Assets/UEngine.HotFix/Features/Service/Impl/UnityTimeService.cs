using UnityEngine;

[Service]
public class UnityTimeService : ITimeService
{
    public float deltaTime()
    {
        return Time.deltaTime;
    }
}