using System.Threading.Tasks;
using Autofac;

namespace Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var cb = new ContainerBuilder();
            cb.RegisterModule<AppModule>();
            await using var container = cb.Build();
            await container.Resolve<App>().Run();
        }
    }
}
