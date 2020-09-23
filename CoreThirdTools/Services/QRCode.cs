using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreThirdTools.Services
{
    public class QRCode
    {
        public byte[] GenerateQRCode(string content)
        {
            var generator = new QRCodeGenerator();

            var codeData = generator.CreateQrCode(content, QRCodeGenerator.ECCLevel.M, true);
            QRCoder.QRCode qrcode = new QRCoder.QRCode(codeData);

            var bitmapImg = qrcode.GetGraphic(10, Color.Black, Color.White, false);

            using MemoryStream stream = new MemoryStream();
            bitmapImg.Save(stream, ImageFormat.Jpeg);
            return stream.GetBuffer();
        }
    }
}
