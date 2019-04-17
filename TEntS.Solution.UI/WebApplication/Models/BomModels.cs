using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication.Models
					{
					public class BomModels
										{
										public BomModels() { }
										public int Id { get; set; }
										[Required(ErrorMessage = "Code is a required field.")]
										public string Code { get; set; }
										[Required(ErrorMessage = "Markup is a required field.")]
										public MarkupModel Markup { get; set; }
										public string MarkupCode { get; set; }
										public List<PoleModels> PoleList { get; set; }
										public bool IsActive { get; set; }
										public int TotalIo { get; set; }
										public ProjectInfoModel ProjectInfo { get; set; }

										public Guid ControlNumber { get; set; }
										public string Name { get; set; }
										public string Location { get; set; }
										public string Owner { get; set; }
										public string Description { get; set; }
										public bool IsApproved { get; set; }
										public DateTime DateCreated { get; set; }
										}
					}