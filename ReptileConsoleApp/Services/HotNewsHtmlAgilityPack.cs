using HtmlAgilityPack;
using ReptileConsoleApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReptileConsoleApp.Services
{
    public class HotNewsHtmlAgilityPack
    {
        public async Task<IList<HotNews>> GetHotNewsAsync()
        {
            var list = new List<HotNews>();

            var web = new HtmlWeb();

            var htmlDocument = await web.LoadFromWebAsync("https://www.cnblogs.com/");

            var node = htmlDocument.DocumentNode.SelectNodes("//*[@id='post_list']/article/section/div/a").ToList();

            foreach (var item in node)
            {
                list.Add(new HotNews
                {
                    Title = item.InnerText,
                    Url = item.GetAttributeValue("href", "")
                });
            }

            return list;
        }
    }
}
