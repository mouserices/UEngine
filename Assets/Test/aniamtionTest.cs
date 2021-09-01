using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

public class aniamtionTest : MonoBehaviour
{
    public AnimationClip AnimationClip;
    // Start is called before the first frame update
    void Start()
    {
        // var addComponent = this.gameObject.AddComponent<View>();
        // Debug.Log(addComponent == null);
        this.GetComponent<AnimancerComponent>().Play(AnimationClip);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
