namespace UEngine.NP.FsmState
{
    public class WalkState : FsmStateBase
    {
        public override StateType GetConflictStates()
        {
            return StateType.Attack;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            var walkStateParam = StateParam as WalkStateParam;
            
            //play anim
            var gameEntity = entity as GameEntity;
            gameEntity.ReplaceAnimation(walkStateParam.AnimClipName);
        }
    }
}