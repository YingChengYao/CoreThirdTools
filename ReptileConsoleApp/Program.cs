using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PuppeteerSharp;
using ReptileConsoleApp.Services;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReptileConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            #region AngleSharp,HtmlAgilityPack
            //IServiceCollection service = new ServiceCollection();

            ////service.AddSingleton<HotNewsHtmlAgilityPack>();
            //service.AddSingleton<HotNewsAngleSharp>();

            ////var provider = service.BuildServiceProvider().GetRequiredService<HotNewsHtmlAgilityPack>();
            //var provider = service.BuildServiceProvider().GetRequiredService<HotNewsAngleSharp>();

            //var list = await provider.GetHotNewsAsync();

            //if (list.Any())
            //{
            //    Console.WriteLine($"一共{list.Count}条数据");

            //    foreach (var item in list)
            //    {
            //        Console.WriteLine($"{item.Title}\t{item.Url}");
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("无数据");
            //}
            //Console.ReadLine();
            #endregion

            #region PuppeteerSharp
            // 下载浏览器执行程序
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);

            // 创建一个浏览器执行实例
            using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                Args = new string[] { "--no-sandbox" }
            });

            // 打开一个页面
            using var page = await browser.NewPageAsync();

            // 设置页面大小
            await page.SetViewportAsync(new ViewPortOptions
            {
                Width = 1920,
                Height = 1080
            });

            #region html文本转换成pdf例子
            var inputFile = Path.Combine(Directory.GetCurrentDirectory(), $"json.txt");

            string html = "";
            using (FileStream fsRead = new FileStream(inputFile, FileMode.Open))
            {
                int fsLen = (int)fsRead.Length;
                byte[] heByte = new byte[fsLen];
                int r = fsRead.Read(heByte, 0, heByte.Length);
                html = System.Text.Encoding.UTF8.GetString(heByte);
            }
            
            html = html.Replace("\\n", "");
            html = html.Replace("\\t", "").Trim();
            html = html.Replace("\\\"", "\"").Trim();
            if (html.StartsWith('"') && html.EndsWith('"'))
            {
                html = html.TrimStart('"').TrimEnd('"');
            }

            var outputFile = Path.Combine(Directory.GetCurrentDirectory(), $"json.pdf");

            await page.SetContentAsync(html);
            var result = await page.GetContentAsync();
            await page.PdfAsync(outputFile);
            #endregion

            //var url = "https://juejin.im";
            //await page.GoToAsync(url, WaitUntilNavigation.Networkidle0);

            //await page.ScreenshotAsync("juejin.png");
            //await page.PdfAsync("juejin.pdf");

            //var content = await page.GetContentAsync();
            //Console.WriteLine(content);
            Console.WriteLine("end");
            Console.ReadLine();
            #endregion
        }
    }
}
