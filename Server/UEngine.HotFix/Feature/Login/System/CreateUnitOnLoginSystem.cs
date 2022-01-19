using System.Collections.Generic;
using Entitas;
using UEngine.Net;
using UEngine.NP.Features.FsmState;
using UEngine.NP.FsmState;
using UnityEngine;

public class CreateUnitOnLoginSystem : ReactiveSystem<GameEntity>
{
    public CreateUnitOnLoginSystem() : base(Contexts.sharedInstance.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.LoginMessage);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasLoginMessage;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            // var unitId = UnitIdGenerater.GetID();
            // var assets = "eliteKnight";
            // Vector3 pos = new Vector3(0, 0, -15f);
            // Vector3 rot = new Vector3(0, 0, 0);
            // var skills = new List<int>() { 1001 };
            // var campType = unitId % 2 == 0 ? CampType.Own : CampType.Enemy;
            //
            // MetaContext.Get<IUnitService>()
            //     .CreateUnit(unitId, 1, assets, pos,
            //         rot, new IdleStateParam() { AnimClipName = "Idle", StateType = StateType.IDLE },
            //         skills, campType);
            //
            // MetaContext.Get<INetworkService>().SendMessage(entity.loginMessage.ConnectedID, new S2C_CreatePlayer_1003()
            // {
            //     Unit = new UnitData()
            //     {
            //         ID = unitId,
            //         Asset = assets,
            //         PosX = pos.x,
            //         PosY = pos.y,
            //         PosZ = pos.z,
            //         RotX = rot.x,
            //         RotY = rot.y,
            //         RotZ = rot.z,
            //         Skills = skills,
            //         CampType = (int)campType
            //     }
            // });
        }
    }
}