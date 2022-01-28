using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BaseUnitExtend 
{
    public static void Say<T>(this BaseUnit<T> baseUnit)
    {
        baseUnit.SayHello();
    }
}
