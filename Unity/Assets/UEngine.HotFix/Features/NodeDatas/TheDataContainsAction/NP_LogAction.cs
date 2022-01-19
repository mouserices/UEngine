using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UEngine.NP
{
    [Title("打印信息",TitleAlignment = TitleAlignments.Centered)]
    public class NP_LogAction:NP_BaseAction
    {
        [LabelText("信息")]
        public string LogInfo;
        
        public override Action GetActionToBeDone()
        {
            this.Action = this.TestLog;
            return this.Action;
        }

        public void TestLog()
        {
            Debug.Log(LogInfo);
        }
    }
}