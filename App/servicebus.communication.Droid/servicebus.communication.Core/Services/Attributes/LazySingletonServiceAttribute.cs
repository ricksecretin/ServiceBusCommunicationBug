using System;
namespace servicebus.communication.Core
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class LazySingletonServiceAttribute : Attribute
    {
    }
}
