using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreThirdTools.Models
{
    public class ImportExcelInput
    {
        public IFormFile ExcelFile { get; set; }
    }
}
