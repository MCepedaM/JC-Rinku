using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rinku.Domain
{
    public class Movement : IEntity
    {
        public int Id { get; set; }
        public Employee Empleado { get; set; } 
        public DateTime Fecha { get; set; }
        public int Entregas { get; set; }
        public bool CubreTurno { get; set; }
        public Int16 CubreRol { get; set; }

    }
}
