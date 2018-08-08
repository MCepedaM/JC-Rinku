using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rinku.Domain
{
    public class Salary
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public Int16 Rol { get; set; }
        public bool interno { get; set; }
        public int DiasLaborados { get; set; }
        public int Entregas { get; set; }
        public decimal Adicional { get; set; }
        public decimal BonoXHora { get; set; }
        public decimal Subtotal { get; set; }
        public decimal PorcentajeIva { get; set; }
        public decimal MontoIva { get; set; }
        public decimal MontoTotal { get; set; }
        public decimal ValesDespensa { get; set; }

    }
}
