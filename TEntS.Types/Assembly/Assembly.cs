using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TEntS.Types.Materials;

namespace TEntS.Types.Assembly
     {
     public class Assembly
          {
          public int Id { get; set; }
          public string Name { get; set; }
          public string Classification { get; set; }
          public bool? IsActive { get; set; }
          public List<MaterialForAssembly> Materials { get; set; }
          public int Quantity { get; set; }
          public decimal? UnitPrice { get; set; }
										public string Code { get; set; }

										public Guid RelBomCode { get; set; }
          }
     }
