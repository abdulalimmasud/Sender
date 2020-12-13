using AstManagement.AssetDB;
using AstManagement.AssetDB.Repositories;
using AstManagement.AssetDB.SeedData;
using AstManagement.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AstManagement.Services;

namespace AstManagement.Extensions
{
    public static class ServiceExtension
    {
        public static void ServicesRegister(this IServiceCollection services, IConfiguration configuration)
        {
            #region settings
            string connectionString = configuration.GetConnectionString(nameof(EyeAssetDbContext));
            services.AddDbContext<EyeAssetDbContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<DbContext, EyeAssetDbContext>();

            var appOption = configuration.GetSection(nameof(ApplicationOptions));
            services.Configure<ApplicationOptions>(appOption);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = ApiKeyAuthenticationOptions.DefaultScheme;
                options.DefaultChallengeScheme = ApiKeyAuthenticationOptions.DefaultScheme;
            }).AddApiKeySupport(options=> { });
            #endregion

            services.AddTransient<IAppRepository, AppRepository>();
            services.AddTransient<IAppService, AppService>();

            services.AddTransient<IAttachmentRepository, AttachmentRepository>();
            services.AddTransient<IAttachmentService, AttachmentService>();

            services.AddTransient<IAttachmentLogRepository, AttachmentLogRepository>();
            services.AddTransient<IAttachmentLogService, AttachmentLogService>();

            services.AddTransient<IMobileOperatorRepository, MobileOperatorRepository>();
            services.AddTransient<IMobileOperatorService, MobileOperatorService>();

            services.AddTransient<IMobileOperatorPackagesRepository, MobileOperatorPackagesRepository>();
            services.AddTransient<IMobileOperatorPackagesService, MobileOperatorPackagesService>();

            services.AddTransient<ISimCardRepository, SimCardRepository>();
            services.AddTransient<ISimCardService, SimCardService>();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();



            #region defaultdata
            AssetDbSeedData.InsertUpdateSeedData(connectionString);
            #endregion
        }
    }
}
