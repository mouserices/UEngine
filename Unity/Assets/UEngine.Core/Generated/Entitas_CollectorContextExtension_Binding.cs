using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;

namespace ILRuntime.Runtime.Generated
{
    unsafe class Entitas_CollectorContextExtension_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            MethodBase method;
            Type[] args;
            Type type = typeof(Entitas.CollectorContextExtension);
            Dictionary<string, List<MethodInfo>> genericMethods = new Dictionary<string, List<MethodInfo>>();
            List<MethodInfo> lst = null;                    
            foreach(var m in type.GetMethods())
            {
                if(m.IsGenericMethodDefinition)
                {
                    if (!genericMethods.TryGetValue(m.Name, out lst))
                    {
                        lst = new List<MethodInfo>();
                        genericMethods[m.Name] = lst;
                    }
                    lst.Add(m);
                }
            }
            args = new Type[]{typeof(UEngine.EntityAdapter.Adapter)};
            if (genericMethods.TryGetValue("CreateCollector", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(Entitas.ICollector<UEngine.EntityAdapter.Adapter>), typeof(Entitas.IContext<UEngine.EntityAdapter.Adapter>), typeof(Entitas.IMatcher<UEngine.EntityAdapter.Adapter>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, CreateCollector_0);

                        break;
                    }
                }
            }


        }


        static StackObject* CreateCollector_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Entitas.IMatcher<UEngine.EntityAdapter.Adapter> @matcher = (Entitas.IMatcher<UEngine.EntityAdapter.Adapter>)typeof(Entitas.IMatcher<UEngine.EntityAdapter.Adapter>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Entitas.IContext<UEngine.EntityAdapter.Adapter> @context = (Entitas.IContext<UEngine.EntityAdapter.Adapter>)typeof(Entitas.IContext<UEngine.EntityAdapter.Adapter>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Entitas.CollectorContextExtension.CreateCollector<UEngine.EntityAdapter.Adapter>(@context, @matcher);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }



    }
}
