using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using UnityEngine;

//1.初始化碰撞组件
//2.收集碰撞的UnitID

public class BulletScanSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;

    public BulletScanSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Bullet);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasView && !entity.isDestroyed;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (GameEntity bulletEntity in entities)
        {
            var view = bulletEntity.view.value as View;
            var boxCollider = view.gameObject.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;

            var attackerEntity = _contexts.game.GetEntityWithUnit(bulletEntity.bullet.SourceUnitID);

            var onTriggerCallBack = view.gameObject.AddComponent<OnTriggerCallBack>();
            onTriggerCallBack.TriggerEnter = (collider) =>
            {
                var attackedEntity = collider.gameObject.GetEntityLink().entity as GameEntity;

                List<HitTarget> targets = null;
                if (!bulletEntity.hitTarget.Targets.Exists((p) => { return p.UnitID == attackedEntity.unit.ID; }))
                {
                    Debug.Log($"HitSucceed,name: {attackedEntity.unit.ID}");
                    Debug.Log($"isAP_AttackSucceed = true! {Time.frameCount}");
                    attackerEntity.isAP_AttackSucceed = true;
                    bulletEntity.hitTarget.Targets.Add(new HitTarget(){UnitID = attackedEntity.unit.ID,HasNotifyToServer = false});
                    bulletEntity.isHitSucceed = true;

#if LOCAL_SERVER
                    var attackerEntity_Server = _contexts.game.GetEntityWithUnit(10000 + bulletEntity.bullet.SourceUnitID);
                    attackerEntity_Server.isAP_AttackSucceed = true;
#endif
                }
            };
        }
    }
}