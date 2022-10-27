using Repository;
using Service;

namespace ApplicationWeb {
    public static class DependencyResolver {

        public static void Resolver(IServiceCollection services) {

            services.AddSingleton<MyContext>();

            services.AddSingleton<EstoqueService>();

        }

        public static void ConfigureContext(IServiceProvider serviceProvider, String connectionString) {

            var context = serviceProvider.GetRequiredService<MyContext>();
            context.SetConnectionString(connectionString);

        }

    }
}
