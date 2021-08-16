using NPBehave;
using UnityEngine;

namespace Test.NPTest
{
    public class ConditionStopTest : MonoBehaviour
    {
        private Root NPTree;

        public void Start()
        {
            NPTree = new Root(new Sequence(new Condition(() => { return Input.GetKeyDown(KeyCode.A);}, Stops.SELF, new Action(
                (bool b) =>
                {
                    Debug.Log("点击了A");
                    return Action.Result.FAILED;
                })), new Action(() =>
            {
                Debug.Log("点击了Aqqqq");
            })));

            NPTree.Start();
        }

        public void Update()
        {
            SyncContext.Instance.Update();
        }
    }
}