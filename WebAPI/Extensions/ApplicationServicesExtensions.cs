using DAL;
using DAL.Context;
using DAL.Interfaces;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Extensions;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration config)
    {
        services.AddDbContext<SocialNetworkContext>(options =>
        {
            options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        });
        
        services.AddCors();
        //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        
        return services;
    }
}