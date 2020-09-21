using CoreThirdTools.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreThirdTools.Controllers
{
    [ApiController]
    public class CaptchaController : ControllerBase
    {
        [HttpGet]
        [Route("captcha")]
        public async Task<FileContentResult> CaptchaAsync([FromServices] Captcha _captcha)
        {
            var code = await _captcha.GenerateRandomCaptchaAsync();

            var result = await _captcha.GenerateCaptchaImageAsync(code);

            return File(result.CaptchaMemoryStream.ToArray(), "image/png");
        }
    }
}
