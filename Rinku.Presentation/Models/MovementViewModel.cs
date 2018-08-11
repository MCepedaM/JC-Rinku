using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rinku.Presentation.Models
{
    public class MovementViewModel
    {
        [Display(Name = "Identificador Único")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Debe selecionar un empleado"), Display(Name = "Empleado ")]
        public string EmpleadoSelect { get; set; }
        public EmployeeViewModel Empleado { get; set; }
        [Required(ErrorMessage = "Debe selecionar una fecha"), Display(Name = "Fecha")]
        [DataType(DataType.DateTime)]
        public DateTime Fecha { get; set; }
        [Display(Name = "Entregas")]
        public int Entregas { get; set; }
        [Display(Name = "Cubre Turno")]
        public bool CubreTurno { get; set; }
        [Display(Name = "Rol Cubierto")]
        public Int16 CubreRol { get; set; }
    }
}