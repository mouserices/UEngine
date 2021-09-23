using System;
using UnityEngine;


public class GameStart : MonoBehaviour
{
    private GameSystems _GameSystems;
    private GameLateUpdateSystems _GameLateUpdateSystems;
    public void Start()
    {
        _GameSystems = new GameSystems(Contexts.sharedInstance);
        _GameSystems.Initialize();

        _GameLateUpdateSystems = new GameLateUpdateSystems(Contexts.sharedInstance);
        _GameLateUpdateSystems.Initialize();
    }

    public void Update()
    {
        _GameSystems.Execute();
        _GameSystems.Cleanup();
    }

    public void LateUpdate()
    {
        _GameLateUpdateSystems.Execute();
        _GameLateUpdateSystems.Cleanup();
    }
}