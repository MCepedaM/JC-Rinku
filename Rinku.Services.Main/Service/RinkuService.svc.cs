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

namespace Rinku.Services.Main.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RinkuService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RinkuService.svc or RinkuService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class RinkuService : IRinkuService
    {
        #region Variables
        private IRepository<Employee> _EmployeeRepository;
        private ILifetimeScope _resolver;
        #endregion Variables

        #region Construtores
        public RinkuService()
        {
            var container = AutofacServiceHostFactory.Container;
            _resolver = container.BeginLifetimeScope();

            _EmployeeRepository = _resolver.Resolve<IRepository<Employee>>();
            
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
            _EmployeeRepository.Delete(emp.Id);
            _EmployeeRepository.SaveChanges();

            return true;
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
    }
}
