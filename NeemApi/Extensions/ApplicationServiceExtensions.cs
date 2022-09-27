using Microsoft.EntityFrameworkCore;
using NeemApi.Data;
using NeemApi.Helper;
using NeemApi.Interfaces;
using NeemApi.Services;
using NeemApi.Settings;

namespace NeemApi.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IFavoritesRepository, FavoritesRepository>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.Configure<MailSettings>(config.GetSection("MailSettings"));
            services.AddTransient<IMailService, Services.MailService>();

            return services;
        }
    }
}
