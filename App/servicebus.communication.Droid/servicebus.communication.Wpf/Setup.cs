using MvvmCross.IoC;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.Plugin;
using servicebus.communication.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace servicebus.communication.Wpf
{
    public class Setup : MvxWpfSetup<servicebus.communication.Core.App>
    {

        public override void LoadPlugins(IMvxPluginManager pluginManager)
        {
            var pluginAttribute = typeof(MvxPluginAttribute);

            var pluginTypes =
                GetPluginAssemblies()
                    .SelectMany(assembly => assembly.GetTypes())
                    .Where(TypeContainsPluginAttribute);

            foreach (var pluginType in pluginTypes)
            {
                pluginManager.EnsurePluginLoaded(pluginType);
            }

            bool TypeContainsPluginAttribute(Type type)
                => (type.GetCustomAttributes(pluginAttribute, false)?.Length ?? 0) > 0;
        }

        protected virtual IEnumerable<Assembly> LoadAllReferencedAssemblies(Assembly assembly)
        {
            var loadedAssemblies = new HashSet<Assembly>();
            LoadReferencedAssemblies(assembly, loadedAssemblies);
            return loadedAssemblies;
        }

        public override IEnumerable<Assembly> GetPluginAssemblies()
        {
            var mvvmCrossAssemblyName = typeof(MvxPluginAttribute).Assembly.GetName().Name;
            var allAssemblies = LoadAllReferencedAssemblies(Assembly.GetEntryAssembly());
            var pluginAssemblies =
                allAssemblies
                    .AsParallel()
                    .Where(asmb => AssemblyReferencesMvvmCross(asmb, mvvmCrossAssemblyName));

            return pluginAssemblies;
        }

        private void LoadReferencedAssemblies(Assembly assembly, ISet<Assembly> loadedAssemblies)
        {
            foreach (var referencedAssembly in assembly.GetReferencedAssemblies())
            {
                try
                {
                    var loadedAssembly = Assembly.Load(referencedAssembly);
                    if (loadedAssemblies.Add(loadedAssembly))
                    {
                        LoadReferencedAssemblies(loadedAssembly, loadedAssemblies);
                    }
                }
                catch
                {
                    continue;
                }
            }
        }

        private bool AssemblyReferencesMvvmCross(Assembly assembly, string mvvmCrossAssemblyName)
        {
            try
            {
                return assembly.GetReferencedAssemblies().Any(a => a.Name == mvvmCrossAssemblyName);
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            typeof(Setup).Assembly.CreatableTypes().WithAttribute<TypeServiceAttribute>().AsInterfaces().RegisterAsDynamic();
            typeof(Setup).Assembly.CreatableTypes().WithAttribute<LazySingletonServiceAttribute>().AsInterfaces().RegisterAsLazySingleton();
            typeof(Setup).Assembly.CreatableTypes().WithAttribute<NonLazySingletonServiceAttribute>().AsInterfaces().RegisterAsSingleton();
        }
    }
}
