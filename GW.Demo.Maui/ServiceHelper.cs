namespace GW.Demo.Maui;

using System;



public static class ServiceHelper
{
    private static IServiceProvider _services;

    public static IServiceProvider Services
    {
        get { return _services; }
        set { _services = value; }
    }

    public static T GetService<T>() where T : class
    {
        return (T) Services.GetService(typeof(T))!;
    }
}
