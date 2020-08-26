using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Siterm.Mail
{
    public class MailBackgroundTask  : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                return Task.CompletedTask;
            }
        }
    }
}
