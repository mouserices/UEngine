using System;
using Entitas.CodeGeneration.Attributes;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

public class ContextAttributeAdaptor : CrossBindingAdaptor
{
    public override Type BaseCLRType
    {
        get { return typeof(ContextAttribute); }
    }

    public override Type AdaptorType
    {
        get { return typeof(Adaptor); }
    }

    public override object CreateCLRInstance(AppDomain appdomain, ILTypeInstance instance)
    {
        return new Adaptor(appdomain, instance);
    }

    public class Adaptor : ContextAttribute, CrossBindingAdaptorType
    {
        AppDomain _appDomain;
        private ILTypeInstance _ilTypeInstance;

        public Adaptor(AppDomain appdomain, ILTypeInstance instance) : base("")
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