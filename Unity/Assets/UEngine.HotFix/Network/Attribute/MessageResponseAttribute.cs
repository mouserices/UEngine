using System;
using System.Reflection;
using UnityEngine;

namespace UEngine.Net
{
    public class MessageResponseAttribute : Attribute
    {
        public Type Type { get; }

        public MessageResponseAttribute(Type t)
        {
            //Debug.LogError($"22222222============ {t} {t.GetTypeInfo()}");
            Type = t;
        }
    }
}