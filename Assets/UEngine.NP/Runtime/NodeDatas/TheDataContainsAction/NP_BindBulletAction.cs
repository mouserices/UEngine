using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UEngine.NP;
using UnityEngine;

[Title("子弹配置", TitleAlignment = TitleAlignments.Centered)]
public class NP_BindBulletAction : NP_ClassForStoreAction
{
    
    [BoxGroup("基本配置")][LabelText("绑定的骨骼名称")] public string BindBoneName;
    [BoxGroup("基本配置")][LabelText("盒子位置")] public NP_BBValue_Vector3 BoxPos = new NP_BBValue_Vector3();
    [BoxGroup("基本配置")][LabelText("盒子旋转")] public NP_BBValue_Vector3 BoxRot = new NP_BBValue_Vector3();
    [BoxGroup("基本配置")][LabelText("盒子缩放")] public NP_BBValue_Vector3 BoxScale = new NP_BBValue_Vector3();
    [BoxGroup("基本配置")][LabelText("子弹延迟多少时间后出生")] public float DelayBornTime = 0;
    [BoxGroup("基本配置")][LabelText("子弹多少时间后自动销毁")] public float AutoDelayDestroyTime = 0;
    [BoxGroup("基本配置")][LabelText("子弹销毁时间倍速")]public float multiple = 1;

    [BoxGroup("碰撞配置")][LabelText("指定哪个子弹打到了人")]public NP_BlackBoardKeySelecter<NP_BBValue_Bool> BlackBoardKey_IsAttacked = new NP_BlackBoardKeySelecter<NP_BBValue_Bool>();
    [BoxGroup("碰撞配置")][LabelText("将目标存放在指定的黑板值中")]public NP_BlackBoardKeySelecter<NP_BBValue_List_Long> BlackBoardKey_Targets = new NP_BlackBoardKeySelecter<NP_BBValue_List_Long>();
    public override Action GetActionToBeDone()
    {
        this.Action = () =>
        {
            var entityWithUnit = Contexts.sharedInstance.game.GetEntityWithUnit(this.UnitID);
            var view = entityWithUnit.view.value as View;

            var bone = view.GetBone(BindBoneName);
            if (bone != null)
            {
                GameEntity combatEntity = Contexts.sharedInstance.game.CreateEntity();
                combatEntity.AddName(BlackBoardKey_IsAttacked.BBKey);
                combatEntity.AddBindBlackBoardKeyToHitTargets(BlackBoardKey_Targets.BBKey);
                combatEntity.AddHitTarget(new List<HitTarget>());
                combatEntity.AddParent(bone);
                combatEntity.AddPosition(new Vector3(BoxPos.Value.X, BoxPos.Value.Y, BoxPos.Value.Z));
                combatEntity.AddRotation(new Vector3(BoxRot.Value.X, BoxRot.Value.Y, BoxRot.Value.Z));
                combatEntity.AddScale(new Vector3(BoxScale.Value.X, BoxScale.Value.Y, BoxScale.Value.Z));
                combatEntity.AddAsset(string.Empty);
                combatEntity.AddLayer(LayerMask.NameToLayer("Player"));
                combatEntity.AddBullet(this.UnitID,Skill.ID);
                combatEntity.AddLife(AutoDelayDestroyTime / multiple,0);
            }
            else
            {
                Debug.LogError($"can not find bone, name: {BindBoneName}");
            }
        };
        return this.Action;
    }
}