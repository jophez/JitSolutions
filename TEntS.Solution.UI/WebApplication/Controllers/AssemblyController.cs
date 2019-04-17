using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using TEntS.ClientBLL.ClientBLL;
using TEntS.ClientBLL.Interfaces;
using WebApplication.Models;
using System;
using TEntS.Types.Materials;
using TEntS.ClientBLL;
using System.Linq;


namespace WebApplication.Controllers
     {
     public class AssemblyController : Controller
          {
          private IAssemblyBll _iAssembly;
          private IMaterial _iMaterial;
          public AssemblyController()
               {
               _iAssembly = new AssemblyBll(new AssemblyBase());
               _iMaterial = new TEntS.ClientBLL.ClientBLL.Material(new MaterialBase());
               }
          // GET: Assembly
          public ActionResult Index()
               {
               return PartialView();
               }

          public ActionResult TableAssemblyListResult()
               {
               List<AssemblyModels> list = new List<AssemblyModels>();
               foreach (var assemblyItem in Roles.IsUserInRole(User.Identity.Name, "Admin") ? _iAssembly.RetrieveAssemblyDetails() : _iAssembly.RetrieveActiveAssemblyDetails())
                    {
                    var dto = new AssemblyModels
                         {
                         Id = assemblyItem.Id
                        ,
                         Name = assemblyItem.Name
                        ,
                         Classification = assemblyItem.Classification
                        ,
                         isActive = assemblyItem.IsActive
                        ,
                         UnitPrice = (decimal)assemblyItem.UnitPrice
                         ,
                         Materials = MapMaterialItems(assemblyItem.Materials)
                         };

                    list.Add(dto);
                    }

               var jsonResult = JsonConvert.SerializeObject(list.ToArray());

               return Json(jsonResult, JsonRequestBehavior.AllowGet);
               }
          public ActionResult MaterialList(string assemblyName)
               {
               var materialList = _iAssembly.RetrieveAssemblyMaterialsByCode(assemblyName);
               var materialListDto = MapMaterialItems(materialList);
               return PartialView(materialListDto);
               }
          public ActionResult TableResult()
               {
               List<AssemblyModels> list = new List<AssemblyModels>();
               foreach (var assemblyItem in Roles.IsUserInRole(User.Identity.Name, "Admin") ? _iAssembly.RetrieveAssemblyDetails() : _iAssembly.RetrieveActiveAssemblyDetails())
                    {
                    var dto = new AssemblyModels
                         {
                         Id = assemblyItem.Id
                        ,
                         Name = assemblyItem.Name
                        ,
                         Classification = assemblyItem.Classification
                        ,
                         isActive = assemblyItem.IsActive
                        ,
                         Materials = MapMaterialItems(assemblyItem.Materials)
                        ,
                         UnitPrice = (decimal)assemblyItem.UnitPrice
                         };

                    list.Add(dto);
                    }

               var jsonResult = JsonConvert.SerializeObject(list.ToArray());

               return Json(jsonResult, JsonRequestBehavior.AllowGet);
               }

          public ActionResult TableMaterialAssemblyResult(int? id = null)
               {
               var materialAssemblyModels = new List<MaterialAssemblyModels>();
               var list = new List<MaterialForAssembly>();

               foreach (var materialItem in Roles.IsUserInRole(User.Identity.Name, "Admin") ? _iMaterial.RetrieveAllMaterials() : _iMaterial.RetrieveAllActiveMaterials())
                    {

                    var materialForAssembly = new MaterialForAssembly
                         {
                         Id = materialItem.Id
                        ,
                         Code = materialItem.Code
                        ,
                         Description = materialItem.Description
                        ,
                         Cost = materialItem.Cost
                        ,
                         Quantity = 0
                        ,
                         Unit = string.Empty
                         };
                    list.Add(materialForAssembly);
                    }

               materialAssemblyModels = MapMaterialItemsForEdit(list, id ?? 0);

               var jsonResult = JsonConvert.SerializeObject(materialAssemblyModels.OrderBy(m => m.Code));

               return Json(jsonResult, JsonRequestBehavior.AllowGet);
               }

          public ActionResult TableMaterialAssemblyResultForEdit(int id)
               {
               var materialAssemblyModels = new List<MaterialAssemblyModels>();
               var list = new List<MaterialForAssembly>();

               foreach (var materialItem in Roles.IsUserInRole(User.Identity.Name, "Admin") ? _iMaterial.RetrieveAllMaterials() : _iMaterial.RetrieveAllActiveMaterials())
                    {

                    var materialForAssembly = new MaterialForAssembly
                         {
                         Id = materialItem.Id
                        ,
                         Code = materialItem.Code
                        ,
                         Description = materialItem.Description
                        ,
                         Cost = materialItem.Cost
                        ,
                         Quantity = 0
                        ,
                         Unit = string.Empty
                         };
                    list.Add(materialForAssembly);
                    }

               materialAssemblyModels = MapMaterialItemsForEdit(list, id);

               var jsonResult = JsonConvert.SerializeObject(materialAssemblyModels.OrderBy(m => m.Code));

               return Json(jsonResult, JsonRequestBehavior.AllowGet);
               }

          // GET: Assembly/Details/5
          public ActionResult Details(int id)
               {
               return View();
               }


          // GET: Assembly/Create
          public ActionResult Create()
               {
               return PartialView("Create", new AssemblyModels());
               }

          // POST: Assembly/Create
          [HttpPost]
          public ActionResult Create(AssemblyModels assemblyObject)
               {
               try
                    {
                    // TODO: Add insert logic here
                    if (!ModelState.IsValid)
                         {
                         var errorMsg = new List<string>();

                         if (assemblyObject.Name == null)
                              errorMsg.Add("NAME is a required field.");

                         if (assemblyObject.Classification == null)
                              errorMsg.Add("CLASSIFICATION is a required field.");

                         if (assemblyObject.Materials.Count == 0)
                              errorMsg.Add("Please select at least one (1) material from the list");

                         var result = AddErrorMsg("Error", errorMsg);
                         return new JsonResult
                              {
                              JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                              Data = result
                              };
                         }
                    else
                         {
                         var localMaterialList = SanitizeMaterialList(assemblyObject.Materials);
                         assemblyObject.Materials = localMaterialList; //overwrite the material list after sanitation

                         var assemblyType = new TEntS.Types.Assembly.Assembly
                              {
                              Classification = assemblyObject.Classification
                            ,
                              Name = assemblyObject.Name
                            ,
                              Materials = MapMaterialModelToType(assemblyObject.Materials)
                              };

                         var userDetail = new TEntS.Types.UserDetails
                              {
                              Email = User.Identity.Name
                              };

                         _iAssembly.Create(assemblyType, userDetail);
                         }
                    return RedirectToAction("TableMaterialAssemblyResult", "Assembly");
                    }
               catch (Exception ex)
                    {
                    return new JsonResult
                         {
                         JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                         Data = AddErrorMsg("Error", new List<string> { ex.Message })
                         };
                    }
               }

          private Dictionary<string, List<string>> AddErrorMsg(string key, List<string> value)
               {
               var dictionaryItem = new Dictionary<string, List<string>>();
               dictionaryItem.Add(key, value);

               return dictionaryItem;
               }

          private List<MaterialForAssembly> MapMaterialModelToType(List<MaterialAssemblyModels> materials)
               {
               var materialListType = new List<MaterialForAssembly>();
               foreach (var model in materials)
                    {
                    var typeItem = new MaterialForAssembly
                         {
                         Id = model.Id
                       ,
                         Quantity = model.Quantity
                       ,
                         Unit = Enum.GetName(typeof(DefaultUnits), model.Unit)
                         };
                    materialListType.Add(typeItem);
                    }

               return materialListType;
               }

          private List<MaterialAssemblyModels> SanitizeMaterialList(List<MaterialAssemblyModels> materials)
               {
               var cleanMaterialList = new List<MaterialAssemblyModels>();

               var materialsWithDuplicates = materials.GroupBy(material => material.Id)
                   .Where(m => m.Count() > 1)
                   .Select(mat => mat).ToList();

               foreach (var duplicates in materialsWithDuplicates)
                    {
                    var duplicateList = duplicates.ToList();
                    var materialItem = new MaterialAssemblyModels
                         {
                         Id = duplicateList[duplicateList.Count - 1].Id
                        ,
                         Quantity = duplicateList[duplicateList.Count - 1].Quantity
                        ,
                         BasePrice = duplicateList[duplicateList.Count - 1].BasePrice,
                         ActualCost = duplicateList[duplicateList.Count - 1].ActualCost,
                         Unit = duplicateList[duplicateList.Count - 1].Unit
                         };
                    cleanMaterialList.Add(materialItem);
                    }

               var uniqueMaterials = materials.GroupBy(material => material.Id)
                   .Where(m => m.Count() == 1)
                   .Select(mat => mat).ToList();

               foreach (var uniqueList in uniqueMaterials)
                    {
                    var unique = uniqueList.ToList();
                    var materialItem = new MaterialAssemblyModels
                         {
                         Id = unique[unique.Count - 1].Id
                        ,
                         Quantity = unique[unique.Count - 1].Quantity
                        ,
                         BasePrice = unique[unique.Count - 1].BasePrice
                        ,
                         ActualCost = unique[unique.Count - 1].ActualCost
                        ,
                         Unit = unique[unique.Count - 1].Unit
                         };
                    cleanMaterialList.Add(materialItem);
                    }

               return cleanMaterialList;
               }

          // GET: Assembly/Edit/5
          public ActionResult Edit(int id)
               {
               try
                    {
                    var assembly = _iAssembly.RetrieveAssemblyDetailsById(id);
                    var assemblyModel = new AssemblyModels
                         {
                         Id = assembly.Id
                        ,
                         Classification = assembly.Classification
                        ,
                         Name = assembly.Name
                        ,
                         isActive = assembly.IsActive
                        ,
                         UnitPrice = (decimal)assembly.UnitPrice
                         ,
                         Materials = MapMaterialItems(assembly.Materials)
                        ,
                         CompleteMaterialList = RetriveListToDisplay(assembly.Materials)
                         };

                    return PartialView("Edit", assemblyModel);
                    }
               catch (Exception ex)
                    {
                    return new JsonResult
                         {
                         JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                         Data = AddErrorMsg("Error", new List<string> { ex.Message })
                         };
                    }
               }

          // POST: Assembly/Edit/5
          [HttpPost]
          public ActionResult Edit(int id, AssemblyModels assemblyObject)
               {
               try
                    {
                    // TODO: Add insert logic here
                    if (!ModelState.IsValid)
                         {
                         var errorMsg = new List<string>();

                         if (assemblyObject.Classification == null)
                              errorMsg.Add("CLASSIFICATION is a required field.");

                         if (assemblyObject.Materials.Count == 0)
                              errorMsg.Add("Please select at least one (1) material from the list");

                         var result = AddErrorMsg("Error", errorMsg);
                         return new JsonResult
                              {
                              JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                              Data = result
                              };
                         }
                    else
                         {

                         assemblyObject.Materials = SanitizeMaterialList(assemblyObject.Materials); //overwrite the material list after sanitation


                         var assemblyType = new TEntS.Types.Assembly.Assembly
                              {
                              Id = assemblyObject.Id,
                              Classification = assemblyObject.Classification
                            ,
                              Name = assemblyObject.Name
                            ,
                              Materials = MapMaterialModelToType(assemblyObject.Materials)
                              };

                         var userDetail = new TEntS.Types.UserDetails
                              {
                              Email = User.Identity.Name
                              };

                         _iAssembly.Update(assemblyType, userDetail);
                         }
                    return RedirectToAction("TableMaterialAssemblyResult", "Assembly");
                    }
               catch
                    {
                    return View();
                    }
               }

          // GET: Assembly/Delete/5
          public ActionResult Delete(int id)
               {
               return View();
               }

          // POST: Assembly/Delete/5
          [HttpPost]
          public ActionResult Delete(int itemId, AssemblyModels assemblyObject)
               {
               try
                    {
                    // TODO: Add delete logic here

                    var userDetail = new TEntS.Types.UserDetails
                         {
                         Email = User.Identity.Name
                         };

                    _iAssembly.Retire(itemId, userDetail);
                    return RedirectToAction("TableMaterialAssemblyResult", "Assembly");
                    }
               catch
                    {
                    return View();
                    }
               }

          #region
          private List<MaterialModels> RetriveListToDisplay(List<MaterialForAssembly> materialAssemblyList)
               {
               List<MaterialModels> list = new List<Models.MaterialModels>();
               foreach (var material in Roles.IsUserInRole(User.Identity.Name, "Admin") ? _iMaterial.RetrieveAllMaterials() : _iMaterial.RetrieveAllActiveMaterials())
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

                    list.Add(dto);
                    }
               foreach (var material in materialAssemblyList)
                    {
                    var materialFound = list.Where(m => m.Id == material.Id).FirstOrDefault();
                    var index = list.IndexOf(materialFound);
                    if (index > -1)
                         {
                         materialFound = MapMaterialToModel(material);
                         materialFound.IsSelected = true;
                         list[index] = materialFound;
                         }
                    }
               return list;
               }

          private MaterialModels MapMaterialToModel(MaterialForAssembly material)
               {
               return new MaterialModels
                    {
                    Code = material.Code,
                    Id = material.Id,
                    Quantity = material.Quantity,
                    Unit = material.Unit,
                    Description = material.Description,
                    IsSelected = true
                    };
               }

          private List<MaterialAssemblyModels> MapMaterialItemsForEdit(List<MaterialForAssembly> completeMaterialList, int id)
               {
               var materialModelList = new List<MaterialAssemblyModels>();
               foreach (var material in completeMaterialList)
                    {
                    var model = new MaterialAssemblyModels
                         {
                         Code = material.Code,
                         Description = material.Description,
                         Id = material.Id,
                         Quantity = material.Quantity,
                         ActualCost = material.Cost.ActualCost,
                         BasePrice = material.Cost.BasePrice,
                         Unit = SetDefaultUnit(material.Unit)
                         };

                    materialModelList.Add(model);
                    }
               if (id > 0)
                    {
                    var assemblyMaterials = new List<MaterialAssemblyModels>();

                    var assemblyItem = _iAssembly.RetrieveAssemblyDetailsById(id);
                    foreach (var material in assemblyItem.Materials)
                         {
                         var dto = new MaterialAssemblyModels
                              {
                              Code = material.Code,
                              Description = material.Description,
                              Id = material.Id,
                              Quantity = material.Quantity,
                              ActualCost = material.Cost.ActualCost,
                              BasePrice = material.Cost.BasePrice,
                              Unit = SetDefaultUnit(material.Unit)
                              };

                         assemblyMaterials.Add(dto);
                         }

                    foreach (var material in assemblyMaterials)
                         {
                         var foundMaterial = materialModelList.Where(a => a.Code == material.Code).FirstOrDefault();
                         if (foundMaterial != null)
                              {
                              foundMaterial.isChecked = true;
                              foundMaterial.Quantity = material.Quantity;
                              foundMaterial.Unit = material.Unit;
                              //materialModelList.Remove(foundMaterial);
                              //materialModelList.Add(foundMaterial);
                              }
                         }
                    }
               materialModelList.OrderBy(a => a.Code);

               return materialModelList;
               }

          private List<MaterialAssemblyModels> MapMaterialItems(List<TEntS.Types.Materials.MaterialForAssembly> materials)
               {
               var materialModelList = new List<MaterialAssemblyModels>();
               foreach (var material in materials)
                    {
                    var model = new MaterialAssemblyModels
                         {
                         Code = material.Code,
                         Description = material.Description,
                         Id = material.Id,
                         Quantity = material.Quantity,
                         ActualCost = material.Cost.ActualCost,
                         BasePrice = material.Cost.BasePrice,
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
                  ,
                    };

               return defaultUnit.SelectedId;
               }
          #endregion
          }
     }
