﻿using BLL.Helpers;
using BLL.Interfaces;
using BLL.Mapping;
using BLL.Services;
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

        
        services.AddAutoMapper(typeof(AutoMapperProfile));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IPhotoService, PhotoService>();

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
        
        return services;
    }
}