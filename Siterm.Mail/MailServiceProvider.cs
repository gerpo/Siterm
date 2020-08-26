using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Siterm.Mail
{
    public static class MailServiceProvider
    {
        public static void RegisterServices(IServiceCollection services)
        {
            Host.CreateDefaultBuilder().UseWindowsService().ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<MailBackgroundTask>();
            });
        }
    }
}
