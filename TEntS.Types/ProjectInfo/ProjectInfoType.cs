using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TEntS.Types.ProjectInfo
					{
					public class ProjectInfoType
										{
										public Guid ControlNumber { get; set; }
										public string Name { get; set; }
										public string Location { get; set; }
										public string Owner { get; set; }
										public string Description { get; set; }
										public bool IsApproved { get; set; }
										}
					}
