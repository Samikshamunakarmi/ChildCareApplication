using ChildCareApplication.Infrastructure.Repositories;
using ChildCareApplication.Application.Interfaces;
using System.Reflection;
using Microsoft.AspNetCore.Identity.UI.Services;
using ChildCareApplication.Application.CommandHandlers;
using ChildCareApplication.Application.CommandHandlers.ChildQueryHandler;
using ChildCareApplication.Domain;
using MediatR;



namespace ChildCareApplication.Infrastructure.Services
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            // Add MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            // Register repositories
            services.AddScoped<IAccount, AccountRepository>();
            services.AddScoped<Ilogin, loginRepository>();
            services.AddScoped<IChildDetail, ChildDetailRepository>();

            //Register Command Handler
            services.AddTransient<IEmailSender, EmailHandler>();
        }
    }
}
