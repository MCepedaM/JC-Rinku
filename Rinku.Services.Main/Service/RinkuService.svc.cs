using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Rinku.Domain;
using Rinku.Contracts;
using Autofac.Integration.Wcf;
using Autofac;
using Rinku.Data;
using System.ServiceModel.Activation;
using Autofac.Integration.Mvc;
using System.Web.Http.Dependencies;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;

namespace Rinku.Services.Main.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RinkuService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RinkuService.svc or RinkuService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class RinkuService : IRinkuService
    {
        #region Variables
        private ILifetimeScope _resolver;
        private IRepository<Employee> _EmployeeRepository;
        private IRepository<Movement> _MovementRepository;

        #endregion Variables

        #region Construtores
        public RinkuService()
        {
            var container = AutofacServiceHostFactory.Container;
            _resolver = container.BeginLifetimeScope();

            _EmployeeRepository = _resolver.Resolve<IRepository<Employee>>();
            _MovementRepository = _resolver.Resolve<IRepository<Movement>>();

        }
        #endregion Construtores

        #region Empleados
        public bool AddUpdateEmpleados(Employee emp)
        {
            if (emp.Id == 0)
                _EmployeeRepository.Add(emp);
            else
                _EmployeeRepository.Update(emp);
            _EmployeeRepository.SaveChanges();
            return true;
        }

        public bool DeleteEmpleados(Employee emp)
        {
            try
            {
                _EmployeeRepository.Delete(emp.Id);
                _EmployeeRepository.SaveChanges();

                return true;

            }
            catch (DbUpdateException dbEx)
            {
                return false;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public Employee GetEmpleado(Employee emp)
        {
           return _EmployeeRepository.Get(emp.Id);
        }

        public List<Employee> GetEmpleados()
        {
            return _EmployeeRepository.GetAll(w => w.Activo == true).ToList();
        }

        #endregion Empleados

        #region Movimientos

        public bool AddUpdateMovimientos(Movement mov)
        {
            mov.Empleado = _EmployeeRepository.Get(mov.Empleado.Id);
            if (mov.Id == 0)
                _MovementRepository.Add(mov);
            else
                _MovementRepository.Update(mov);
            _MovementRepository.SaveChanges();

            return true;
        }

        public List<Movement> GetMovimientos(DateTime fechainicio, DateTime FechaFin)
        {
            var resulr = _MovementRepository.GetAll(w => w.Fecha >= fechainicio && w.Fecha <= FechaFin, p=> p.Empleado).ToList();
            return resulr;
        }

        public List<Movement> GetMovimientosByEmpleado(Employee emp, DateTime fechainicio, DateTime FechaFin)
        {
            return _MovementRepository.GetAll(w => w.Empleado.Id == emp.Id  && w.Fecha >= fechainicio && w.Fecha <= FechaFin, p => p.Empleado).ToList();
        }

        public bool DeleteMoviento(Movement mov)
        {
            _MovementRepository.Delete(mov.Id);
            _MovementRepository.SaveChanges();

            return true;
        }

        public Movement GetMovientoById(int movId)
        {
            return _MovementRepository.GetAll(w => w.Id == movId, p => p.Empleado).FirstOrDefault();
        }

        #endregion Movimientos

        #region Calculo Salarios
        public List<Salary> GetSalarios(DateTime fechainicio, DateTime FechaFin, int IdEmpleado)
        {
            SqlParameter[] valores = { new SqlParameter("@FechaInicial", fechainicio.Date), new SqlParameter("@FechaFinal", FechaFin.Date.AddDays(1)), new SqlParameter("@IdUsuario", IdEmpleado) };

            return _MovementRepository.ExecuteProcedure<Salary>("msp_CalculaSueldo @FechaInicial, @FechaFinal, @IdUsuario", valores);
        }

        #endregion Calculo Salarios
    }
}
