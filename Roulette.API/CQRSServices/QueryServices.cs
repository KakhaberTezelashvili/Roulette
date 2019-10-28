using Autofac;
using Microsoft.Extensions.DependencyInjection;
using PBG.Micro.CQRS.Queries;
using Roulette.Application.QueryHandlers;
using Roulette.Domain.Repositories;
using Roulette.Infrastructure.Repositories;

namespace Roulette.API.CQRSServices
{
    public class QueryServices
    {
        public static void RegisterServices(ContainerBuilder builder, IServiceCollection services)
        {
            services.AddTransient<IQueryBusAsync, QueryBusAsync>();
            services.AddTransient<IQueryRepository, QueryRepository>();
            builder.RegisterAssemblyTypes(typeof(QueryHandler).Assembly).AsClosedTypesOf(typeof(IQueryHandlerAsync<,>));
        }
    }
}
