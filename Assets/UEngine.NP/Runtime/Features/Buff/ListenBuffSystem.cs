using Entitas;
using UEngine.NP;

public class ListenBuffSystem : IExecuteSystem
{
    private Contexts _Contexts;
    private IGroup<BuffEntity> _Group;
    public ListenBuffSystem(Contexts contexts)
    {
        _Contexts = contexts;
        _Group = _Contexts.buff.GetGroup(BuffMatcher.ListenBuff);
    }

    public void Execute()
    {
        if (_Group.count > 0)
        {
            foreach (BuffEntity buffEntity in _Group)
            {
                var buffCondition = buffEntity.listenBuff.BuffCondition;
                if (buffCondition.Invoke())
                {
                    foreach (VTD_Id vtdId in buffEntity.listenBuff.BuffEffects)
                    {
                        var behaveTreeDatas = Contexts.sharedInstance.game.behaveTreeDataEntity.behaveTreeData.NameToBehaveTreeDatas;
                        if (behaveTreeDatas.TryGetValue(buffEntity.buff.BehavetreeName,out var npDataSupportorBase))
                        {
                            if (npDataSupportorBase.NP_BuffDatas.TryGetValue(vtdId.Value,out var buffNodeData))
                            {
                                buffNodeData.Excute();
                            }
                        }
                    }
                }
            }
        }
    }
}