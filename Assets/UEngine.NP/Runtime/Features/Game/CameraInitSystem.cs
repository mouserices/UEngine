using Entitas;
using UnityEngine;

public class CameraInitSystem : IInitializeSystem
{
    private CameraService _CameraService;
    public CameraInitSystem(Contexts contexts, Services services)
    {
        _CameraService = services.CameraService;
    }

    public void Initialize()
    {
        _CameraService.FindMainCamera();
        _CameraService.CreateVirtualCamera(new Vector3(0,10f,-20f),new Vector3(40f,0,0));
    }
}