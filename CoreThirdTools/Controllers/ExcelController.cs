using CoreThirdTools.Models;
using CoreThirdTools.Utils;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreThirdTools.Controllers
{
    [ApiController]
    public class ExcelController : ControllerBase
    {
        #region 基于epplus
        [HttpPost]
        [Route("import")]
        public List<ExcelDemoDto> Import([FromForm] ImportExcelInput input)
        {
            var list = new List<ExcelDemoDto>();

            using (var package = new ExcelPackage(input.ExcelFile.OpenReadStream()))
            {
                // 获取到第一个Sheet，也可以通过 Worksheets["name"] 获取指定的工作表
                var sheet = package.Workbook.Worksheets.First();

                #region 获取开始和结束行列的个数，根据个数可以做各种校验工作

                // +1 是因为第一行往往我们获取到的都是Excel的标题
                int startRowNumber = sheet.Dimension.Start.Row + 1;
                int endRowNumber = sheet.Dimension.End.Row;
                int startColumn = sheet.Dimension.Start.Column;
                int endColumn = sheet.Dimension.End.Column;

                #endregion

                // 循环获取整个Excel数据表数据
                for (int currentRow = startRowNumber; currentRow <= endRowNumber; currentRow++)
                {
                    list.Add(new ExcelDemoDto
                    {
                        AAA = sheet.Cells[currentRow, 1].Text,
                        BBB = sheet.Cells[currentRow, 2].Text,
                        CCC = sheet.Cells[currentRow, 3].Text,
                        DDD = sheet.Cells[currentRow, 4].Text,
                        EEE = sheet.Cells[currentRow, 5].Text,
                        FFF = sheet.Cells[currentRow, 6].Text
                    });
                }
            }

            return list;
        }

        [HttpGet]
        [Route("export")]
        public async Task<string> Export()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("sheet1");

            //工作簿进行计算，通常Excel会自动进行计算，但如果你打开工作簿的机器上没有计算引擎，那么这行代码就发挥了作用
            //worksheet.Calculate();

            var headers = new string[] { "AAA", "BBB", "CCC", "DDD", "EEE", "FFF" };
            for (int i = 0; i < headers.Length; i++)
            {
                //worksheet.Cells[1, i + 1].Value = headers[i];
                worksheet.SetValue(1, i + 1, headers[i]);//这种赋值方法比上面的性能好一些
                worksheet.Cells[1, i + 1].Style.Font.Bold = true;//设置单元格字体加粗
                                                                 //设置单元格上边框，同理，右、下、左也一样的设置即可
                worksheet.Cells[1, i + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            }

            ////设置第二行第三列到第五行第三例的数据格式为整数
            //worksheet.Cells["C2:C5"].Style.Numberformat.Format = "#,##0";
            ////设置第二行第四列到第五行第五列的数据格式为保留小数点后两位
            //worksheet.Cells["D2:E5"].Style.Numberformat.Format = "#,##0.00";
            ////设置第二行第一列到第四行第一列的数据格式为文本格式
            //worksheet.Cells["A2:A4"].Style.Numberformat.Format = "@";
            //worksheet.Cells.AutoFitColumns(0);  //所有单元格的列都自适应
            ////第一行第一列到第四行第五列的数据设置筛选器
            //worksheet.Cells["A1:E4"].AutoFilter = true;

            //获取一个区域，区域范围是第一行第一列到第一行第五列
            //using (var range = worksheet.Cells[1, 1, 1, 5]) 
            //{
            //    range.Style.Font.Bold = true;
            //    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            //    range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            //    range.Style.Font.Color.SetColor(Color.White);
            //}

            // 模拟数据
            var list = new List<ExcelDemoDto>();
            for (int i = 1; i <= 10; i++)
            {
                list.Add(new ExcelDemoDto
                {
                    AAA = $"A{i}",
                    BBB = $"B{i}",
                    CCC = $"C{i}",
                    DDD = $"D{i}",
                    EEE = $"E{i}",
                    FFF = $"F{i}"
                });
            }

            // 支持各种直接获取数据的方法
            // worksheet.Cells.Load*...

            int row = 2;
            foreach (var item in list)
            {
                worksheet.Cells[row, 1].Value = item.AAA;
                worksheet.Cells[row, 2].Value = item.BBB;
                worksheet.Cells[row, 3].Value = item.CCC;
                worksheet.Cells[row, 4].Value = item.DDD;
                worksheet.Cells[row, 5].Value = item.EEE;
                worksheet.Cells[row, 6].Value = item.FFF;

                row++;
            }

            // 通常做法是，将excel上传至对象存储，获取到下载链接，这里将其输出到项目根目录。
            var path = Path.Combine(Directory.GetCurrentDirectory(), $"excel.xlsx");
            await package.GetAsByteArray().DownloadAsync(path);
            return path;
        }

        #endregion

    }
}
