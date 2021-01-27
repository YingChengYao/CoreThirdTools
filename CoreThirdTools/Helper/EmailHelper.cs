using CoreThirdTools.Models.Email;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreThirdTools.Helper
{
    public static class EmailHelper
    {
        /// <summary>
        /// 发送Email
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task SendAsync(MimeMessage message)
        {
            if (!message.From.Any())
            {
                //发件人
                message.From.Add(new MailboxAddress(AppSettings.Email.From.Name, AppSettings.Email.From.Address));
            }
            if (!message.To.Any())
            {
                //收件人
                var address = AppSettings.Email.To.Select(x => new MailboxAddress(x.Key, x.Value));
                message.To.AddRange(address);
            }

            using var client = new SmtpClient
            {
                ServerCertificateValidationCallback = (s, c, h, e) => true
            };
            client.AuthenticationMechanisms.Remove("XOAUTH2");

            await client.ConnectAsync(AppSettings.Email.Host, AppSettings.Email.Port, AppSettings.Email.UseSsl);
            await client.AuthenticateAsync(AppSettings.Email.From.Username, AppSettings.Email.From.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        /// <summary>
        /// 发送Email
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task SendAsync(MimeMessage message, EamilSendDto eamilSendDto)
        {
            if (!message.From.Any())
            {
                //发件人
                message.From.Add(new MailboxAddress(eamilSendDto.FormName, eamilSendDto.FormAddress));
            }
            if (!message.To.Any())
            {
                //收件人
                var address = AppSettings.Email.To.Select(x => new MailboxAddress(x.Key, x.Value));
                message.To.AddRange(eamilSendDto.ToMailboxAddress);
            }

            using var client = new SmtpClient
            {
                ServerCertificateValidationCallback = (s, c, h, e) => true
            };
            client.AuthenticationMechanisms.Remove("XOAUTH2");

            await client.ConnectAsync(eamilSendDto.Host, eamilSendDto.Port, eamilSendDto.UseSsl);
            await client.AuthenticateAsync(eamilSendDto.FormUserName, eamilSendDto.FormPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
