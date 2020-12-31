using CoreThirdTools.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreThirdTools.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QrCodeController : ControllerBase
    {
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="_qrcode"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GenerateQRCode")]
        public FileContentResult QrCode([FromServices] QRCode _qrcode, string content, int pixel, bool drawQuietZones)
        {
            var buffer = _qrcode.GenerateQRCode(content, pixel, drawQuietZones: drawQuietZones);

            return File(buffer, "image/jpeg");
        }

        /// <summary>
        /// 生成带logo二维码
        /// </summary>
        /// <param name="_qrcode"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GenerateQRCodeWithLogo")]
        public FileContentResult QrCode([FromServices] QRCode _qrcode, string content, int iconSize, string iconPath, string level)
        {
            var buffer = _qrcode.GenerateQRCodeWithLogo(content, iconSize, iconPath, level: level);

            return File(buffer, "image/jpeg");
        }
    }
}
