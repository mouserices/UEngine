namespace UEngine.NP.FsmState
{
    public class WalkState : FsmStateBase
    {
        public override StateType GetConflictStates()
        {
            return StateType.Attack| StateType.Combo;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            var walkStateParam = StateParam as WalkStateParam;

#if CLIENT
            //play anim
            var gameEntity = entity as UnitEntity;
            gameEntity.ReplaceAnimation(walkStateParam.AnimClipName,1f,null);
#endif
        }
    }
}