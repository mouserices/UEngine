using System;
using Entitas;
using UEngine.NP.Unit;

namespace UEngine.NP.FsmState
{
    public abstract class FsmStateBase
    {
        public StateType StateType;
        public int Priority;
        public IStateParam StateParam;
        
        private IEntity _entity;
        public IEntity entity => this._entity;

        public void Link(IEntity entity)
        {
            this._entity = this._entity == null ? entity : throw new Exception("EntityLink is already linked to " + (object) this._entity + "!");
            this._entity.Retain((object) this);
        }

        public void Unlink()
        {
            if (this._entity == null)
                throw new Exception("EntityLink is already unlinked!");
            this._entity.Release((object) this);
            this._entity = (IEntity) null;
        }

        public virtual void OnEnter()
        {
           
        }

        public virtual void OnRemove()
        {
            this.Unlink();
        }

        public virtual void OnExist()
        {
            this.Unlink();
        }

        public virtual bool TryEnter(StateType stateType)
        {
            var conflictStates = GetConflictStates();
            if ((conflictStates & stateType) == stateType)
            {
                return false;
            }
            return true;
        }

        public abstract StateType GetConflictStates();
    }
}