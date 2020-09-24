using System.Net.Http;
using Autofac;

namespace Sample
{
    public class AppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterInstance(new HttpClient());

            builder.RegisterType<App>()
                .AsSelf()
                .SingleInstance();
        }
    }
}