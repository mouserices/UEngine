using System.Collections.Generic;
// using Cinemachine;
using Entitas;
using UnityEngine;

public class PlayerFollowSystem : ReactiveSystem<UnitEntity>,IInitializeSystem
{
    private Contexts m_Contexts;
    private GameObject m_FollowCamera;
    public PlayerFollowSystem(Contexts contexts) : base(contexts.unit)
    {
        m_Contexts = contexts;
    }

    protected override ICollector<UnitEntity> GetTrigger(IContext<UnitEntity> context)
    {
       return context.CreateCollector(UnitMatcher.MainPlayer);
    }

    protected override bool Filter(UnitEntity entity) => entity.isMainPlayer && entity.hasView;
    

    protected override void Execute(List<UnitEntity> entities)
    {
        foreach (UnitEntity gameEntity in entities)
        {
            BindFollowCamera(gameEntity);
        }
    }

    private void BindFollowCamera(UnitEntity gameEntity)
    {
        var view = gameEntity.view.value as View;
        
        // var cinemachineVirtualCamera = m_FollowCamera.GetComponent<CinemachineVirtualCamera>();
        // cinemachineVirtualCamera.Follow = view.transform.Find("PlayerCameraRoot");
        // cinemachineVirtualCamera.LookAt = view.transform.Find("PlayerCameraRoot");
    }

    public void Initialize()
    {
        m_FollowCamera = GameObject.Instantiate(Resources.Load<GameObject>("PlayerFollowCamera"));
    }
}