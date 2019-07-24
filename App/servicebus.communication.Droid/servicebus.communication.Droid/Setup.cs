using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.IoC;
using servicebus.communication.Core;

namespace servicebus.communication.Droid
{
    public class Setup : MvxAppCompatSetup<App>
    {
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            typeof(Setup).Assembly.CreatableTypes().WithAttribute<TypeServiceAttribute>().AsInterfaces().RegisterAsDynamic();
            typeof(Setup).Assembly.CreatableTypes().WithAttribute<LazySingletonServiceAttribute>().AsInterfaces().RegisterAsLazySingleton();
            typeof(Setup).Assembly.CreatableTypes().WithAttribute<NonLazySingletonServiceAttribute>().AsInterfaces().RegisterAsSingleton();
        }
    }
}
