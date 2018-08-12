using System;
using System.ComponentModel.DataAnnotations;

namespace Rinku.Presentation.Models
{
    public class SalaryViewModel
    {
        [Display(Name = "Identificador Único")]
        public int Id { get; set; }
        [Display(Name = "Nombre empleado")]
        public string Nombre { get; set; }
        [Display(Name = "Rol")]
        public Int16 Rol { get; set; }
        [Display(Name = "Interno")]
        public bool interno { get; set; }
        [Display(Name = "Dias laborados")]
        public int DiasLaborados { get; set; }
        [Display(Name = "Entregas")]
        public int Entregas { get; set; }
        [Display(Name = "Bono Adicional")]
        public decimal Adicional { get; set; }
        [Display(Name = "Bono x Hora")]
        public decimal BonoXHora { get; set; }
        [Display(Name = "Subtotal")]
        public decimal Subtotal { get; set; }
        [Display(Name = "% Iva")]
        public decimal PorcentajeIva { get; set; }
        [Display(Name = "Monto Iva")]
        public decimal MontoIva { get; set; }
        [Display(Name = "Monto")]
        public decimal MontoTotal { get; set; }
        [Display(Name = "Vales de despensa")]
        public decimal ValesDespensa { get; set; }

        public decimal totalAPagar()
        {
            return MontoIva + ValesDespensa;
        }
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