using UnityEngine;


public struct WrapperUnityObjectComponent<T>
    where T : Object
{
    public T Value;
}