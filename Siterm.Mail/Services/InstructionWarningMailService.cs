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
    public class InstructionWarningMailService : BackgroundService
    {
        private readonly InstructionDataService _instructionDataService;
        private readonly ILogger _logger;
        private readonly SettingsProvider _settingsProvider;
        private readonly SmtpClient _smtpClient;

        public InstructionWarningMailService(SettingsProvider settingsProvider, SmtpClient smtpClient,
            InstructionDataService instructionDataService,
            ILogger logger)
        {
            _settingsProvider = settingsProvider;
            _smtpClient = smtpClient;
            _instructionDataService = instructionDataService;
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

                    var instructions = await GetInstructionsToNotifyAsync();

                    foreach (var instruction in instructions)
                    {
                        await _instructionDataService.MarkAsNotified(instruction);

                        try
                        {
                            var message = ComposeMessage(instruction, sender, ccRecipients);
                            await _smtpClient.SendAsync(message, stoppingToken);
                        }
                        catch (Exception e)
                        {
                            _logger.Error(e,
                                string.Format(LogStrings.InstructionMailNotificationError, instruction.Path));

                            await _instructionDataService.MarkAsNotNotified(instruction);
                        }
                    }

                    break;
                }
                catch (Exception e)
                {
                    _logger.Error(e, LogStrings.InstructionsMailNotificationError);
                }
        }

        private static MimeMessage ComposeMessage(Instruction instruction, MailboxAddress sender,
            IEnumerable<MailboxAddress> ccRecipients)
        {
            var receiver = instruction.Instructed is null
                ? instruction.OldInstructedString.ToTitleCase()
                : instruction.Instructed.FullName;

            var bodyBuilder = new BodyBuilder
            {
                TextBody = string.Format(UiStrings.InstructionMailBody, receiver, instruction.Device.Name,
                    instruction.ValidTill.ToShortDateString()),
            };

            if (File.Exists(instruction.Path))
                bodyBuilder.Attachments.Add(instruction.Path);

            var message = new MimeMessage
            {
                Sender = sender,
                Subject = string.Format(UiStrings.InstructionMailSubject, instruction.Device.Name,
                    instruction.ValidTill.ToShortDateString()),

                Body = bodyBuilder.ToMessageBody()
            };

            if (!(instruction.Instructed is null) && !string.IsNullOrEmpty(instruction.Instructed.Email) &&
                Helper.IsValidEmail(instruction.Instructed.Email))
                message.To.Add(new MailboxAddress(instruction.Instructed.FullName, instruction.Instructed.Email));

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

        private async Task<IEnumerable<Instruction>> GetInstructionsToNotifyAsync()
        {
            return await _instructionDataService.GetAllForMailNotification();
        }

        private MailboxAddress GetSender()
        {
            var senderAddress = _settingsProvider.GetSetting(SettingName.MailSenderAddress).Value;
            return new MailboxAddress(UiStrings.SenderName, senderAddress);
        }
    }
}