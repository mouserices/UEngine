public partial class MetaContext
{
    public static T Get<T>() where T : class, IService
    {
        var serviceRegisterServices = Contexts.sharedInstance.meta.service.RegisterServices;
        if (serviceRegisterServices.ContainsKey(typeof(T)))
        {
            return serviceRegisterServices[typeof(T)] as T;
        }
        return null;
    }
}