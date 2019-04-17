using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class QuotationModels
    {
        public List<MaterialModels> MaterialDto { get; set; }
        public List<AssemblyModels> AssemblyDto { get; set; }
        public List<BomModels> BomDto { get; set; }
    }
}