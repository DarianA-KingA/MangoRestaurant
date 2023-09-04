using Mango.Service.OrderAPI.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Mango.Service.OrderAPI.Extension
{
    public static class ApplicationBuilderExtentions
    {
        public static IAzureServiceBusConsumer ServiceBusConsumer { get; set; }
        public static IApplicationBuilder UserAzureServiceBusConsumer(this IApplicationBuilder app)
        {
            ServiceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();
            var hostAplicationLife = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            hostAplicationLife.ApplicationStarted.Register(OnStart);
            hostAplicationLife.ApplicationStopping.Register(OnStop);
            return app;
        }

        private static void OnStart()
        {
            ServiceBusConsumer.Start();
        }
        private static void OnStop()
        {
            ServiceBusConsumer.Stop();
        }
    }
}
