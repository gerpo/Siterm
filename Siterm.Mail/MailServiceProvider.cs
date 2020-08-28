using MailKit.Net.Smtp;
using Microsoft.Extensions.DependencyInjection;
using Siterm.Mail.Services;

namespace Siterm.Mail
{
    public static class MailServiceProvider
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<SmtpClient>();
            services.AddHostedService<InstructionWarningMailService>();
            services.AddHostedService<ServiceReportWarningMailService>();
        }
    }
}