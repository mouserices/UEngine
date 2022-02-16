using System;
using System.Collections.Generic;
using System.Reflection;

namespace ILRuntime.Runtime.Generated
{
    class CLRBindings
    {

//will auto register in unity
#if UNITY_5_3_OR_NEWER
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
#endif
        static private void RegisterBindingAction()
        {
            ILRuntime.Runtime.CLRBinding.CLRBindingUtils.RegisterBindingAction(Initialize);
        }


        /// <summary>
        /// Initialize the CLR binding, please invoke this AFTER CLR Redirection registration
        /// </summary>
        public static void Initialize(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            Entitas_Entity_Binding.Register(app);
            System_Type_Binding.Register(app);
            System_Int32_Binding.Register(app);
            Entitas_Matcher_1_UEngine_EntityAdapter_Binding_Adapter_Binding.Register(app);
            System_String_Binding.Register(app);
            Entitas_ContextInfo_Binding.Register(app);
            Entitas_SafeAERC_Binding.Register(app);
            Entitas_IContext_Binding.Register(app);
            System_Object_Binding.Register(app);
            System_Linq_Enumerable_Binding.Register(app);
            System_Collections_Generic_IEnumerable_1_MethodInfo_Binding.Register(app);
            System_Collections_Generic_IEnumerator_1_MethodInfo_Binding.Register(app);
            System_Reflection_MethodBase_Binding.Register(app);
            System_Collections_IEnumerator_Binding.Register(app);
            System_IDisposable_Binding.Register(app);
            System_Attribute_Binding.Register(app);
            Entitas_CollectorContextExtension_Binding.Register(app);
            System_DateTime_Binding.Register(app);
            UnityEngine_RectTransformUtility_Binding.Register(app);
        }

        /// <summary>
        /// Release the CLR binding, please invoke this BEFORE ILRuntime Appdomain destroy
        /// </summary>
        public static void Shutdown(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
        }
    }
}
