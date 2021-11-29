using System;
using UnityEngine;


public class GameStart : MonoBehaviour
{
    private UpdateSystems _GameSystems;
    private LateUpdateSystems _GameLateUpdateSystems;
    private Services _services;
    private Contexts _contexts;
    public void Start()
    {
        _contexts = Contexts.sharedInstance;
        _services = new Services();
        
        CreateServices(_contexts,_services);
        
        _GameSystems = new UpdateSystems(_contexts,_services);
        _GameSystems.Initialize();

        _GameLateUpdateSystems = new LateUpdateSystems(_contexts,_services);
        _GameLateUpdateSystems.Initialize();
    }

    private void CreateServices(Contexts contexts, Services services)
    {
        services.InputService = new InputService(contexts);
        services.CameraService = new CameraService(contexts);
        services.UIService = new UIService(contexts);
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