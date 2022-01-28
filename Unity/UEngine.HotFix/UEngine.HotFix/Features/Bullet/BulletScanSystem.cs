using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using UnityEngine;

//1.初始化碰撞组件
//2.收集碰撞的UnitID

public class BulletScanSystem : ReactiveSystem<UnitEntity>
{
    private readonly Contexts _contexts;

    public BulletScanSystem(Contexts contexts) : base(contexts.unit)
    {
        _contexts = contexts;
    }

    protected override ICollector<UnitEntity> GetTrigger(IContext<UnitEntity> context)
    {
        return context.CreateCollector<UnitEntity>(UnitMatcher.Bullet);
    }

    protected override bool Filter(UnitEntity entity)
    {
        return entity.hasView && !entity.isDestroyed;
    }

    protected override void Execute(List<UnitEntity> entities)
    {
        foreach (UnitEntity bulletEntity in entities)
        {
             var boxCollider = bulletEntity.view.value.GetObj().AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;

            var attackerEntity = _contexts.unit.GetEntityWithUnit(bulletEntity.bullet.SourceUnitID);

            var onTriggerCallBack = bulletEntity.view.value.GetObj().AddComponent<OnTriggerCallBack>();
            onTriggerCallBack.TriggerEnter = (collider) =>
            {
                var attackedEntity = collider.gameObject.GetEntityLink().entity as UnitEntity;

                List<HitTarget> targets = null;
                if (!bulletEntity.hitTarget.Targets.Exists((p) => { return p.UnitID == attackedEntity.unit.ID; }))
                {
                    Debug.Log($"HitSucceed,name: {attackedEntity.unit.ID}");
                    Debug.Log($"isAP_AttackSucceed = true! {Time.frameCount}");
                    attackerEntity.isAP_AttackSucceed = true;
                    bulletEntity.hitTarget.Targets.Add(new HitTarget(){UnitID = attackedEntity.unit.ID,HasNotifyToServer = false});
                    bulletEntity.isHitSucceed = true;

/*#if LOCAL_SERVER
                    var attackerEntity_Server = _contexts.game.GetEntityWithUnit(10000 + bulletEntity.bullet.SourceUnitID);
                    attackerEntity_Server.isAP_AttackSucceed = true;
#endif*/
                }
            };
        }
    }
}