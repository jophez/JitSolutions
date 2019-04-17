using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Models
     {
     public class MaterialAssemblyModels
          {
          public MaterialAssemblyModels()
               {

               }

          public int Id { get; set; }

          public string Code { get; set; }

          public string Description { get; set; }

          public bool? IsActive { get; set; }

          public int Unit { get; set; }
          public int Quantity { get; set; }
          public double BasePrice { get; set; }
          public double ActualCost { get; set; }
          public bool isChecked { get; set; }
          }
     }