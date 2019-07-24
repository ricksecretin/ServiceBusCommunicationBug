using MvvmCross.IoC;
using MvvmCross.ViewModels;

namespace servicebus.communication.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();
            
            typeof(App).Assembly.CreatableTypes().WithAttribute<TypeServiceAttribute>().AsInterfaces().RegisterAsDynamic();
            typeof(App).Assembly.CreatableTypes().WithAttribute<LazySingletonServiceAttribute>().AsInterfaces().RegisterAsLazySingleton();
            typeof(App).Assembly.CreatableTypes().WithAttribute<NonLazySingletonServiceAttribute>().AsInterfaces().RegisterAsSingleton();

            RegisterCustomAppStart<AppStart>();
        }
    }
}
