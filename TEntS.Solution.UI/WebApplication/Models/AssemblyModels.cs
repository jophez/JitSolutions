using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class AssemblyModels
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Classification { get; set; }
        public bool? isActive { get; set; }
        public int Quantity { get; set; }
        public List<MaterialAssemblyModels> Materials { get; set; }
        public List<MaterialModels> CompleteMaterialList { get; set; }
        public decimal UnitPrice { get; set; }
        public string PoleNumber { get; set; }
								public string Code { get; set; }
								public bool isChecked { get; set; }
								public decimal? TotalAmount { get; set; }
    }
}