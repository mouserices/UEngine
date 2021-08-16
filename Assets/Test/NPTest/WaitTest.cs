using NPBehave;
using UnityEngine;

namespace Test.NPTest
{
    public class WaitTest : MonoBehaviour
    {
        private Root NPTree;

        public void Start()
        {
            NPTree = new Root(new Sequence(new Action(() =>
            {
                Debug.Log("hellow");
            })));
            
            NPTree.Start();
        }

        public void Update()
        {
            SyncContext.Instance.Update();
        }
    }
}