namespace GW.Demo.Maui.Utility;

using System;



public static class ServiceHelper
{
    private static IServiceProvider? _Services;


    public static IServiceProvider? Services
    {
        get { return _Services; }
        set { _Services = value; }
    }


    public static T GetService<T>() where T: class
    {
        return (T) Services?.GetService(typeof(T))!;
    }
}
