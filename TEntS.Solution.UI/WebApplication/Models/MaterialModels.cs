using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Models
{
    public class MaterialModels
    {
        public MaterialModels() { }

        public int Id { get; set; }
        public bool IsSelected { get; set; }
        [Required(ErrorMessage = "Code is a required field")]
        [RegularExpression(@"^[M]\d{1}-\d{3}$", ErrorMessage = "Material code is not in the correct format.")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Description is a required field")]
        public string Description { get; set; }
        [Display(Name ="Unit Price")]
        public double? UnitPrice { get; set; }
        public DateTime DateCreated { get; set; }
        public bool? IsActive { get; set; }

        public string Unit { get; set; }
        public int Quantity { get; set; } 
    }
}