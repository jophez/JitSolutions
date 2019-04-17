using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TEntS.Types.Pole
     {
     public class PoleTypes
          {
          public int Id { get; set; }
          public string Number { get; set; }
          public string Code { get; set; }
          public int BSpan { get; set; }
          public int Wires { get; set; }
          public string RelBomCode { get; set; }
										//public int RelAssemblyId { get; set; }
          public List<Assembly.Assembly> AssemblyList { get; set; }
          public List<PoleAssembly> PoleAssemblyList { get; set; }
          }
     }
