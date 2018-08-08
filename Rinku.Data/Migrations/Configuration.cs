namespace Rinku.Data.Migrations
{
    using Domain;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Rinku.Data.ModelContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Rinku.Data.ModelContext context)
        {
            //  This method will be called after migrating to the latest version.
            Employee emp = null;
            Movement mov = null;
            //List<Employee> lstEmp = new List<Employee>();
            Random rd = new Random();
            for (int i = 1; i < 11; i++)
            {
                emp = new Employee();
                emp.Activo = true;
                emp.interno = rd.Next(1, 2) == 1 ? true : false;
                emp.Nombre = "Marco " + i.ToString();
                emp.Rol = Convert.ToInt16(rd.Next(1, 3));
                context.Empleado.AddOrUpdate(emp);
            }
            context.SaveChanges();
            int idUser = 1;
            emp = context.Empleado.Find(idUser);
            for (int i = 1; i < 100; i++)
            {
                if (i % 10 == 0)
                {
                    idUser++;
                    emp = context.Empleado.Find(idUser);
                    context.SaveChanges();
                }
                mov = new Movement();
                mov.CubreTurno = (emp.Rol == 3 && rd.Next(1, 2) == 1) ? true : false;
                mov.CubreRol = mov.CubreTurno ? Convert.ToInt16(rd.Next(1, 2)) : Convert.ToInt16(0);
                mov.Empleado = emp;
                mov.Entregas = rd.Next(1, 20);
                mov.Fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, rd.Next(1, 28));
                context.Movimiento.AddOrUpdate(mov);
            }
        }
    }
}
