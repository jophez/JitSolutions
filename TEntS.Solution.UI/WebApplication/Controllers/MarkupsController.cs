using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TEntS.ClientBLL.ClientBLL;
using TEntS.ClientBLL.Interfaces;
using TEntS.Types.Markups;
using WebApplication.Models;

namespace WebApplication.Controllers
					{
					public class MarkupsController : Controller
										{

										private readonly IMarkups _markup;
										private readonly IBillOfMaterials _bom;
										public MarkupsController()
															{
															_markup = new Markups(new MarkUpsBLL());
															_bom = new BillOfMaterials(new BillOfMaterialsBLL());

															}
										// GET: Markups
										public ActionResult Index()
															{
															var markupList = _markup.RetrieveAllActiveMarkups().OrderBy(m => m.Code).ToList().Select(mm => new SelectListItem
																				{ Value = mm.Id.ToString(), Text = mm.Code }).ToList();
															ViewBag.MarkupList = markupList;

															var bomList = _bom.RetrieveAllActiveBomItems().OrderBy(b => b.Code).ToList().Select(bb => new SelectListItem
																				{ Value = bb.Id.ToString(), Text = bb.ProjectInfo.Name }).ToList();
															ViewBag.BomList = bomList;

															ViewBag.Message = "";
															return View();
															}

										// GET: Markups/Details/5
										public ActionResult Details(int id)
															{
															return View();
															}

										// GET: Markups/Create
										public ActionResult Create()
															{
															return View();
															}

										// POST: Markups/Create
										[HttpPost]
										public ActionResult Create(MarkupModel collection)
															{
															try
																				{

																				if (!ModelState.IsValid)
																									{
																									ModelState.AddModelError("*", "An unexpected error occurred.");
																									return RedirectToAction("Index");
																									}
																				// TODO: Add insert logic here
																				var userDetail = new TEntS.Types.UserDetails
																									{
																									Email = User.Identity.Name
																									};
																				var markupObject = new MarkupTypes
																									{
																									Administrative = float.Parse(collection.Administrative.ToString())
																								,
																									Code = collection.Code
																								,
																									Contingency = float.Parse(collection.Contingency.ToString())
																								,
																									DirectLabor = float.Parse(collection.DirectLabor.ToString())
																								,
																									Equipment = float.Parse(collection.Equipment.ToString())
																								,
																									Marketing = float.Parse(collection.Marketing.ToString())
																								,
																									Markup = float.Parse(collection.Markup.ToString())
																								,
																									Representation = float.Parse(collection.Representation.ToString())
																								,
																									Vat_Ewt = float.Parse(collection.Vat_Ewt.ToString())
																									,
																									Generic_1 = collection.Generic1 != null ? collection.Generic1 : null
																									,
																									Generic_2 = collection.Generic2 != null ? collection.Generic2 : null
																									,
																									Generic_3 = collection.Generic3 != null ? collection.Generic3 : null
																									};

																				_markup.Create(markupObject, userDetail);

																				ViewBag.Message = "Markup created successfully !";

																				return RedirectToAction("Index");

																				}
															catch
																				{
																				return RedirectToAction("Index");
																				}

															}

										// GET: Markups/Edit/5
										public ActionResult Edit(string markupCode)
															{
															var thisMarkup = _markup.RetrieveAllMarkups().Where(m => m.Code.Equals(markupCode, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
															var model = new MarkupModel
																				{
																				Id = thisMarkup.Id,
																				Administrative = thisMarkup.Administrative
																				,
																				Code = thisMarkup.Code
																				,
																				Contingency = thisMarkup.Contingency
																				,
																				DirectLabor = thisMarkup.DirectLabor
																				,
																				Equipment = thisMarkup.Equipment
																				,
																				Marketing = thisMarkup.Marketing
																				,
																				Markup = thisMarkup.Markup
																				,
																				Representation = thisMarkup.Representation
																				,
																				Vat_Ewt = thisMarkup.Vat_Ewt
																				,
																				Generic1 = thisMarkup.Generic_1
																				,
																				Generic2 = thisMarkup.Generic_2
																				,
																				Generic3 = thisMarkup.Generic_3
																				};
															return View(model);
															}

										// POST: Markups/Edit/5
										[HttpPost]
										public ActionResult Edit(MarkupModel collection)
															{
															try
																				{
																				// TODO: Add update logic here
																				if (!ModelState.IsValid)
																									{
																									ModelState.AddModelError("*", "An unexpected error occurred.");
																									return View(collection);
																									}

																				var userDetail = new TEntS.Types.UserDetails
																									{
																									Email = User.Identity.Name
																									};

																				//var markupType = new MarkupTypes {

																				//}

																				//_markup.Update()
																				ViewBag.Message = "Record successfully updated.";

																				return RedirectToAction("Index");
																				}
															catch
																				{
																				return View();
																				}
															}

										// GET: Markups/Delete/5
										public ActionResult Delete(string markupCode)
															{
															try
																				{
																				// TODO: Add delete logic here
																				var markup = _markup.RetrieveAllActiveMarkups().Where(m => m.Code.Equals(markupCode, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault().Id;
																				var userDetail = new TEntS.Types.UserDetails
																									{
																									Email = User.Identity.Name
																									};
																				_markup.Retire(markup, userDetail);
																				return RedirectToAction("Index");
																				}
															catch
																				{
																				return View();
																				}
															}

										// POST: Markups/Delete/5
										[HttpPost]
										[ValidateAntiForgeryToken]
										public ActionResult AddMarkupToBom(string bomListId, string newMarkupId)
															{
															try
																				{
																				// TODO: Add delete logic here
																				var bom = _bom.RetrieveAllActiveBomItems().Where(b => b.Id.ToString().Equals(bomListId, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
																				var markup = _markup.RetrieveAllActiveMarkups().Where(m => m.Id.ToString().Equals(newMarkupId, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

																				var userDetail = new TEntS.Types.UserDetails
																									{
																									Email = User.Identity.Name
																									};
																				_bom.AssignMarkupToBom(bom.Id, markup.Id, userDetail);
																				PopulateDropDowns();
																				ViewBag.Message = "Record successfully created!.";
																				}
															catch
																				{
																				ModelState.AddModelError("Markup", "Invalid Markup selected.");
																				}
															return View("Index");

															}

										[HttpPost]
										[ValidateAntiForgeryToken]
										public ActionResult GetMarkups(string bomListId)
															{
															try
																				{
																				if (!string.IsNullOrWhiteSpace(bomListId))
																									{
																									var bom = _bom.RetrieveAllActiveBomItems().Where(b => b.Id.ToString().Equals(bomListId, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
																									ViewBag.MarkupsForThisBom = _bom.GetMarkups(bom.Id);

																									PopulateDropDowns();
																									ViewBag.Message = "Markups retrieved successfully !";
																									}
																				}
															catch
																				{
																				ModelState.AddModelError("Markup", "Invalid Markup selected.");
																				}
															return View("Index");
															}

										public ActionResult RemoveMarkupFromBom(string markupListForRemove, string bomListId)
															{
															try
																				{
																				var bom = _bom.RetrieveAllActiveBomItems().Where(b => b.Id.ToString().Equals(bomListId, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
																				var markup = _markup.RetrieveAllActiveMarkups().Where(m => m.Id.ToString().Equals(markupListForRemove, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
																				if (bom.Markup.Id == markup.Id)
																									{
																									var userDetail = new TEntS.Types.UserDetails
																														{
																														Email = User.Identity.Name
																														};

																									_bom.RemoveMarkupFromBom(bom.Id, markup.Id, userDetail);
																									}

																				PopulateDropDowns();

																				}
															catch
																				{
																				ModelState.AddModelError("Markup", "Invalid Markup selected");
																				}
															return View("Index");
															}

										#region PRIVATE METHODS
										private void PopulateDropDowns()
															{
															var markupList = _markup.RetrieveAllActiveMarkups().OrderBy(m => m.Code).ToList().Select(mm => new SelectListItem
																				{ Value = mm.Id.ToString(), Text = mm.Code }).ToList();
															ViewBag.MarkupList = markupList;

															var bomList = _bom.RetrieveAllActiveBomItems().OrderBy(b => b.Code).ToList().Select(bb => new SelectListItem
																				{ Value = bb.Id.ToString(), Text = bb.ProjectInfo.Name }).ToList();
															ViewBag.BomList = bomList;
															}
										#endregion
										}
					}
