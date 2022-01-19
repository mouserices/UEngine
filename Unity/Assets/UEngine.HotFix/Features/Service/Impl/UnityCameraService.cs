using UnityEngine;

[Service]
public class UnityCameraService : ICameraService
{
    private Camera _camera;
    public Camera GetCamera()
    {
        if (_camera == null)
        {
            _camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        }
        return _camera;
    }
}