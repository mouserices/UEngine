using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;

namespace UEngine
{   
    public class IEqualityComparer_1_ILTypeInstanceAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(System.Collections.Generic.IEqualityComparer<ILRuntime.Runtime.Intepreter.ILTypeInstance>);
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

        public class Adapter : System.Collections.Generic.IEqualityComparer<ILRuntime.Runtime.Intepreter.ILTypeInstance>, CrossBindingAdaptorType
        {
            CrossBindingFunctionInfo<ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Boolean> mEquals_0 = new CrossBindingFunctionInfo<ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Boolean>("Equals");
            CrossBindingFunctionInfo<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32> mGetHashCode_1 = new CrossBindingFunctionInfo<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>("GetHashCode");

            bool isInvokingToString;
            ILTypeInstance instance;
            ILRuntime.Runtime.Enviorment.AppDomain appdomain;

            public Adapter()
            {

            }

            public Adapter(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
            {
                this.appdomain = appdomain;
                this.instance = instance;
            }

            public ILTypeInstance ILInstance { get { return instance; } }

            public System.Boolean Equals(ILRuntime.Runtime.Intepreter.ILTypeInstance x, ILRuntime.Runtime.Intepreter.ILTypeInstance y)
            {
                return mEquals_0.Invoke(this.instance, x, y);
            }

            public System.Int32 GetHashCode(ILRuntime.Runtime.Intepreter.ILTypeInstance obj)
            {
                return mGetHashCode_1.Invoke(this.instance, obj);
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

