using System;
using Entitas;
using Entitas.Unity;
using UnityEngine;

[RequireComponent(typeof(View))]
public class OnTriggerCallBack : MonoBehaviour
{
    public Action<Collider> TriggerEnter;
    public void OnTriggerEnter(Collider other)
    {
        if (TriggerEnter != null)
        {
            TriggerEnter.Invoke(other);
        }
    }
}