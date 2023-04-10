using Microsoft.Extensions.DependencyInjection;
using Customer.Bl.Queries.GetCustomers;
using Customer.Dal.Interfaces.Repositories;
using FluentValidation.AspNetCore;
using Customer.Bl.Validators;
using FluentValidation;

namespace Customer.ApplicationSetup
{
    public static class ApplicationConfigurationExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddMediator();
            services.AddRepositories();
            services.AddFluentValidation();
        }

        private static void AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetCustomersQueryRequest).Assembly));
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.Scan(scan => scan
            .FromAssemblyOf<ICustomerRepository>()
            .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Repository")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        }

        private static void AddFluentValidation(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<AddCustomerCommandRequestValidator>();
        }
    }
}