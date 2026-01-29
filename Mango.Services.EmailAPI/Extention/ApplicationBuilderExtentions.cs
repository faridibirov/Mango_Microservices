using Mango.Services.EmailAPI.Messaging;

namespace Mango.Services.EmailAPI.Extention;

public static class ApplicationBuilderExtentions
{
    private static IAzureServiceBusConsumer ServiceBusConsumer { get; set; }
    public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder builder)
    {
        ServiceBusConsumer = builder.ApplicationServices.GetService<IAzureServiceBusConsumer>();
        var hostApplicationLife = builder.ApplicationServices.GetService<IHostApplicationLifetime>();

        hostApplicationLife.ApplicationStarted.Register(OnStart);
        hostApplicationLife.ApplicationStopping.Register(OnStop);
        return builder; 
    }

    private static void OnStop()
    {
       ServiceBusConsumer.Stop();
    }

    private static void OnStart()
    {
       ServiceBusConsumer.Start();
    }
}
