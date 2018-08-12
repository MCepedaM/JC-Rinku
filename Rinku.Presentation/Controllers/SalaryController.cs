using Rinku.Presentation.ExternalServices;
using Rinku.Presentation.Helpers;
using Rinku.Presentation.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.Mvc;


namespace Rinku.Presentation.Controllers
{
    public class SalaryController : Controller
    {
        // GET: Salary
        public ActionResult Index()
        {
            return View(new List<SalaryViewModel>());
        }

       [HttpGet]
       public ActionResult GetCalculoNomina(string fechaString, int idEmpleado)
       {
            DateTime fecha = DateTime.ParseExact(fechaString, "d-M-yyyy", CultureInfo.InvariantCulture);
            SalaryViewModel salModel = new SalaryViewModel();
            StringBuilder sb = new StringBuilder();
            var serviceNomina = GetService.nominaService;
            var lstSuedos = serviceNomina.GetSalarios(fecha, fecha.AddMonths(1), idEmpleado);

            foreach (var sueldo in lstSuedos)
            {
                salModel = new SalaryViewModel();
                salModel.CopyPropertiesFrom(sueldo);
                sb.Append("<tr><td>" + salModel.Nombre + "</td><td>" +
                            salModel.getDescripcionRol() + "</td><td>" +
                            (salModel.interno ? "Si" : "No") + "</td><td>" +
                            salModel.DiasLaborados + "</td><td>" +
                            salModel.Entregas + "</td><td>" +
                            salModel.Adicional + "</td><td>" +
                            salModel.BonoXHora + "</td><td>" +
                            salModel.Subtotal + "</td><td>" +
                            "( " + salModel.PorcentajeIva + "%) " + salModel.MontoIva + "</td><td>" +
                            salModel.MontoTotal + "</td><td>" +
                            salModel.ValesDespensa + "</td><td>" +
                                (salModel.MontoTotal + salModel.ValesDespensa).ToString("#.##") + "</ td ></ tr > ");
            }
            return Content(sb.ToString());

       }
    }
}
