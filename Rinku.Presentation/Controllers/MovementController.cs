using Rinku.Presentation.ExternalServices;
using Rinku.Presentation.Helpers;
using Rinku.Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Service = Rinku.Presentation.Services.Nomina;


namespace Rinku.Presentation.Controllers
{
    public class MovementController : Controller
    {
        public ActionResult IndexDefault()
        {
            return Index(DateTime.Now.AddDays(-DateTime.Now.Day + 1).Date, DateTime.Now.AddDays(-DateTime.Now.Day).AddMonths(1), 0);
        }

        // GET: Movement
        public ActionResult Index(DateTime fechaInicio, DateTime fechaFin, int idEmpleado)
        {
            List<MovementViewModel> lstMovModel = new List<MovementViewModel>();
            MovementViewModel movModel = null;
            try
            {
                List<Services.Nomina.Movement> lstMov;
                var serviceNomina = GetService.nominaService;
                if (idEmpleado == 0)
                    lstMov = serviceNomina.GetMovimientos(fechaInicio, fechaFin).ToList();
                else
                    lstMov = serviceNomina.GetMovimientosByEmpleado(new Services.Nomina.Employee() {Id = idEmpleado }, fechaInicio, fechaFin).ToList();


                foreach (var mov in lstMov)
                {
                    movModel = new MovementViewModel();
                    movModel.CopyPropertiesFrom(mov);
                    movModel.Empleado = new EmployeeViewModel();
                    movModel.Empleado.CopyPropertiesFrom(mov.Empleado);
                    lstMovModel.Add(movModel);
                }
                return View(lstMovModel);

            }
            catch (Exception ex)
            {
                return Json(new { isWarning = false, isSuccess = false, msj = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            
        }

        // GET: Movement/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Movement/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movement/Create
        [HttpPost]
        public ActionResult Create(MovementViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Service.Movement mov = new Services.Nomina.Movement();
                        mov.CopyPropertiesFrom(model);
                        var idEmp = model.EmpleadoSelect.Split('-');
                        mov.Empleado = new Services.Nomina.Employee();
                        mov.Empleado.Id = Convert.ToInt32(idEmp[0].Trim());
                        var serviceNomina = GetService.nominaService;
                        serviceNomina.AddUpdateMovimientos(mov);

                        return Json(new { isError = false, isWarning = false, isSuccess = true, msj = "¡Los datos fueron guardados correctamente!", newurl ="" });

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

        // GET: Movement/Edit/5
        public ActionResult Edit(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MovementViewModel movModel = new MovementViewModel();
                    var serviceNomina = GetService.nominaService;
                    var mov = serviceNomina.GetMovientoById(id );
                    movModel.CopyPropertiesFrom(mov);
                    movModel.Empleado = new EmployeeViewModel();
                    movModel.Empleado.CopyPropertiesFrom(mov.Empleado);
                    movModel.EmpleadoSelect = movModel.Empleado.Id.ToString() + " - " + movModel.Empleado.Nombre + " [" + movModel.Empleado.getDescripcionRol() + "]";
                    return View(movModel);
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

        // POST: Movement/Edit/5
        [HttpPost]
        public ActionResult Edit(MovementViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Service.Movement mov = new Services.Nomina.Movement();
                        mov.CopyPropertiesFrom(model);
                        var idEmp = model.EmpleadoSelect.Split('-');
                        mov.Empleado = new Services.Nomina.Employee();
                        mov.Empleado.Id = Convert.ToInt32(idEmp[0].Trim());
                        var serviceNomina = GetService.nominaService;
                        serviceNomina.AddUpdateMovimientos(mov);

                        return Json(new { isError = false, isWarning = false, isSuccess = true, msj = "¡Los datos fueron Actualizados correctamente!", newurl = Url.Action("Index", "Movement",new { fechaInicio = DateTime.Now.AddDays(-DateTime.Now.Day + 1).Date, fechaFin = DateTime.Now.AddDays(-DateTime.Now.Day).AddMonths(1), idEmpleado = 0 }) });

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

        // GET: Movement/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var serviceNomina = GetService.nominaService;
                serviceNomina.DeleteMoviento(new Service.Movement() { Id = id });
                return Json(new { isError = false, Id = id, isSuccess = true, msj = "El Movimiento fue dado de baja.", newurl = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, isWarning = false, isSuccess = false, msj = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Movement/Delete/5
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
