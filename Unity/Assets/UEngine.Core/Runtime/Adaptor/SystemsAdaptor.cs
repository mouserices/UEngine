using System;
using Entitas;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using UEngine;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

public class SystemsAdaptor : CrossBindingAdaptor
{
    public override Type BaseCLRType
    {
        get { return typeof(Systems); }
    }

    public override Type AdaptorType
    {
        get { return typeof(Adaptor); }
    }

    public override object CreateCLRInstance(AppDomain appdomain, ILTypeInstance instance)
    {
        return new Adaptor(appdomain,instance);
    }
    public class Adaptor : Systems,CrossBindingAdaptorType
    {
        private AppDomain _appDomain;
        private ILTypeInstance _ilTypeInstance;
        public Adaptor(AppDomain appdomain, ILTypeInstance instance)
        {
            _appDomain = appdomain;
            _ilTypeInstance = instance;
        }

        public ILTypeInstance ILInstance
        {
            get { return _ilTypeInstance; }
        }
    }
}