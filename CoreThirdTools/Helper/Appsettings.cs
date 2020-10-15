using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreThirdTools.Helper
{
    /// <summary>
    /// appsettings.json操作类
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// 配置文件的根节点
        /// </summary>
        private static readonly IConfigurationRoot _config;

        //static string contentPath { get; set; }

        //public Appsettings(string contentPath)
        //{
        //    string Path = "appsettings.json";

        //    //如果配置文件 是 根据环境变量来分开了，可以这样写
        //    //Path = $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json";

        //    _config = new ConfigurationBuilder()
        //       .SetBasePath(contentPath)
        //       .Add(new JsonConfigurationSource { Path = Path, Optional = false, ReloadOnChange = true })//这样的话，可以直接读目录里的json文件，而不是 bin 文件夹下的，所以不用修改复制属性
        //       .Build();
        //}

        static AppSettings()
        {
            // 加载appsettings.json，并构建IConfigurationRoot
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true);
            _config = builder.Build();
        }

        /// <summary>
        /// JWT
        /// </summary>
        public static class JWT
        {
            /// <summary>
            /// Token的时间偏移量
            /// </summary>
            public static string ClockSkew => _config["JWT:ClockSkew"];

            /// <summary>
            /// 访问群体
            /// </summary>
            public static string ValidAudience => _config["JWT:ValidAudience"];

            /// <summary>
            /// 颁发者
            /// </summary>
            public static string ValidIssuer => _config["JWT:ValidIssuer"];

            /// <summary>
            /// 密钥
            /// </summary>
            public static string SecurityKey => _config["JWT:SecurityKey"];

            /// <summary>
            /// 过期时间
            /// </summary>
            public static int Expires => Convert.ToInt32(_config["JWT:Expires"]);
        }

        public static class Email
        {
            /// <summary>
            /// Host
            /// </summary>
            public static string Host => _config["Email:Host"];

            /// <summary>
            /// Port
            /// </summary>
            public static int Port => Convert.ToInt32(_config["Email:Port"]);

            /// <summary>
            /// UseSsl
            /// </summary>
            public static bool UseSsl => Convert.ToBoolean(_config["Email:UseSsl"]);

            /// <summary>
            /// From
            /// </summary>
            public static class From
            {
                /// <summary>
                /// Username
                /// </summary>
                public static string Username => _config["Email:From:Username"];

                /// <summary>
                /// Password
                /// </summary>
                public static string Password => _config["Email:From:Password"];

                /// <summary>
                /// Name
                /// </summary>
                public static string Name => _config["Email:From:Name"];

                /// <summary>
                /// Address
                /// </summary>
                public static string Address => _config["Email:From:Address"];
            }

            /// <summary>
            /// To
            /// </summary>
            public static IDictionary<string, string> To
            {
                get
                {
                    var dic = new Dictionary<string, string>();

                    var emails = _config.GetSection("Email:To");
                    foreach (IConfigurationSection section in emails.GetChildren())
                    {
                        var name = section["Name"];
                        var address = section["Address"];

                        dic.Add(name, address);
                    }
                    return dic;
                }
            }
        }
    }
}
