using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rinku.Presentation.Models
{
    public class MovementViewModel
    {
        public int Id { get; set; }
        //public EmployeeViewModel Empleado { get; set; }
        [Required(ErrorMessage = "Debe selecionar un empleado"), Display(Name = "Empleado")]
        public string NombreEmpleado{get; set;}
        [Required(ErrorMessage = "Debe selecionar una fecha"), Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }
        public int Entregas { get; set; }
        public bool CubreTurno { get; set; }
        public Int16 CubreRol { get; set; }
    }
}