using Cinemachine;
using UnityEngine;

public class CameraService : Service
{
    private GameObject _CameraGO;
    private CinemachineVirtualCamera _CinemachineVirtual;
    private Camera _Camera;

    public GameObject CameraGO
    {
        get => _CameraGO;
        set => _CameraGO = value;
    }

    public CinemachineVirtualCamera CinemachineVirtual
    {
        get => _CinemachineVirtual;
        set => _CinemachineVirtual = value;
    }

    public Camera Camera
    {
        get => _Camera;
        set => _Camera = value;
    }

    public CameraService(Contexts contexts) : base(contexts)
    {
    }

    public void FindMainCamera()
    {
        _Camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    public void CreateVirtualCamera(Vector3 pos,Vector3 rot)
    {
        _CameraGO = GameObject.Instantiate(Resources.Load<GameObject>("PlayerFollowCamera"));
        _CinemachineVirtual = _CameraGO.GetComponent<CinemachineVirtualCamera>();

        _CameraGO.transform.position = pos;
        _CameraGO.transform.rotation = Quaternion.Euler(rot);
    }
}