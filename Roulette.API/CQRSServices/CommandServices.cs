using Autofac;
using Microsoft.Extensions.DependencyInjection;
using PBG.Micro.CQRS.Commands;
using Roulette.Application.CommandHandlers;
using Roulette.Domain.Repositories;
using Roulette.Infrastructure.Repositories;

namespace Roulette.API.CQRSServices
{
    public class CommandServices
    {
        public static void RegisterServices(ContainerBuilder builder, IServiceCollection services)
        {
            services.AddTransient<ICommandBusAsync, CommandBusAsync>();
            services.AddTransient<ICommandRepository, CommandRepository>();
            builder.RegisterAssemblyTypes(typeof(CommandHandler).Assembly).AsClosedTypesOf(typeof(ICommandHandlerAsync<,>));
            builder.RegisterAssemblyTypes(typeof(CommandHandler).Assembly).AsClosedTypesOf(typeof(ICommandHandlerAsync<>));
        }
    }
}