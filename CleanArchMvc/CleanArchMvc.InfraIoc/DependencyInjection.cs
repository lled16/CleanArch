using CleanArchMvc.Application.Interfaces;
using CleanArchMvc.Application.Mappings;
using CleanArchMvc.Application.Services;
using CleanArchMvc.Domain.Interfaces;
using CleanArchMvc.InfraData.Context;
using CleanArchMvc.InfraData.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace CleanArchMvc.InfraIoc
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductServices, ProductService>();
            services.AddScoped<ICategoryServices, CategoryService>();
            services.AddAutoMapper(typeof(DomainToDTOMappingProfile));
            services.AddAutoMapper(typeof(DTOToCommandsMappingProfile));

            var myhandlers = AppDomain.CurrentDomain.Load("CleanArchMVC.Application");
            services.AddMediatR(cfg => { cfg.RegisterServicesFromAssemblies([typeof(ProductService).Assembly, typeof(IProductServices).Assembly]); });

            return services;
        }
    }
}
