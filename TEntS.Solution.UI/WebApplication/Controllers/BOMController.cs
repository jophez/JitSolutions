using Newtonsoft.Json;
using Spire.Xls;
using Spire.Xls.Collections;
using Spire.Xls.Core;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using TEntS.ClientBLL.ClientBLL;
using TEntS.ClientBLL.Interfaces;
using TEntS.Types.Assembly;
using TEntS.Types.BOM;
using TEntS.Types.Markups;
using TEntS.Types.Pole;
using WebApplication.Models;
using System;
using TEntS.Types.ProjectInfo;

namespace WebApplication.Controllers
					{
					public class BOMController : Controller
										{
										private IBillOfMaterials _iBom;
										private IAssemblyBll _iAssembly;
										private IMarkups _markup;
										public BOMController()
															{
															_iAssembly = new AssemblyBll(new AssemblyBase());
															_iBom = new BillOfMaterials(new BillOfMaterialsBLL());
															_markup = new Markups(new MarkUpsBLL());
															}

										// GET: BOM
										public ActionResult Index()
															{
															List<BomModels> list = new List<BomModels>();
															var markupList = _markup.RetrieveAllActiveMarkups().OrderBy(m => m.Code).ToList().Select(mm => new SelectListItem
																				{ Value = mm.Id.ToString(), Text = mm.Code }).ToList();
															ViewBag.MarkupList = markupList;
															int totalIo = 0;
															foreach (var bomItem in Roles.IsUserInRole(User.Identity.Name, "Admin") ? _iBom.RetrieveAllBomItems() : _iBom.RetrieveAllActiveBomItems())
																				{
																				var dto = new BomModels
																									{
																									Id = bomItem.Id,
																									Code = bomItem.Code,
																									Markup = MapMarkupsToModel(bomItem.Markup),
																									PoleList = MapPoleListToModel(bomItem.PoleList, ref totalIo),
																									TotalIo = totalIo,
																									IsActive = bomItem.IsActive
																									};
																				list.Add(dto);
																				}

															return PartialView(list);
															}

										//public ActionResult ExportToExcel(int bomId)
										//					{
										//					var bomList = Roles.IsUserInRole(User.Identity.Name, "Admin") ? _iBom.RetrieveAllBomItems() : _iBom.RetrieveAllActiveBomItems();
										//					var bomItem = bomList.Where(bom => bom.Id == bomId).FirstOrDefault();
										//					using (var workBook = new Workbook())
										//										{
										//										WorksheetsCollection workSheets = workBook.Worksheets;
										//										}
										//					}
										public JsonResult LoadBomListItems()
															{
															List<BomModels> list = new List<BomModels>();
															int totalIo = 0;
															foreach (var bom in Roles.IsUserInRole(User.Identity.Name, "Admin") ? _iBom.RetrieveAllBomItems() : _iBom.RetrieveAllActiveBomItems())
																				{
																				if (bom.Code != null)
																									{
																									var dto = new BomModels
																														{
																														Id = bom.Id,
																														Code = bom.Code,
																														IsActive = bom.IsActive,
																														MarkupCode = MapMarkupsToModel(bom.Markup).Code,
																														PoleList = MapPoleListToModel(bom.PoleList, ref totalIo),
																														ControlNumber = bom.ProjectInfo.ControlNumber,
																														Name = bom.ProjectInfo.Name,
																														Location = bom.ProjectInfo.Location,
																														Owner = bom.ProjectInfo.Owner,
																														Description = bom.ProjectInfo.Description,
																														IsApproved = bom.ProjectInfo.IsApproved,
																														TotalIo = totalIo
																														};
																									list.Add(dto);
																									}
																				}

															var jsonResult = JsonConvert.SerializeObject(list.ToArray());
															return Json(jsonResult, JsonRequestBehavior.AllowGet);
															}
										private List<PoleModels> MapPoleListToModel(List<PoleTypes> poleList, ref int totalIo, int? bomId = null)
															{
															totalIo = 0;
															var list = new List<PoleModels>();
															foreach (var pole in poleList)
																				{
																				var item = new PoleModels
																									{
																									Number = pole.Number,
																									BSpan = pole.BSpan,
																									Code = pole.Code,
																									Wires = pole.Wires,
																									RelBomCode = pole.RelBomCode,
																									AssemblyList = bomId != null ? MapAssemblyListToModel(bomId.Value, pole.RelBomCode) : new List<AssemblyModels>(),
																									AssemblyListToSave = new List<Models.PoleAssembly>()
																									};
																				list.Add(item);
																				totalIo += item.BSpan * item.Wires;
																				}

															return list;
															}

										private List<AssemblyModels> MapAssemblyListToModel(int bomId, string controlNumber)
															{
															var list = new List<AssemblyModels>();
															var assemblyList = _iBom.RetrieveAllBomAssemblyDetails().Where(a => a.RelBomCode.ToString().Equals(controlNumber)).ToList();
															foreach (var item in assemblyList)
																				{
																				var assembly = new AssemblyModels
																									{
																									isChecked = bomId > 0,
																									Code = item.Code,
																									Classification = item.Classification,
																									Id = item.Id,
																									Name = item.Name,
																									Quantity = item.Quantity

																									};
																				list.Add(assembly);
																				}

															return list;
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

										public ActionResult TableAssemblyBOMResult()
															{
															var bomAssemblyModels = new List<AssemblyModels>();

															foreach (var assembly in Roles.IsUserInRole(User.Identity.Name, "Admin") ? _iAssembly.RetrieveAssemblyDetails() : _iAssembly.RetrieveActiveAssemblyDetails())
																				{

																				var assemblyDetails = new AssemblyModels
																									{
																									Id = assembly.Id
																								,
																									Name = assembly.Name
																								,
																									Classification = assembly.Classification
																								,
																									UnitPrice = (decimal)assembly.UnitPrice
																									};

																				bomAssemblyModels.Add(assemblyDetails);

																				}

															var jsonResult = JsonConvert.SerializeObject(bomAssemblyModels);

															return Json(jsonResult, JsonRequestBehavior.AllowGet);
															}
										// GET: BOM/Details/5
										public ActionResult Details(int id)
															{
															return View();
															}

										//GET : AssemblyList
										public PartialViewResult PoleAssemblyForEditPopup(string code)
															{
															var codeArray = code.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
															var bomAssemblyModels = new List<AssemblyModels>();
															foreach (var assembly in Roles.IsUserInRole(User.Identity.Name, "Admin") ? _iAssembly.RetrieveAssemblyDetails() : _iAssembly.RetrieveActiveAssemblyDetails())
																				{
																				var item = _iBom.RetrieveAllBomAssemblyDetails().Where(a => a.RelBomCode.ToString().Equals(codeArray[0]) && a.Code.Equals(codeArray[1]) && a.Name.Equals(assembly.Name)).FirstOrDefault();
																				var assemblyDetails = new AssemblyModels
																									{
																									Id = assembly.Id,
																									Name = assembly.Name,
																									Classification = assembly.Classification,
																									UnitPrice = (decimal)assembly.UnitPrice,
																									isChecked = item != null ? true : false,
																									Quantity = item != null ? item.Quantity : 0,
																									TotalAmount = item != null ? item.Quantity * assembly.UnitPrice : Decimal.Parse("0"),
																									Code = code
																									};

																				bomAssemblyModels.Add(assemblyDetails);
																				}
															return PartialView("PoleAssemblyForEditPopup", bomAssemblyModels);
															}

										private AssemblyModels CovertAssemblyTypeToModel(Assembly assembly)
															{
															return new AssemblyModels
																				{
																				Code = assembly.Code,
																				Classification = assembly.Classification,
																				Name = assembly.Name,
																				Quantity = assembly.Quantity,
																				Id = assembly.Id,
																				isChecked = true
																				};
															}

										public PartialViewResult popupPoleAssembly()
															{
															var bomAssemblyModels = new List<AssemblyModels>();

															foreach (var assembly in Roles.IsUserInRole(User.Identity.Name, "Admin") ? _iAssembly.RetrieveAssemblyDetails() : _iAssembly.RetrieveActiveAssemblyDetails())
																				{

																				var assemblyDetails = new AssemblyModels
																									{
																									Id = assembly.Id
																								,
																									Name = assembly.Name
																								,
																									Classification = assembly.Classification
																								,
																									UnitPrice = (decimal)assembly.UnitPrice
																									};

																				bomAssemblyModels.Add(assemblyDetails);

																				}
															return PartialView("popupPoleAssembly", bomAssemblyModels);
															}
										// GET: BOM/Create
										public ActionResult Create()
															{
															List<BomModels> list = new List<BomModels>();
															var markupList = _markup.RetrieveAllActiveMarkups().OrderBy(m => m.Code).ToList().Select(mm => new SelectListItem
																				{ Value = mm.Id.ToString(), Text = mm.Code }).ToList();
															ViewBag.MarkupList = markupList;

															return PartialView("Create", new BomModels());
															}

										// POST: BOM/Create
										[HttpPost]
										public ActionResult Create(BomModels bomItem)
															{
															try
																				{
																				if (!ModelState.IsValid)
																									{
																									var errorMsg = new List<string>();
																									if (bomItem.Code == null)
																														errorMsg.Add("CODE is a required field");

																									var result = AddErrorMsg("Error", errorMsg);
																									return new JsonResult
																														{
																														JsonRequestBehavior = JsonRequestBehavior.AllowGet,
																														Data = result
																														};
																									}

																				BomTypes bomType = ConvertModelToType(bomItem);
																				var userDetail = new TEntS.Types.UserDetails
																									{
																									Email = User.Identity.Name
																									};

																				var isSaved = _iBom.Create(bomType, userDetail);
																				if (isSaved)
																									return RedirectToAction("LoadBomListItems");
																				else
																									throw new TEntS.Types.Exception.TEntSException("Record has not been saved.");
																				}
															catch (TEntS.Types.Exception.TEntSException ex)
																				{
																				var errorResult = AddErrorMsg("Error", new List<string> { ex.Message });
																				return new JsonResult
																									{
																									JsonRequestBehavior = JsonRequestBehavior.AllowGet,
																									Data = errorResult
																									};
																				}
															catch
																				{
																				return View();
																				}
															}

										private BomTypes ConvertModelToType(BomModels model)
															{
															return new BomTypes
																				{
																				Code = new Guid(model.Code).ToString(),
																				Markup = GetMarkupType(model.Markup),
																				PoleList = GetPoleType(model.PoleList),
																				ProjectInfo = GetProjectInfo(model.ProjectInfo)
																				};
															}

										private ProjectInfoType GetProjectInfo(ProjectInfoModel projectInfo)
															{
															return new ProjectInfoType
																				{
																				ControlNumber = new Guid(projectInfo.ControlNumber.ToString()),
																				Description = projectInfo.Description,
																				Location = projectInfo.Location,
																				Name = projectInfo.Name,
																				Owner = projectInfo.Owner,
																				IsApproved = false
																				};
															}

										private List<PoleTypes> GetPoleType(List<PoleModels> poleList)
															{
															var poleAssemblyList = poleList.Select(p => p.AssemblyListToSave).ToList();
															var list = new List<PoleTypes>();
															foreach (var model in poleList)
																				{
																				var pAssembly = new PoleTypes
																									{
																									RelBomCode = model.RelBomCode,
																									BSpan = model.BSpan,
																									Code = model.Code,
																									Number = model.Number,
																									Wires = model.Wires,
																									PoleAssemblyList = GetPoleAssembly(model.AssemblyListToSave, poleAssemblyList, model.RelBomCode)
																									};
																				list.Add(pAssembly);
																				}
															return list;
															}

										private List<TEntS.Types.Pole.PoleAssembly> GetPoleAssembly(List<Models.PoleAssembly> assemblyListToSave, List<List<Models.PoleAssembly>> poleAssemblyList, string relBomCode = null)
															{
															var list = new List<TEntS.Types.Pole.PoleAssembly>();
															var tmpList = new List<TEntS.Types.Pole.PoleAssembly>();
															foreach (var poleList in poleAssemblyList)
																				{
																				if (poleList != null)
																									{
																									foreach (Models.PoleAssembly poleItem in poleList)
																														{
																														var item = new TEntS.Types.Pole.PoleAssembly
																																			{
																																			AssemblyId = poleItem.AssemblyId,
																																			Code = poleItem.Code,
																																			PoleId = poleItem.Number,
																																			Quantity = poleItem.Quantity
																																			};
																														tmpList.Add(item);
																														}
																									}
																				}
															if (assemblyListToSave == null)
																				{
																				var tmpAssemblyListToSave = _iBom.RetrieveAllBomAssemblyDetails().Where(a => a.RelBomCode.ToString().Equals(relBomCode)).ToList();
																				foreach (var pole in tmpAssemblyListToSave)
																									{
																									if (tmpList.Where(a => a.Code.Equals(pole.Code) && a.AssemblyId == pole.Id).FirstOrDefault() != null)
																														{
																														var item = new TEntS.Types.Pole.PoleAssembly
																																			{
																																			AssemblyId = pole.Id,
																																			Code = pole.Code,
																																			Quantity = pole.Quantity
																																			};
																														list.Add(item);
																														}
																									else
																														continue;
																									}
																				}
															else
																				{
																				foreach (var model in assemblyListToSave)
																									{
																									if (poleAssemblyList.Select(a => a.Select(b => b.Code.Equals(model.Code))).FirstOrDefault() == null) { continue; }
																									var poleAssembly = new TEntS.Types.Pole.PoleAssembly
																														{
																														Code = model.Code,
																														PoleId = model.Number,
																														AssemblyId = model.AssemblyId,
																														Quantity = model.Quantity
																														};
																									list.Add(poleAssembly);
																									}
																				}
															return list;
															}

										private MarkupTypes GetMarkupType(MarkupModel markup)
															{
															return new MarkupTypes
																				{
																				Id = markup.Id,
																				Code = markup.Code
																				};
															}

										// GET: BOM/Edit/5
										public ActionResult Edit(string code)
															{
															BomModels model = new BomModels();
															try
																				{
																				var bomList = Roles.IsUserInRole(User.Identity.Name, "Admin") ? _iBom.RetrieveAllBomItems() : _iBom.RetrieveAllActiveBomItems();
																				model = ConvertTypeToModel(bomList.Where(bom => bom.Code.Equals(code)).FirstOrDefault());
																				var markupList = _markup.RetrieveAllActiveMarkups().OrderBy(m => m.Code).ToList().Select(mm => new SelectListItem
																									{ Value = mm.Id.ToString(), Text = mm.Code }).ToList();

																				//ViewBag.MarkupList = markupList;
																				markupList.Find(markup => markup.Text.Equals(model.Markup.Code)).Selected = true;
																				var markupListItem = markupList.Find(markup => markup.Text.Equals(model.Markup.Code));
																				ViewBag.MarkupList = markupList;
																				ViewBag.Markup = markupListItem;
																				}
															catch (Exception ex)
																				{
																				ModelState.AddModelError("EditError", ex.Message);
																				}
															return PartialView("Edit", model);
															}

										private BomModels ConvertTypeToModel(BomTypes bomTypes)
															{
															if (bomTypes == null) return new BomModels();
															int totalIo = 0;
															return new BomModels
																				{
																				Code = bomTypes.Code,
																				Id = bomTypes.Id,
																				ProjectInfo = new ProjectInfoModel
																									{
																									ControlNumber = bomTypes.ProjectInfo.ControlNumber,
																									Description = bomTypes.ProjectInfo.Description,
																									Location = bomTypes.ProjectInfo.Location,
																									IsApproved = bomTypes.ProjectInfo.IsApproved,
																									Name = bomTypes.ProjectInfo.Name,
																									Owner = bomTypes.ProjectInfo.Owner
																									},
																				Markup = MapMarkupsToModel(bomTypes.Markup),
																				PoleList = MapPoleListToModel(bomTypes.PoleList, ref totalIo, bomTypes.Id),
																				TotalIo = totalIo
																				};
															}

										// POST: BOM/Edit/5
										[HttpPost]
										public ActionResult Edit(BomModels bomItem)
															{
															try
																				{
																				var errors = ModelState
					.Where(x => x.Value.Errors.Count > 0)
					.Select(x => new { x.Key, x.Value.Errors })
					.ToArray();
																				if (!ModelState.IsValid)
																									{
																									var errorMsg = new List<string>();
																									if (bomItem.Code == null)
																														errorMsg.Add("CODE is a required field");

																									var result = AddErrorMsg("Error", errorMsg);
																									return new JsonResult
																														{
																														JsonRequestBehavior = JsonRequestBehavior.AllowGet,
																														Data = result
																														};
																									}
																				else
																									{
																									BomTypes bomType = ConvertModelToType(bomItem);
																									var userDetail = new TEntS.Types.UserDetails
																														{
																														Email = User.Identity.Name
																														};

																									var isSaved = _iBom.Update(bomType, userDetail);
																									if (isSaved)
																														{
																														return RedirectToAction("LoadBomListItems", "BOM");
																														}
																									else
																														throw new TEntS.Types.Exception.TEntSException("Record has not been saved.");
																									}
																				}
															catch (TEntS.Types.Exception.TEntSException ex)
																				{
																				var errorResult = AddErrorMsg("Error", new List<string> { ex.Message });
																				return new JsonResult
																									{
																									JsonRequestBehavior = JsonRequestBehavior.AllowGet,
																									Data = errorResult
																									};
																				}
															catch
																				{
																				return View();
																				}
															}

										// GET: BOM/Delete/5
										public ActionResult Delete(int id)
															{
															return View();
															}

										// POST: BOM/Delete/5
										[HttpPost]
										public ActionResult Delete(int id, FormCollection collection)
															{
															try
																				{
																				// TODO: Add delete logic here

																				return RedirectToAction("Index");
																				}
															catch
																				{
																				return View();
																				}
															}

										private Dictionary<string, List<string>> AddErrorMsg(string key, List<string> value)
															{
															var dictionaryItem = new Dictionary<string, List<string>>();
															dictionaryItem.Add(key, value);

															return dictionaryItem;
															}
										}
					}
