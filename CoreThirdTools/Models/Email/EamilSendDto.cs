using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreThirdTools.Models.Email
{
    public class EamilSendDto
    {
        /// <summary>
        /// 邮件名称
        /// </summary>
        public string FormName { get; set; }

        /// <summary>
        /// 发送人邮件地址
        /// </summary>
        public string FormAddress { get; set; }

        /// <summary>
        /// 发送人用户名
        /// </summary>
        public string FormUserName { get; set; }

        /// <summary>
        /// 发送人密码
        /// </summary>
        public string FormPassword { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        public List<MailboxAddress> ToMailboxAddress { get; set; }

        /// <summary>
        /// 邮箱协议--qq邮箱:smtp.qq.com
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 端口465
        /// </summary>
        public int Port { get; set; }

        public bool UseSsl { get; set; }
    }
}
