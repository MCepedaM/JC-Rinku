using Rinku.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Rinku.Services.Main.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IRinkuService" in both code and config file together.
    [ServiceContract]
    public interface IRinkuService
    {
        [OperationContract]
        List<Employee> GetEmpleados();
        [OperationContract]
        bool AddUpdateEmpleados(Employee emp);
        [OperationContract]
        bool DeleteEmpleados(Employee emp);
        [OperationContract]
        Employee GetEmpleado(Employee emp);


        //[OperationContract]
        //void GetMovimentos();
        //[OperationContract]
        //void AddMovimiento();
        //[OperationContract]
        //void DeleteMovimiento();
    }
}
