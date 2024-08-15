using ChildCareApplication.Infrastructure.Repositories;
using ChildCareApplication.Application.Interfaces;
using System.Reflection;
using Microsoft.AspNetCore.Identity.UI.Services;
using ChildCareApplication.Application.CommandHandlers;
using ChildCareApplication.Application.CommandHandlers.ChildQueryHandler;
using ChildCareApplication.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ChildCareApplication.Application.CommandHandlers.ChildCommanHandler;



namespace ChildCareApplication.Infrastructure.Services
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
          

            // Register repositories
            services.AddScoped<IAccount, AccountRepository>();
            services.AddScoped<Ilogin, loginRepository>();
            services.AddScoped<IChildDetail, ChildDetailRepository>();
            services.AddScoped<IParent, ParentDetailRepository>();
            // Register MediatR and all handlers in the assembly
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllChildDetailsQueryHandler).Assembly));


           // services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            //Register Command Handler
            services.AddTransient<IEmailSender, EmailHandler>();
        }
    }
}
