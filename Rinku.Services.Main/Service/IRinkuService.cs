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
        #region Empleados

        /// <summary>
        /// Obtiene la lista de empleados
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Employee> GetEmpleados();
        /// <summary>
        /// Actualiza o guarda un nuevo empleado
        /// Actualiza si el campo Id diferente de 0 "cero"
        /// </summary>
        /// <param name="emp">Entidad empleado</param>
        /// <returns></returns>
        [OperationContract]
        bool AddUpdateEmpleados(Employee emp);
        /// <summary>
        /// Borra un empleado segun su Id unico
        /// </summary>
        /// <param name="emp"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteEmpleados(Employee emp);
        /// <summary>
        /// Obtiene un empleado en especifico segun su Id Unico
        /// </summary>
        /// <param name="emp"></param>
        /// <returns></returns>
        [OperationContract]
        Employee GetEmpleado(Employee emp);

        #endregion Empleados

        #region Movimientos

        /// <summary>
        /// Actualiza o guarda un movimiento
        /// Actualiza si el Id unico viene diferente de 0 "cero"
        /// </summary>
        /// <param name="mov"></param>
        /// <returns></returns>
        [OperationContract]
        bool AddUpdateMovimientos(Movement mov);
        /// <summary>
        /// Obtiene todos los movimientos en un rango especifico
        /// </summary>
        /// <param name="fechainicio"></param>
        /// <param name="FechaFin"></param>
        /// <returns></returns>
        [OperationContract]
        List<Movement> GetMovimientos(DateTime fechainicio, DateTime FechaFin);
        /// <summary>
        /// Obtiene los movimientos realizados por un empleado y en un rango especifico
        /// </summary>
        /// <param name="emp"></param>
        /// <param name="fechainicio"></param>
        /// <param name="FechaFin"></param>
        /// <returns></returns>
        [OperationContract]
        List<Movement> GetMovimientosByEmpleado(Employee emp, DateTime fechainicio, DateTime FechaFin);
        /// <summary>
        /// Elmina un movimiento
        /// </summary>
        /// <param name="mov"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteMoviento(Movement mov);

        #endregion Movimientos

        #region Calculo Salarios
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fechainicio"></param>
        /// <param name="FechaFin"></param>
        /// <param name="IdEmpleado"></param>
        /// <returns></returns>
        [OperationContract]
        List<Salary> GetSalarios(DateTime fechainicio, DateTime FechaFin, int IdEmpleado);

        #endregion Calculo Salarios

    }
}
