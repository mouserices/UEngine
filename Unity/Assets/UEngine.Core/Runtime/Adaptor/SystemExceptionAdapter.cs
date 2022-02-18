using System;
using System.Collections;
using System.Collections.Generic;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using UnityEngine;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

public class SystemExceptionAdapter : CrossBindingAdaptor
{
    public override Type BaseCLRType {
        get
        {
            return typeof(Exception);
        }
    }
    public override Type AdaptorType {
        get
        {
            return typeof(Adapter);
        }
    }
    public override object CreateCLRInstance(AppDomain appdomain, ILTypeInstance instance)
    {
        throw new NotImplementedException();
    }
    
    public class Adapter : SystemException,CrossBindingAdaptorType
    {
        public ILTypeInstance ILInstance { get; }
    }
}
