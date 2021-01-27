using CoreThirdTools.Helper;
using CoreThirdTools.Models.Email;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreThirdTools.Services
{
    public class Email
    {
        public async Task SendEmailAsync(MimeMessage message)
        {
            var host = "smtp.qq.com";
            var port = 465;//587
            var useSsl = true;
            var from_username = "632173516@qq.com";
            var from_password = "vcntoakcrmihbfcc";
            var from_name = "测试";
            var from_address = "632173516@qq.com";

            var address = new List<MailboxAddress>
            {
                new MailboxAddress("111","yingcy@tijian.net"),
                new MailboxAddress("222","yingcy@tijian.net")
            };

            message.From.Add(new MailboxAddress(from_name, from_address));
            message.To.AddRange(address);

            using var client = new SmtpClient
            {
                ServerCertificateValidationCallback = (s, c, h, e) => true
            };
            client.AuthenticationMechanisms.Remove("XOAUTH2");

            await client.ConnectAsync(host, port, useSsl);
            await client.AuthenticateAsync(from_username, from_password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        public async Task SendEmailAsync(MimeMessage message, EamilSendDto eamilSendDto)
        {
            await EmailHelper.SendAsync(message, eamilSendDto);
        }
    }
}
