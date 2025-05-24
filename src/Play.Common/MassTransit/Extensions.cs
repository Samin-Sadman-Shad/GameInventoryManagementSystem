using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Play.Common.MassTransit
{
    public static class Extensions
    {
        public static IServiceCollection AddMassTransit(this IServiceCollection services)
        {
            services.AddMassTransit(configure =>
            {
                configure.AddConsumers(Assembly.GetEntryAssembly());

                configure.UsingRabbitMq((context, configurator) =>
                {
                    var configuration = context.GetService<IConfiguration>();
                    var rabbitMqSettings = configuration!.GetSection(RabbitMQSettingsOption.RabbitMQSettings)
                                            .Get<RabbitMQSettingsOption>();
                    configurator.Host(rabbitMqSettings!.Host);

                    //define or modify how queues are created in rabbitMq
                    configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(new ServiceSettings().ServiceName, false));
                });
            });

            return services;
        }
    }
}
