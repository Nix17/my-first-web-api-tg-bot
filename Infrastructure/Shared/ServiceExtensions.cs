using Application.Interfaces.Services;
using Domain.Settings;
using Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Shared;

public static class ServiceRegistration
{
    public static void AddSharedInfrastructure(this IServiceCollection services, IConfigurationRoot config)
    {
        services.Configure<MailSettings>(config.GetSection("MailSettings"));
        services.AddTransient<IDateTimeService, DateTimeService>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IExcelService, ExcelService>();
    }
}