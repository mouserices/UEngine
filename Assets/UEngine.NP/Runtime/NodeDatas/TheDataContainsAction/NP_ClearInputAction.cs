
    using System;
    using Sirenix.OdinInspector;
    using UEngine.NP;
    using UnityEngine;

    public class NP_ClearInputAction : NP_BaseAction
    {
        [LabelText("清除的Key")]
        public KeyCode KeyCode;
        public override Action GetActionToBeDone()
        {
            this.Action = () =>
            {
                var entity = Contexts.sharedInstance.game.GetEntityWithUnit(this.Skill.UnitID);
                if (entity.hasInputKey)
                {
                    entity.inputKey.KeyCodes.Remove(KeyCode);
                }
            };
            return this.Action;
        }
    }
