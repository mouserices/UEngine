using System;
using UnityEngine;


public class GameStart : MonoBehaviour
{
    private GameSystems _GameSystems;
    public void Start()
    {
        _GameSystems = new GameSystems(Contexts.sharedInstance);
        _GameSystems.Initialize();
    }

    public void Update()
    {
        _GameSystems.Execute();
        _GameSystems.Cleanup();
    }
}