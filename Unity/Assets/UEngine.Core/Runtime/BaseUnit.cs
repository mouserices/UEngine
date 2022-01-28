using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class BaseUnit<T>
{
    public virtual void SayHello()
    {
        Debug.Log("hello in BaseUnit");
    }
}
