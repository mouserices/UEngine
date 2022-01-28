using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;

namespace UEngine
{   
    public class ReactiveSystem_1_UEngine_EntityAdapter_Binding_AdapterAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(Entitas.ReactiveSystem<UEngine.EntityAdapter.Adapter>);
            }
        }

        public override Type AdaptorType
        {
            get
            {
                return typeof(Adapter);
            }
        }

        public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            return new Adapter(appdomain, instance);
        }

        public class Adapter : Entitas.ReactiveSystem<UEngine.EntityAdapter.Adapter>, CrossBindingAdaptorType
        {
            CrossBindingFunctionInfo<Entitas.IContext<UEngine.EntityAdapter.Adapter>, Entitas.ICollector<UEngine.EntityAdapter.Adapter>> mGetTrigger_0 = new CrossBindingFunctionInfo<Entitas.IContext<UEngine.EntityAdapter.Adapter>, Entitas.ICollector<UEngine.EntityAdapter.Adapter>>("GetTrigger");
            CrossBindingFunctionInfo<UEngine.EntityAdapter.Adapter, System.Boolean> mFilter_1 = new CrossBindingFunctionInfo<UEngine.EntityAdapter.Adapter, System.Boolean>("Filter");
            CrossBindingMethodInfo<System.Collections.Generic.List<UEngine.EntityAdapter.Adapter>> mExecute_2 = new CrossBindingMethodInfo<System.Collections.Generic.List<UEngine.EntityAdapter.Adapter>>("Execute");

            bool isInvokingToString;
            ILTypeInstance instance;
            ILRuntime.Runtime.Enviorment.AppDomain appdomain;

            public Adapter():base(new Context_1_UEngine_EntityAdapter_Binding_AdapterAdapter.Adapter())
            {

            }

            public Adapter(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance):base(new Context_1_UEngine_EntityAdapter_Binding_AdapterAdapter.Adapter())
            {
                this.appdomain = appdomain;
                this.instance = instance;
            }

            public ILTypeInstance ILInstance { get { return instance; } }

            protected override Entitas.ICollector<UEngine.EntityAdapter.Adapter> GetTrigger(Entitas.IContext<UEngine.EntityAdapter.Adapter> context)
            {
                return mGetTrigger_0.Invoke(this.instance, context);
            }

            protected override System.Boolean Filter(UEngine.EntityAdapter.Adapter entity)
            {
                return mFilter_1.Invoke(this.instance, entity);
            }

            protected override void Execute(System.Collections.Generic.List<UEngine.EntityAdapter.Adapter> entities)
            {
                mExecute_2.Invoke(this.instance, entities);
            }

            public override string ToString()
            {
                IMethod m = appdomain.ObjectType.GetMethod("ToString", 0);
                m = instance.Type.GetVirtualMethod(m);
                if (m == null || m is ILMethod)
                {
                    if (!isInvokingToString)
                    {
                        isInvokingToString = true;
                        string res = instance.ToString();
                        isInvokingToString = false;
                        return res;
                    }
                    else
                        return instance.Type.FullName;
                }
                else
                    return instance.Type.FullName;
            }
        }
    }
}

