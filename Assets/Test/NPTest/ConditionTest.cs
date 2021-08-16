using System;
using NPBehave;
using UnityEngine;
using Action = NPBehave.Action;

namespace Test.NPTest
{
    public class ConditionTest : MonoBehaviour
    {
        private Root NPTree;

        public void Start()
        {
            NPTree = new Root(new Condition(
                () => { return Input.GetKeyDown(KeyCode.A); },
                new Action(() => { Debug.Log("点击了A"); })));

            NPTree.Start();
        }

        public void Update()
        {
            SyncContext.Instance.Update();
        }
    }
}