using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Hosting;
using MimeKit;
using Serilog;
using Siterm.Domain.Models;
using Siterm.EntityFramework.Services;
using Siterm.Mail.Resources;
using Siterm.Settings.Models;
using Siterm.Settings.Services;
using Siterm.Support.Misc;

namespace Siterm.Mail.Services
{
    public class ServiceReportWarningMailService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly ServiceReportDataService _serviceReportDataService;
        private readonly SettingsProvider _settingsProvider;
        private readonly SmtpClient _smtpClient;

        public ServiceReportWarningMailService(SettingsProvider settingsProvider, SmtpClient smtpClient,
            ServiceReportDataService serviceReportDataService,
            ILogger logger)
        {
            _settingsProvider = settingsProvider;
            _smtpClient = smtpClient;
            _serviceReportDataService = serviceReportDataService;
            _logger = logger;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _smtpClient.DisconnectAsync(true, cancellationToken);

            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
                try
                {
                    await ConfigureClient();
                    var sender = GetSender();
                    var ccRecipients = GetCcRecipients();

                    var serviceReports = await GetServiceReportsToNotifyAsync();

                    foreach (var serviceReport in serviceReports)
                    {
                        await _serviceReportDataService.MarkAsNotified(serviceReport);

                        try
                        {
                            var message = ComposeMessage(serviceReport, sender, ccRecipients);
                            await _smtpClient.SendAsync(message, stoppingToken);
                        }
                        catch (Exception e)
                        {
                            _logger.Error(e,
                                string.Format(LogStrings.InstructionMailNotificationError, serviceReport.Path));

                            await _serviceReportDataService.MarkAsNotNotified(serviceReport);
                        }
                    }

                    break;
                }
                catch (Exception e)
                {
                    _logger.Error(e, LogStrings.InstructionsMailNotificationError);
                }
        }

        private static MimeMessage ComposeMessage(ServiceReport serviceReport, MailboxAddress sender,
            IEnumerable<MailboxAddress> ccRecipients)
        {
            var bodyBuilder = new BodyBuilder
            {
                TextBody = string.Format(UiStrings.ServiceReportMailBody, serviceReport.Device.Name,
                    serviceReport.ValidTill.ToShortDateString())
            };
            if (File.Exists(serviceReport.Path))
                bodyBuilder.Attachments.Add(serviceReport.Path);

            var message = new MimeMessage
            {
                Sender = sender,
                Subject = string.Format(UiStrings.ServiceReportMailSubject, serviceReport.Device.Name,
                    serviceReport.ValidTill.ToShortDateString()),

                Body = bodyBuilder.ToMessageBody(),
            };

            if (serviceReport.Device?.Chief != null && !string.IsNullOrEmpty(serviceReport.Device.Chief.Email) &&
                Helper.IsValidEmail(serviceReport.Device.Chief.Email))
                message.To.Add(
                    new MailboxAddress(serviceReport.Device.Chief.FullName, serviceReport.Device.Chief.Email));

            message.Cc.AddRange(ccRecipients);

            return message;
        }

        private async Task ConfigureClient()
        {
            var smtpServer = _settingsProvider.GetSetting(SettingName.SmtpServer).Value;
            var smtpPort = int.Parse(_settingsProvider.GetSetting(SettingName.SmtpServerPort).Value);
            var userName = _settingsProvider.GetSetting(SettingName.MailUserName).Value;
            var password = _settingsProvider.GetSetting(SettingName.MailPassword).Value;

            await _smtpClient.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
            await _smtpClient.AuthenticateAsync(userName, password);
        }

        private List<MailboxAddress> GetCcRecipients()
        {
            var ccRecipientsString = _settingsProvider.GetSetting(SettingName.CCEmailAddresses).Value;
            var ccRecipient = ccRecipientsString.Split(';');

            var ccRecipientList = new List<MailboxAddress>();

            foreach (var cc in ccRecipient)
                if (MailboxAddress.TryParse(cc, out var address) && Helper.IsValidEmail(cc))
                    ccRecipientList.Add(address);

            return ccRecipientList;
        }

        private MailboxAddress GetSender()
        {
            var senderAddress = _settingsProvider.GetSetting(SettingName.MailSenderAddress).Value;
            return new MailboxAddress(UiStrings.SenderName, senderAddress);
        }

        private async Task<IEnumerable<ServiceReport>> GetServiceReportsToNotifyAsync()
        {
            return await _serviceReportDataService.GetAllForMailNotification();
        }
    }
}