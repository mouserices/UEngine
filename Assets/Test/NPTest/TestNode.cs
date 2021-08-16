using System.Collections;
using System.Collections.Generic;
using NPBehave;
using UnityEngine;

public class TestNode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var root = new Root(new Sequence(
            new Action((bool b) =>
                {
                    Debug.Log("log1");
                    return Action.Result.FAILED;
                }
            )
            , new Action(() => { Debug.Log("log2"); })));
        root.Start();
    }

    // Update is called once per frame
    void Update()
    {
    }
}