using Assignment.Application.Contracts.ServiceInterfaces;
using Assignment.Application.Services;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Assignment.Application
{
    public static class ApplicationServiceCollectionExtension
    {
        public static void RegisterApplicationDependencies(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddTransient<ITransactionService, TransactionService>();
        }
    }
}
