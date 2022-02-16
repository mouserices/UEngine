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
    unsafe class Entitas_Matcher_1_UEngine_EntityAdapter_Binding_Adapter_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(Entitas.Matcher<UEngine.EntityAdapter.Adapter>);
            args = new Type[]{typeof(System.Int32[])};
            method = type.GetMethod("AllOf", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AllOf_0);
            args = new Type[]{typeof(System.String[])};
            method = type.GetMethod("set_componentNames", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_componentNames_1);
            args = new Type[]{typeof(Entitas.IMatcher<UEngine.EntityAdapter.Adapter>[])};
            method = type.GetMethod("AllOf", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AllOf_2);
            args = new Type[]{typeof(System.Int32[])};
            method = type.GetMethod("AnyOf", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AnyOf_3);
            args = new Type[]{typeof(Entitas.IMatcher<UEngine.EntityAdapter.Adapter>[])};
            method = type.GetMethod("AnyOf", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AnyOf_4);


        }


        static StackObject* AllOf_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32[] @indices = (System.Int32[])typeof(System.Int32[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Entitas.Matcher<UEngine.EntityAdapter.Adapter>.AllOf(@indices);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* set_componentNames_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String[] @value = (System.String[])typeof(System.String[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Entitas.Matcher<UEngine.EntityAdapter.Adapter> instance_of_this_method = (Entitas.Matcher<UEngine.EntityAdapter.Adapter>)typeof(Entitas.Matcher<UEngine.EntityAdapter.Adapter>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.componentNames = value;

            return __ret;
        }

        static StackObject* AllOf_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Entitas.IMatcher<UEngine.EntityAdapter.Adapter>[] @matchers = (Entitas.IMatcher<UEngine.EntityAdapter.Adapter>[])typeof(Entitas.IMatcher<UEngine.EntityAdapter.Adapter>[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Entitas.Matcher<UEngine.EntityAdapter.Adapter>.AllOf(@matchers);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* AnyOf_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32[] @indices = (System.Int32[])typeof(System.Int32[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Entitas.Matcher<UEngine.EntityAdapter.Adapter>.AnyOf(@indices);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* AnyOf_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Entitas.IMatcher<UEngine.EntityAdapter.Adapter>[] @matchers = (Entitas.IMatcher<UEngine.EntityAdapter.Adapter>[])typeof(Entitas.IMatcher<UEngine.EntityAdapter.Adapter>[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Entitas.Matcher<UEngine.EntityAdapter.Adapter>.AnyOf(@matchers);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }



    }
}
