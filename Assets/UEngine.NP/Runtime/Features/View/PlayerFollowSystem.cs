using System.Collections.Generic;
using Cinemachine;
using Entitas;
using UnityEngine;

public class PlayerFollowSystem : ReactiveSystem<GameEntity>,IInitializeSystem
{
    private Contexts m_Contexts;
    private GameObject m_FollowCamera;
    public PlayerFollowSystem(Contexts contexts) : base(contexts.game)
    {
        m_Contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
       return context.CreateCollector(GameMatcher.MainPlayer);
    }

    protected override bool Filter(GameEntity entity) => entity.hasMainPlayer && entity.hasView;
    

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (GameEntity gameEntity in entities)
        {
            BindFollowCamera(gameEntity);
        }
    }

    private void BindFollowCamera(GameEntity gameEntity)
    {
        var view = gameEntity.view.value as View;
        
        var cinemachineVirtualCamera = m_FollowCamera.GetComponent<CinemachineVirtualCamera>();
        cinemachineVirtualCamera.Follow = view.transform.Find("PlayerCameraRoot");
        cinemachineVirtualCamera.LookAt = view.transform.Find("PlayerCameraRoot");
    }

    public void Initialize()
    {
        m_FollowCamera = GameObject.Instantiate(Resources.Load<GameObject>("PlayerFollowCamera"));
    }
}