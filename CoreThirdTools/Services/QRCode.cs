using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace CoreThirdTools.Services
{
    public class QRCode
    {
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="pixel">像素大小默认10</param>
        /// <param name="level">设置纠错级别默认为M</param>
        /// <param name="drawQuietZones">静止区，位于二维码某一边的空白边界,用来阻止读者获取与正在浏览的二维码无关的信息 即是否绘画二维码的空白边框区域 默认为true</param>
        /// <returns></returns>
        public byte[] GenerateQRCode(string content, int pixel = 10, string level = "M", bool drawQuietZones = true)
        {
            var generator = new QRCodeGenerator();

            QRCodeGenerator.ECCLevel eccLevel = (QRCodeGenerator.ECCLevel)(level == "L" ? 0 : level == "M" ? 1 : level == "Q" ? 2 : 3);

            var codeData = generator.CreateQrCode(content, eccLevel, true);
            QRCoder.QRCode qrcode = new QRCoder.QRCode(codeData);

            #region 参数介绍
            //GetGraphic方法参数介绍
            //pixelsPerModule //生成二维码图片的像素大小 ，我这里设置的是5
            //darkColor       //暗色   一般设置为Color.Black 黑色
            //lightColor      //亮色   一般设置为Color.White  白色
            //icon             //二维码 水印图标 例如：Bitmap icon = new Bitmap(context.Server.MapPath("~/images/zs.png")); 默认为NULL ，加上这个二维码中间会显示一个图标
            //iconSizePercent  //水印图标的大小比例 ，可根据自己的喜好设置
            //iconBorderWidth  // 水印图标的边框
            //drawQuietZones   //静止区，位于二维码某一边的空白边界,用来阻止读者获取与正在浏览的二维码无关的信息 即是否绘画二维码的空白边框区域 默认为true
            #endregion
            var bitmapImg = qrcode.GetGraphic(pixel, Color.Black, Color.White, drawQuietZones);

            using MemoryStream stream = new MemoryStream();
            bitmapImg.Save(stream, ImageFormat.Jpeg);
            return stream.GetBuffer();
        }

        /// <summary>
        /// 生成带Logo的二维码
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="pixel">像素大小默认10</param>
        /// <param name="level">设置纠错级别默认为M</param>
        /// <param name="drawQuietZones">静止区，位于二维码某一边的空白边界,用来阻止读者获取与正在浏览的二维码无关的信息 即是否绘画二维码的空白边框区域 默认为true</param>
        /// <returns></returns>
        public byte[] GenerateQRCodeWithLogo(string content, int iconSize, string iconPath, int pixel = 10, string level = "M", bool drawQuietZones = true)
        {
            var generator = new QRCodeGenerator();

            QRCodeGenerator.ECCLevel eccLevel = (QRCodeGenerator.ECCLevel)(level == "L" ? 0 : level == "M" ? 1 : level == "Q" ? 2 : 3);

            var codeData = generator.CreateQrCode(content, eccLevel, drawQuietZones);
            QRCoder.QRCode qrcode = new QRCoder.QRCode(codeData);

            #region 参数介绍
            //GetGraphic方法参数介绍
            //pixelsPerModule //生成二维码图片的像素大小 ，我这里设置的是5
            //darkColor       //暗色   一般设置为Color.Black 黑色
            //lightColor      //亮色   一般设置为Color.White  白色
            //icon             //二维码 水印图标 例如：Bitmap icon = new Bitmap(context.Server.MapPath("~/images/zs.png")); 默认为NULL ，加上这个二维码中间会显示一个图标
            //iconSizePercent  //水印图标的大小比例 ，可根据自己的喜好设置
            //iconBorderWidth  // 水印图标的边框
            //drawQuietZones   //静止区，位于二维码某一边的空白边界,用来阻止读者获取与正在浏览的二维码无关的信息 即是否绘画二维码的空白边框区域 默认为true
            #endregion
            var bitmapImg = qrcode.GetGraphic(pixel, Color.Black, Color.White, GetIconBitmap(iconPath), iconSize);

            using MemoryStream stream = new MemoryStream();
            bitmapImg.Save(stream, ImageFormat.Jpeg);
            return stream.GetBuffer();
        }

        private Bitmap GetIconBitmap(string iconPath)
        {
            Bitmap img = null;
            if (iconPath.Length > 0)
            {
                try
                {
                    img = new Bitmap(iconPath);
                }
                catch (Exception ex)
                {
                    //throw ex;
                }
            }
            return img;
        }
    }
}
