using System.Collections.Generic;

namespace WebApplication.Models
     {
     public class PoleModels
          {
          public int Id { get; set; }
          public string Number { get; set; }
          public string Code { get; set; }
          public int BSpan { get; set; }
          public int Wires { get; set; }
          public string RelBomCode { get; set; }
										//public int RelAssemblyId { get; set; }
          public List<PoleAssembly> AssemblyListToSave { get; set; }
          public List<AssemblyModels> AssemblyList { get; set; }
          }
     }