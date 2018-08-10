using Rinku.Presentation.ExternalServices;
using Rinku.Presentation.Helpers;
using Rinku.Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service = Rinku.Presentation.Services.Nomina;

namespace Rinku.Presentation.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            List<EmployeeViewModel> lstEmp = new List<EmployeeViewModel>();
            EmployeeViewModel empModel= null;
            try
            {
                var serviceNomina = GetService.nominaService;
                var lstEmployees = serviceNomina.GetEmpleados();

                foreach (var emp in lstEmployees)
                {
                    empModel = new EmployeeViewModel();
                    empModel.CopyPropertiesFrom(emp);
                    lstEmp.Add(empModel);
                }

            }
            catch (Exception ex)
            {
                return Json(new { isWarning = false, isSuccess = false, msj = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return View(lstEmp);
        }

        // GET: Employee/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(EmployeeViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Service.Employee emp = new Services.Nomina.Employee();
                        emp.CopyPropertiesFrom(model);
                        var serviceNomina = GetService.nominaService;
                        serviceNomina.AddUpdateEmpleados(emp);

                        return Json(new { isError = false, isWarning = false, isSuccess = true, msj = "¡Los datos fueron guardados correctamente!", newurl = Url.Action("Index", "Employee") });

                    }
                    catch (Exception ex)
                    {
                        return Json(new { isError = true, isWarning = false, isSuccess = false, msj = ex.Message });
                    }
                }
                else
                {
                    return Json(new { isError = false, isWarning = true, isSuccess = false, msj = Errors.getModelError(ModelState) });
                }
                
                //return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    EmployeeViewModel empModel = new EmployeeViewModel();
                    var serviceNomina = GetService.nominaService;
                    var emple= serviceNomina.GetEmpleado(new Service.Employee() {  Id= id });
                    empModel.CopyPropertiesFrom(emple);

                    return View(empModel);
                }
                catch (Exception ex)
                {
                    return Json(new { isError = true, isWarning = false, isSuccess = false, msj = ex.Message });
                }
            }
            else
            {
                return Json(new { isError = false, isWarning = true, isSuccess = false, msj = Errors.getModelError(ModelState) });
            }   
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var serviceNomina = GetService.nominaService;
                if(serviceNomina.DeleteEmpleados(new Service.Employee() { Id = id }))                
                    return Json(new { isError = false, Id = id, isSuccess = true, msj = "El Empleado fue dado de baja.", newurl = "" }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { isWarning = true, Id = id, isSuccess = false, msj = "Primero debe dar de baja los movientos realizados para este empleado.", newurl = "" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { isError = true, isWarning = false, isSuccess = false, msj = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Employee/Delete/5
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
    }
}
