using AngleSharp;
using ReptileConsoleApp.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReptileConsoleApp.Services
{
    public class HotNewsAngleSharp
    {
        public async Task<IList<HotNews>> GetHotNewsAsync()
        {
            var list = new List<HotNews>();

            var config = Configuration.Default.WithDefaultLoader();
            var address = "https://www.cnblogs.com";
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(address);

            var cellSelector = "article.post-item";
            var cells = document.QuerySelectorAll(cellSelector);

            foreach (var item in cells)
            {
                var a = item.QuerySelector("section>div>a");
                list.Add(new HotNews
                {
                    Title = a.TextContent,
                    Url = a.GetAttribute("href")
                });
            }

            return list;
        }
    }
}
