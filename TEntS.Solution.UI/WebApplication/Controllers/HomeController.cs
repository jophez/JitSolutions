using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using TEntS.ClientBLL;
using TEntS.ClientBLL.ClientBLL;
using TEntS.ClientBLL.Interfaces;
using TEntS.Types.Assembly;
using TEntS.Types.Markups;
using TEntS.Types.Pole;
using TEntS.Types.ProjectInfo;
using WebApplication.Models;

namespace WebApplication.Controllers
					{
					public class HomeController : Controller
										{
										private int DayDiff = ConfigurationManager.AppSettings["DayDifference"] != null ? int.Parse(ConfigurationManager.AppSettings["DayDifference"].ToString()) : 7;
										private IMaterial _material;
										private IAssemblyBll _assembly;
										private IBillOfMaterials _bom;
										public HomeController()
															{
															_material = new Material(new MaterialBase());
															_assembly = new AssemblyBll(new AssemblyBase());
															_bom = new BillOfMaterials(new BillOfMaterialsBLL());
															}
										public ActionResult DashboardV1()
															{
															List<MaterialModels> mList = new List<Models.MaterialModels>();
															foreach (var material in Roles.IsUserInRole(User.Identity.Name, "Admin") ? _material.RetrieveAllMaterials() : _material.RetrieveAllActiveMaterials())
																				{
																				var dto = new MaterialModels
																									{
																									Id = material.Id,
																									Code = material.Code,
																									Description = material.Description,
																									DateCreated = material.CreationDate,
																									IsActive = Roles.IsUserInRole(User.Identity.Name, "Admin") ? material.IsActive : null,
																									UnitPrice = Roles.IsUserInRole(User.Identity.Name, "Admin") ? (double?)material.Cost.BasePrice : null
																									};

																				mList.Add(dto);
																				}

															List<AssemblyModels> aList = new List<AssemblyModels>();
															foreach (var assembly in Roles.IsUserInRole(User.Identity.Name, "Admin") ? _assembly.RetrieveAssemblyDetails() : _assembly.RetrieveActiveAssemblyDetails())
																				{
																				var dto = new AssemblyModels
																									{
																									Id = assembly.Id
																								,
																									Classification = assembly.Classification
																								,
																									Name = assembly.Name
																								,
																									UnitPrice = (decimal)assembly.UnitPrice
																								,
																									isActive = assembly.IsActive
																								,
																									Materials = MapMaterialItems(assembly.Materials)
																									};

																				aList.Add(dto);
																				}
															List<BomModels> bList = new List<BomModels>();
															foreach (var bom in Roles.IsUserInRole(User.Identity.Name, "Admin") ? _bom.RetrieveAllBomItems() : _bom.RetrieveAllActiveBomItems())
																				{
																				var dto = new BomModels
																									{
																									Code = bom.Code,
																									Id = bom.Id,
																									IsActive = bom.IsActive,
																									Markup = MapMarkupsToModel(bom.Markup),
																									PoleList = MapPoleListToModel(bom.PoleList),
																									ControlNumber = bom.ProjectInfo.ControlNumber,
																									Name = bom.ProjectInfo.Name,
																									Location = bom.ProjectInfo.Location,
																									Owner = bom.ProjectInfo.Owner,
																									Description = bom.ProjectInfo.Description,
																									IsApproved = bom.ProjectInfo.IsApproved,
																									DateCreated = bom.DateCreated
																									};
																				bList.Add(dto);
																				}
															var qModels = new QuotationModels() { MaterialDto = new List<MaterialModels>(), AssemblyDto = new List<AssemblyModels>(), BomDto = new List<BomModels>() };
															qModels.MaterialDto = mList;
															qModels.AssemblyDto = aList;
															qModels.BomDto = bList;
															ViewBag.TotalNotifications = mList.Count + aList.Count + bList.Count;
															ViewBag.NewMaterialCount = mList.FindAll(m => m.DateCreated >= DateTime.Now.AddDays(DayDiff)).Count;
															ViewBag.AssemblyCount = aList.Count;
															ViewBag.BomCount = bList.FindAll(b => b.DateCreated >= DateTime.Now.AddDays(DayDiff)).Count;
															return View(qModels);
															}
										public ActionResult DashboardV2()
															{
															return View();
															}

										#region
										private List<MaterialAssemblyModels> MapMaterialItems(List<TEntS.Types.Materials.MaterialForAssembly> materials)
															{
															var materialModelList = new List<MaterialAssemblyModels>();
															foreach (var material in materials)
																				{
																				var model = new MaterialAssemblyModels
																									{
																									Code = material.Code
																								,
																									Description = material.Description
																								,
																									Id = material.Id
																								,
																									ActualCost = material.Cost.ActualCost
																								,
																									BasePrice = material.Cost.BasePrice
																								,
																									Quantity = material.Quantity
																								,
																									Unit = SetDefaultUnit(material.Unit)
																									};

																				materialModelList.Add(model);
																				}

															return materialModelList;
															}

										private int SetDefaultUnit(string unit)
															{
															var defaultUnit = new UnitType
																				{
																				SelectedId = (int)Enum.Parse(typeof(DefaultUnits), unit.Length > 0 ? unit : "pc")
																				};

															return defaultUnit.SelectedId;
															}

										private MarkupModel MapMarkupsToModel(MarkupTypes markup)
															{
															return markup != null ? new MarkupModel
																				{
																				Administrative = markup.Administrative,
																				Code = markup.Code,
																				Contingency = markup.Contingency,
																				DirectLabor = markup.DirectLabor,
																				Equipment = markup.Equipment,
																				Generic1 = markup.Generic_1,
																				Generic2 = markup.Generic_2,
																				Generic3 = markup.Generic_3,
																				Marketing = markup.Marketing,
																				Markup = markup.Markup,
																				Representation = markup.Representation,
																				Vat_Ewt = markup.Vat_Ewt,
																				Id = markup.Id
																				} : new MarkupModel();
															}

										private List<PoleModels> MapPoleListToModel(List<PoleTypes> poleList)
															{
															var list = new List<PoleModels>();
															foreach (var pole in poleList)
																				{
																				var item = new PoleModels
																									{
																									RelBomCode = pole.RelBomCode,
																									Number = pole.Number,
																									BSpan = pole.BSpan,
																									Code = pole.Code,
																									Wires = pole.Wires,
																									//AssemblyList = MapAssemblyListToModel(pole.AssemblyList, pole.Number, pole.Code),
																									AssemblyListToSave = new List<Models.PoleAssembly>()
																									};
																				list.Add(item);
																				}

															return list;
															}

										private List<AssemblyModels> MapAssemblyListToModel(List<Assembly> assemblyList, string poleNumber, string code)
															{
															var list = new List<AssemblyModels>();
															foreach (var item in assemblyList)
																				{
																				var assembly = new AssemblyModels
																									{
																									Code = code,
																									PoleNumber = poleNumber,
																									Classification = item.Classification,
																									Id = item.Id,
																									Name = item.Name,
																									Quantity = item.Quantity
																									};
																				list.Add(assembly);
																				}

															return list;
															}
										#endregion
										}
					}