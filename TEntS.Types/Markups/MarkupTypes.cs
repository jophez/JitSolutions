using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TEntS.Types.Markups
    {
   public class MarkupTypes
        {
         public int Id { get; set; }
         public string Code { get; set; }
         public bool isActive { get; set; }
         public float Markup { get; set; }
         public float Vat_Ewt { get; set; }
         public float Administrative { get; set; }
         public float Equipment { get; set; }
         public float Marketing { get; set; }
         public float Contingency { get; set; }
         public float Representation { get; set; }
         public float DirectLabor { get; set; }
         public float? Generic_1 { get; set; }
         public float? Generic_2 { get; set; }
         public float? Generic_3 { get; set; }
        }
    }
