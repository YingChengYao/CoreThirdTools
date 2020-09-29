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
    public class Appsettings
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

        static Appsettings()
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
    }
}
