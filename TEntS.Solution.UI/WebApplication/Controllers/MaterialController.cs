using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using TEntS.ClientBLL;
using TEntS.ClientBLL.ClientBLL;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class MaterialController : Controller
    {
        private IMaterial _material;
        public MaterialController()
        {
            _material = new Material(new MaterialBase());
        }
        // GET: Material
        public ActionResult Index()
        {
            var list = RetriveListToDisplay();

            return PartialView(list);
        }

        public JsonResult TableResult()
        {
            var list = RetriveListToDisplay();

            var jsonResult = JsonConvert.SerializeObject(list.ToArray());

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
        // GET: Material/Details/5
        public ActionResult Details(int Id)
        {
            return RedirectToAction("DashboardV1");
        }

        // GET: Material/Create
        public ActionResult Create()
        {
            return PartialView("Create", new MaterialModels());
        }

        // POST: Material/Create
        [HttpPost]
        public ActionResult Create(MaterialModels materialObject)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorMsg = new List<string>();

                    if (materialObject.Code == null)
                        errorMsg.Add("CODE is a required field.");

                    if (materialObject.Description == null)
                        errorMsg.Add("DESCRIPTION is a required field.");

                    var result = AddErrorMsg("Error", errorMsg);
                    return new JsonResult
                    {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        Data = result
                    };
                }

                var model = new TEntS.Types.Materials.Material
                {
                    Code = materialObject.Code,
                    Cost = new TEntS.Types.Materials.Costing
                    {
                        BasePrice = (double)materialObject.UnitPrice
                    },
                    Description = materialObject.Description
                };

                var userDetail = new TEntS.Types.UserDetails
                {
                    Email = User.Identity.Name
                };

                _material.Create(model, userDetail);

                return RedirectToAction("TableResult", "Material");
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

        // GET: Material/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var materialObject = _material.RetrieveActiveMaterialsById(id);
                var materialDto = new MaterialModels
                {
                    Code = materialObject.Code,
                    Description = materialObject.Description,
                    UnitPrice = materialObject.Cost.BasePrice
                };

                return PartialView("Edit", materialDto);
            }
            catch(Exception ex)
            {
                return new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = AddErrorMsg("Error", new List<string> { ex.Message })
                };
            }
        }

        // POST: Material/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, MaterialModels collection)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorMsg = new List<string>();
                    if (id == 0)
                        errorMsg.Add("ID is a required field.");

                    if (collection.Code == null)
                        errorMsg.Add("CODE is a required field.");

                    if (collection.Description == null)
                        errorMsg.Add("DESCRIPTION is a required field.");

                    var result = AddErrorMsg("Error", errorMsg);
                    return new JsonResult
                    {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        Data = result
                    };
                }

                // TODO: Add update logic here
                var materialObj = new TEntS.Types.Materials.Material
                {
                    Code = collection.Code,
                    Description = collection.Description,
                    Cost = new TEntS.Types.Materials.Costing
                    {
                        BasePrice = (double)collection.UnitPrice
                    },
                    Id = id,
                    IsActive = true,
                    CreationDate = DateTime.Now
                };

                var userDetail = new TEntS.Types.UserDetails
                {
                    Email = User.Identity.Name
                };

                _material.Update(materialObj, userDetail);

                return RedirectToAction("TableResult", "Material");
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

        // GET: Material/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var materialObject = _material.RetrieveActiveMaterialsById(id);
                var materialDto = new MaterialModels
                {
                    Code = materialObject.Code,
                    Description = materialObject.Description,
                    UnitPrice = materialObject.Cost.BasePrice
                };

                return PartialView("Delete", materialDto);
            }
            catch(Exception ex)
            {
                return new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = AddErrorMsg("Error", new List<string> { ex.Message })
                };
            }
        }

        // POST: Material/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, MaterialModels collection)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorMsg = new List<string>();

                    if (id == 0)
                        errorMsg.Add("ID is a required field.");

                    if (collection.Code == null)
                        errorMsg.Add("CODE is a required field.");

                    if (collection.Description == null)
                        errorMsg.Add("DESCRIPTION is a required field.");

                    var result = AddErrorMsg("Error", errorMsg);
                    return new JsonResult
                    {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        Data = result
                    };
                }

                // TODO: Add update logic here
                var userDetail = new TEntS.Types.UserDetails
                {
                    Email = User.Identity.Name
                };

                _material.Retire(id, userDetail);

                return RedirectToAction("TableResult", "Material");
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

        #region
        private List<MaterialModels> RetriveListToDisplay()
        {
            List<MaterialModels> list = new List<Models.MaterialModels>();
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

                list.Add(dto);
            }
            return list;
        }
        #endregion
    }
}
