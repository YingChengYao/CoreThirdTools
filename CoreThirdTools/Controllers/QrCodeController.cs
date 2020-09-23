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
        [HttpGet]
        public FileContentResult QrCode([FromServices] QRCode _qrcode, string content)
        {
            var buffer = _qrcode.GenerateQRCode(content);

            return File(buffer, "image/jpeg");
        }
    }
}
