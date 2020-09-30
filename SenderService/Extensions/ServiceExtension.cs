using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationDb;
using NotificationDb.Repositories;
using SenderService.Configuration;
using SenderService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SenderService.Extensions
{
    public static class ServiceExtension
    {
        public static void ServiceResgister(this IServiceCollection services, IConfiguration configuration)
        {
            var smartMailSection = configuration.GetSection(nameof(SmarterMail));
            services.Configure<SmarterMail>(smartMailSection);

            var smsCredetial = configuration.GetSection(nameof(SmsCredential));
            services.Configure<SmsCredential>(smsCredetial);

            var appOptions = configuration.GetSection(nameof(ApplicationOptions));
            services.Configure<ApplicationOptions>(appOptions);

            var connectionString = configuration.GetConnectionString(nameof(NotificationDbContext));
            services.AddDbContext<NotificationDbContext>(options => options.UseSqlServer(connectionString));

            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<ISmsService, SmsService>();

            services.AddTransient<ISmsRepository, SmsRepository>();
            services.AddTransient<IEmailRepository, EmailRepository>();
        }
    }
}
