using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rinku.Domain
{
    public class Employee : IEntity
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public Int16 Rol { get; set; }
        public bool interno { get; set; }
        public bool Activo { get; set; }
    }
}
