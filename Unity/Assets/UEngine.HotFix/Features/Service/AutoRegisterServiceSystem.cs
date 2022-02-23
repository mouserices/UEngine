using System;
using System.Collections.Generic;
using Entitas;

public class AutoRegisterServiceSystem : IInitializeSystem
{
    public void Initialize()
    {
        var metaContext = Contexts.sharedInstance.meta;

        Dictionary<Type, IService> services = new Dictionary<Type, IService>();

        Type[] types;
#if SERVER
        types =  this.GetType().Assembly.GetTypes();
#elif CLIENT
        types = Main.GetTypes().ToArray();
#endif
        foreach (var type in types)
        {
            if (!type.IsClass)
            {
                continue;
            }
            var attributes = type.GetCustomAttributes(typeof(ServiceAttribute), true);
            if (attributes.Length > 0)
            {
                var instance = Activator.CreateInstance(type);
                foreach (var @interface in type.GetInterfaces())
                {
                    if (@interface.GetInterfaces().Length > 0)
                    {
                        services.Add(@interface, instance as IService);
                    }
                }
            }
        }

        metaContext.SetService(services);
    }
}