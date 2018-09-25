using EmployeeManagement.Logic;
using EmployeeManagement.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        CommonFunction common = new CommonFunction();

        public ActionResult Index()
        {
            return View();
        }

        #region Sample1 - Custom Grid with paging
        public ActionResult Sample1(int page = 1, int pageSize = 10)
        {
            EmployeeModel objModel = new EmployeeModel();
            objModel.StaticPageSize = 10;

            BindSample1(objModel, page, pageSize);
            return View(objModel);
        }
        public ActionResult Sample1FilterSearch(EmployeeModel objModel, int page = 1, int pageSize = 10)
        {
            BindSample1(objModel, page, pageSize);
            return PartialView("Sample1List", objModel);
        }
        public void BindSample1(EmployeeModel objModel, int page, int pageSize)
        {
            CityManager objCityManager = new CityManager(new DataContext());
            EmployeeManager context = new EmployeeManager(new DataContext());

            StringBuilder query = new StringBuilder();
            List<string> colName = common.GetColumns(CommonFunction.module.Employee.ToString());
            query = common.GetSqlTableQuery(CommonFunction.module.Employee.ToString());
            if (objModel != null)
                objModel.StaticPageSize = pageSize;

            //objModel.CityList = Extens.ToSelectList(objCityManager.GetDtCity(), "CityID", "CityName");
            context.setModel(query, objModel, colName, "Name", page, pageSize);
        }
        #endregion

        #region Sample2 - Custom Grid with paging and sorting
        public ActionResult Sample2(int page = 1, int pageSize = 10)
        {
            EmployeeModel objModel = new EmployeeModel();
            objModel.StaticPageSize = 10;

            BindSample2(objModel, page, pageSize);
            return View(objModel);
        }
        public ActionResult Sample2FilterSearch(EmployeeModel objModel, int page = 1, int pageSize = 10)
        {
            BindSample2(objModel, page, pageSize);
            return PartialView("Sample2List", objModel);
        }
        public void BindSample2(EmployeeModel objModel, int page, int pageSize)
        {
            CityManager objCityManager = new CityManager(new DataContext());
            EmployeeManager context = new EmployeeManager(new DataContext());

            StringBuilder query = new StringBuilder();
            List<string> colName = common.GetColumns(CommonFunction.module.Employee.ToString());
            query = common.GetSqlTableQuery(CommonFunction.module.Employee.ToString());
            if (objModel != null)
                objModel.StaticPageSize = pageSize;

            context.setModel(query, objModel, colName, "Name", page, pageSize);
        }
        #endregion

        #region Sample3 - Custom Grid with Each Column filter, paging and sorting
        public ActionResult Sample3(int page = 1, int pageSize = 10)
        {
            EmployeeModel objModel = new EmployeeModel();
            objModel.StaticPageSize = 10;

            BindSample3(objModel, page, pageSize);
            return View(objModel);
        }
        public ActionResult Sample3FilterSearch(EmployeeModel objModel, int page = 1, int pageSize = 10)
        {
            BindSample3(objModel, page, pageSize);
            return PartialView("Sample3List", objModel);
        }
        public void BindSample3(EmployeeModel objModel, int page, int pageSize)
        {
            CityManager objCityManager = new CityManager(new DataContext());
            EmployeeManager context = new EmployeeManager(new DataContext());

            StringBuilder query = new StringBuilder();
            List<string> colName = common.GetColumns(CommonFunction.module.Employee.ToString());
            query = common.GetSqlTableQuery(CommonFunction.module.Employee.ToString());
            if (objModel != null)
                objModel.StaticPageSize = pageSize;

            objModel.CityList = Extens.ToSelectList(objCityManager.GetDtCity(), "CityID", "CityName");
            context.setModel(query, objModel, colName, "Name", page, pageSize);
        }
        #endregion

        #region Sample4 - Custom Grid with Each Column filter, paging, sorting and CRUD operation
        public ActionResult Sample4(int page = 1, int pageSize = 10)
        {
            EmployeeModel objModel = new EmployeeModel();
            objModel.StaticPageSize = 10;

            BindSample4(objModel, page, pageSize);
            return View(objModel);
        }
        public ActionResult Sample4FilterSearch(EmployeeModel objModel, int page = 1, int pageSize = 10)
        {
            BindSample4(objModel, page, pageSize);
            return PartialView("Sample4List", objModel);
        }
        public void BindSample4(EmployeeModel objModel, int page, int pageSize)
        {
            CityManager objCityManager = new CityManager(new DataContext());
            EmployeeManager context = new EmployeeManager(new DataContext());

            StringBuilder query = new StringBuilder();
            List<string> colName = common.GetColumns(CommonFunction.module.Employee.ToString());
            query = common.GetSqlTableQuery(CommonFunction.module.Employee.ToString());
            if (objModel != null)
                objModel.StaticPageSize = pageSize;

            objModel.CityList = Extens.ToSelectList(objCityManager.GetDtCity(), "CityID", "CityName");
            context.setModel(query, objModel, colName, "Name", page, pageSize);
        }

        [HttpPost]
        public ActionResult AddEditEmployee(int Id = 0)
        {
            CityManager objCityManager = new CityManager(new DataContext());
            EmployeeManager context = new EmployeeManager(new DataContext());

            EmployeeModel objModel = new EmployeeModel();
            if (Id != 0)
            {
                objModel.Table = context.GetEmployeeById(Id);
            }
            else { objModel.Table = new Employee(); }
            objModel.CityList = Extens.ToSelectList(objCityManager.GetDtCity(), "CityID", "CityName");

            return PartialView("EmployeeCRUD", objModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveEmployee(EmployeeModel objModel, int page = 1, int pageSize = 10)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join("|", ModelState.Values.SelectMany(e => e.Errors).Select(em => em.ErrorMessage));
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, message);
            }
            CityManager objCityManager = new CityManager(new DataContext());
            EmployeeManager context = new EmployeeManager(new DataContext());
            //Save      
            var msg = context.SaveEmployee(objModel.Table);
            if (msg.Contains("exists"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "exists");
            }
            else
            {
                if (objModel.StaticPageSize != 0)
                {
                    pageSize = objModel.StaticPageSize;
                }
                objModel.StaticPageSize = pageSize;//objModel.StaticPageSize = 10;

                BindSample4(objModel, page, pageSize);
                return PartialView("Sample4List", objModel);
            }
        }

        [HttpPost]
        public ActionResult DeleteEmployee(string Id, EmployeeModel objModel, int page = 1, int pageSize = 10)
        {
            CityManager objCityManager = new CityManager(new DataContext());
            EmployeeManager context = new EmployeeManager(new DataContext());

            if (Id.LastIndexOf(',') > 0)
            { Id = Id.Remove(Id.LastIndexOf(',')); }

            bool flgDelete = false;
            bool flgDeleteError = false;
            var strList = Id.Split(',');
            foreach (var stId in strList)
            {
                try
                {
                    if (!string.IsNullOrEmpty(stId))
                    {
                        context.DeleteEmployee(Convert.ToInt32(stId));
                        flgDelete = true;
                    }
                }
                catch (Exception ex)
                {
                    flgDeleteError = true;
                }
            }

            if (flgDelete && flgDeleteError)
            {
                string msg = "DeleteNotAllowedForSomeRec";
                ModelState.AddModelError("Error", msg);
            }
            else if (flgDelete)
            {
                string msg = "SuccessfullyDeleted";
                ModelState.AddModelError("Success", msg);
            }
            else
            {
                string msg = "DeleteNotAllowed";
                ModelState.AddModelError("Error", msg);
            }

            if (objModel.StaticPageSize != 0)
            {
                pageSize = objModel.StaticPageSize;
            }
            objModel.StaticPageSize = pageSize;//objModel.StaticPageSize = 10;

            BindSample4(objModel, page, pageSize);
            return PartialView("Sample4List", objModel);
        }
        #endregion

        #region Sample5 - Multiple custom grid on single page
        public ActionResult Sample5(int page = 1, int pageSize = 10)
        {
            EmployeeModel objModel = new EmployeeModel();
            objModel.StaticPageSize = 10;

            BindSample5(objModel, page, pageSize);
            return View(objModel);
        }
        public void BindSample5(EmployeeModel objModel, int page, int pageSize)
        {
            CityManager objCityManager = new CityManager(new DataContext());
            EmployeeManager context = new EmployeeManager(new DataContext());

            StringBuilder query = new StringBuilder();
            List<string> colName = common.GetColumns(CommonFunction.module.Employee.ToString());
            query = common.GetSqlTableQuery(CommonFunction.module.Employee.ToString());
            if (objModel != null)
                objModel.StaticPageSize = pageSize;

            //objModel.CityList = Extens.ToSelectList(objCityManager.GetDtCity(), "CityID", "CityName");
            context.setModel(query, objModel, colName, "Name", page, pageSize);
        }
        #endregion

        #region Sample6 - Thumbnail view with paging, sorting and image preview
        public ActionResult Sample6(int page = 1, int pageSize = 10)
        {
            EmployeeModel objModel = new EmployeeModel();
            objModel.StaticPageSize = 10;

            BindSample6(objModel, page, pageSize);
            return View(objModel);
        }
        public ActionResult Sample6FilterSearch(EmployeeModel objModel, int page = 1, int pageSize = 10)
        {
            objModel.SortNameType = objModel.fieldName + "_" + objModel.sortOrder;
            BindSample6(objModel, page, pageSize);
            return PartialView("Sample6List", objModel);
        }
        public void BindSample6(EmployeeModel objModel, int page, int pageSize)
        {
            CityManager objCityManager = new CityManager(new DataContext());
            EmployeeManager context = new EmployeeManager(new DataContext());

            StringBuilder query = new StringBuilder();
            List<string> colName = common.GetColumns(CommonFunction.module.Employee.ToString());
            query = common.GetSqlTableQuery(CommonFunction.module.Employee.ToString());
            if (objModel != null)
                objModel.StaticPageSize = pageSize;

            context.setModel(query, objModel, colName, "Name", page, pageSize);
        }
        #endregion

        #region Sample7 -  Thumbnail view with Infinite scroll
        public ActionResult Sample7(int page = 1, int pageSize = 10)
        {
            EmployeeModel objModel = new EmployeeModel();
            //objModel.StaticPageSize = 10;
            //BindSample7(objModel, page, pageSize);
            return View(objModel);
        }
        public ActionResult Sample7FilterSearch(EmployeeModel objModel, int page = 1, int pageSize = 10)
        {
            objModel.SortNameType = objModel.fieldName + "_" + objModel.sortOrder;
            pageSize = pageSize == 10 ? 12 : pageSize;
            BindSample7(objModel, page, pageSize);

            var NoMoreData = objModel.dynamicList.Count < pageSize;
            var HTMLString = RenderPartialViewToString("Sample7List", objModel);
            return Json(new { NoMoreData = NoMoreData, HTMLString = HTMLString }, JsonRequestBehavior.AllowGet);
        }
        public void BindSample7(EmployeeModel objModel, int page, int pageSize)
        {
            CityManager objCityManager = new CityManager(new DataContext());
            EmployeeManager context = new EmployeeManager(new DataContext());

            StringBuilder query = new StringBuilder();
            List<string> colName = common.GetColumns(CommonFunction.module.Employee.ToString());
            query = common.GetSqlTableQuery(CommonFunction.module.Employee.ToString());
            if (objModel != null)
                objModel.StaticPageSize = pageSize;

            context.setModel(query, objModel, colName, "Name", page, pageSize);
        }
        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");
            ViewData.Model = model;
            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult =
                ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext
                (ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
        #endregion

        #region Sample8 - Semi Thumbnail view with paging, sorting and image preview
        public ActionResult Sample8(int page = 1, int pageSize = 10)
        {
            EmployeeModel objModel = new EmployeeModel();
            objModel.StaticPageSize = 10;

            BindSample8(objModel, page, pageSize);
            return View(objModel);
        }
        public ActionResult Sample8FilterSearch(EmployeeModel objModel, int page = 1, int pageSize = 10)
        {
            objModel.SortNameType = objModel.fieldName + "_" + objModel.sortOrder;
            BindSample8(objModel, page, pageSize);
            return PartialView("Sample8List", objModel);
        }
        public void BindSample8(EmployeeModel objModel, int page, int pageSize)
        {
            CityManager objCityManager = new CityManager(new DataContext());
            EmployeeManager context = new EmployeeManager(new DataContext());

            StringBuilder query = new StringBuilder();
            List<string> colName = common.GetColumns(CommonFunction.module.Employee.ToString());
            query = common.GetSqlTableQuery(CommonFunction.module.Employee.ToString());
            if (objModel != null)
                objModel.StaticPageSize = pageSize;

            context.setModel(query, objModel, colName, "Name", page, pageSize);
        }
        #endregion

        #region Sample9 - Semi Thumbnail view with Infinite scroll
        public ActionResult Sample9(int page = 1, int pageSize = 10)
        {
            EmployeeModel objModel = new EmployeeModel();
            //objModel.StaticPageSize = 10;
            //BindSample9(objModel, page, pageSize);
            return View(objModel);
        }
        public ActionResult Sample9FilterSearch(EmployeeModel objModel, int page = 1, int pageSize = 10)
        {
            objModel.SortNameType = objModel.fieldName + "_" + objModel.sortOrder;
            pageSize = pageSize == 10 ? 12 : pageSize;
            BindSample9(objModel, page, pageSize);

            var NoMoreData = objModel.dynamicList.Count < pageSize;
            var HTMLString = RenderPartialViewToString("Sample9List", objModel);
            return Json(new { NoMoreData = NoMoreData, HTMLString = HTMLString }, JsonRequestBehavior.AllowGet);
        }
        public void BindSample9(EmployeeModel objModel, int page, int pageSize)
        {
            CityManager objCityManager = new CityManager(new DataContext());
            EmployeeManager context = new EmployeeManager(new DataContext());

            StringBuilder query = new StringBuilder();
            List<string> colName = common.GetColumns(CommonFunction.module.Employee.ToString());
            query = common.GetSqlTableQuery(CommonFunction.module.Employee.ToString());
            if (objModel != null)
                objModel.StaticPageSize = pageSize;

            context.setModel(query, objModel, colName, "Name", page, pageSize);
        }
        #endregion
    }
}