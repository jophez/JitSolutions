using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TEntS.Types.Pole;
using TEntS.Types.ProjectInfo;

namespace TEntS.Types.BOM
     {
     public class BomTypes
          {
          public BomTypes() { }
          public int Id { get; set; }
          public string Code { get; set; }
          public bool IsActive { get; set; }
          public DateTime DateCreated { get; set; }
          public Markups.MarkupTypes Markup { get; set; }
          public List<PoleTypes> PoleList { get; set; }
										public ProjectInfoType ProjectInfo { get; set; }
          }
     }
