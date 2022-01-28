using System.Collections.Generic;
using System.ComponentModel.Design;
// using Cinemachine;
using Entitas;
using UnityEngine;

namespace UEngine.HotFix
{
    public class CameraFollowSystem : ReactiveSystem<UnitEntity>
    {
        public CameraFollowSystem() : base(Contexts.sharedInstance.unit)
        {
        }

        protected override ICollector<UnitEntity> GetTrigger(IContext<UnitEntity> context)
        {
            return context.CreateCollector(UnitMatcher.AllOf(UnitMatcher.MainPlayer,UnitMatcher.View));
        }

        protected override bool Filter(UnitEntity entity)
        {
            return entity.isMainPlayer && entity.hasView;
        }

        protected override void Execute(List<UnitEntity> entities)
        {
            var mainPlayerEntity = entities.SingleEntity();
            var gameObject = mainPlayerEntity.view.value.GetObj();

            var followCamera = GameObject.Instantiate(Resources.Load<GameObject>("FollowCamera"));
            // followCamera.GetComponent<CinemachineVirtualCamera>().Follow = gameObject.transform;
            // followCamera.GetComponent<CinemachineVirtualCamera>().LookAt = gameObject.transform;
        }
    }
}