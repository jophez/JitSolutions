using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
					{
					public class ProjectInfoModel
										{
										public Guid ControlNumber { get; set; }
										public string Name { get; set; }
										public string Location { get; set; }
										public string Owner { get; set; }
										public string Description { get; set; }
										public bool IsApproved { get; set; }
										}
					}