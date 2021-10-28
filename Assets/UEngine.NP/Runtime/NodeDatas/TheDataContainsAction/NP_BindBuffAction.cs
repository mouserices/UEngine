using System;
using Sirenix.OdinInspector;
using UEngine.NP;

public class NP_BindBuffAction : NP_BaseAction
{
    [LabelText("从下方选择你要绑定Buff的ID")]
    public VTD_Id VtdId;
    
    public override Action GetActionToBeDone()
    {
        this.Action = () =>
        {
            var behaveTreeDatas = Contexts.sharedInstance.game.behaveTreeDataEntity.behaveTreeData.NameToBehaveTreeDatas;
            if (behaveTreeDatas.TryGetValue(this.Skill.NPBehaveName,out var npDataSupportorBase))
            {
                if (npDataSupportorBase.NP_BuffDatas.TryGetValue(VtdId.Value,out var buffNodeData))
                {
                    buffNodeData.Excute();
                }
            }
        };
        return this.Action;
    }
}