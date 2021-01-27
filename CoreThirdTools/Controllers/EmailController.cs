using CoreThirdTools.Models.Email;
using CoreThirdTools.Services;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreThirdTools.Controllers
{
    [ApiController]
    public class EmailController : ControllerBase
    {
        [HttpPost]
        [Route("sendEmail")]
        public async Task SendEmailAsync([FromServices] Email _email)
        {
            var message = new MimeMessage
            {
                Subject = "我是邮件主题",
                Body = new BodyBuilder
                {
                    //TextBody="",
                    HtmlBody = $"我是邮件内容，时间:{DateTime.Now:yyyy-MM-dd HH:mm:ss}"
                }.ToMessageBody()
            };

            //await _email.SendEmailAsync(message);

            EamilSendDto eamilSendDto = new EamilSendDto()
            {
                FormName = "测试",
                FormAddress = "6321735161@qq.com",
                FormUserName = "632173516@qq.com",
                FormPassword = "vcntoakcrmihbfcc",
                ToMailboxAddress = new List<MailboxAddress>
                {
                    new MailboxAddress("111","yingcy@tijian.net"),
                    new MailboxAddress("222","yingcy@tijian.net")
                },
                Host = "smtp.qq.com",
                Port = 465,
                UseSsl = true
            };
            await _email.SendEmailAsync(message, eamilSendDto);
        }

        [HttpPost]
        [Route("sendEmailImage")]
        public async Task SendEmailImageAsync([FromServices] Email _email)
        {
            var path = "D:\\bg.jpg";

            var builder = new BodyBuilder();

            var image = builder.LinkedResources.Add(path);
            image.ContentId = MimeUtils.GenerateMessageId();

            builder.HtmlBody = $"当前时间:{DateTime.Now:yyyy-MM-dd HH:mm:ss} <img src=\"cid:{image.ContentId}\"/>";

            var message = new MimeMessage
            {
                Subject = "带图片的邮件推送",
                Body = builder.ToMessageBody()
            };

            await _email.SendEmailAsync(message);
        }
    }
}