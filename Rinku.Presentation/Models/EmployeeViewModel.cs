using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rinku.Presentation.Models
{
    public class EmployeeViewModel
    {
        [Display(Name = "Id Unico")]
        public int Id { get; set; }
        [Required(ErrorMessage ="Debe capturar el nombre del empleado"), Display(Name = "Nombre empleado")]
        [StringLength(200, ErrorMessage ="A superado el limete de caracteres permitidos que son 200")]
        public string Nombre { get; set; }
        [Display(Name = "Rol")]
        [Required]
        public Int16 Rol { get; set; }
        [Required]
        [Display(Name = "Es interno")]
        public bool interno { get; set; }
        [Display(Name = "Esta Activo")]
        public bool Activo { get; set; }

        public string getDescripcionRol()
        {
            string descripcion = "";
            switch (Rol)
            {
                case 1:
                    descripcion = "Chofer";
                    break;
                case 2:
                    descripcion = "Cargador";
                    break;
                default:
                    descripcion = "Auxiliar";
                    break;
            }

            return descripcion;
        }
        
    }
}