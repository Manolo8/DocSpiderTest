using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Instances {
    public static class Instantiator {
        public static void Instantiate(Assembly assembly, IServiceCollection services) {
            var found = assembly.GetTypes();

            var scoped    = found.Where(e => typeof(IScoped).IsAssignableFrom(e)).ToList();
            var singleton = found.Where(e => typeof(ISingleton).IsAssignableFrom(e)).ToList();

            scoped.ForEach(e => services.AddScoped(e));
            singleton.ForEach(e => services.AddSingleton(e));
        }
    }
}