using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Assignment.EntityFrameworkCore.Context;
using Assignment.Framework;
using Assignment.Domain.Repositories;
using Assignment.EntityFrameworkCore.Repository;

namespace Assignment.EntityFrameworkCore
{
    [ExcludeFromCodeCoverage]
    public static class EfCoreServiceCollectionExtensions
    {
        public static void RegisterEfCoreDependencies(this IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            services.AddDbContext<AssignmentDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("AssignmentDbContext")));
            services.AddScoped<IDbFactory, DbFactory>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<ITransactionRepository, TransactionRepository>();
        }
    }
}
