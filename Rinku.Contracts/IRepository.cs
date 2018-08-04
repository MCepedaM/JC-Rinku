using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Data.SqlClient;
using Rinku.Domain;

namespace Rinku.Contracts
{
    public interface IRepository<T> where T : class, IEntity, new()
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is read only.
        /// </summary>
        /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets the entity by id.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        T Get(int id);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <remarks></remarks>
        T Update(T entity);

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <remarks></remarks>
        T Add(T entity);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <remarks></remarks>
        void Delete(T entity);

        /// <summary>
        /// Deletes the entity with the specified unique identifier.
        /// </summary>
        /// <param name="id">The entity's unique identifier.</param>
        void Delete(int id);

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Attaches the given entity to the context.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <remarks>It is used to repopulate a context with an entity that is known to already exist in the database.</remarks>
        T Attach(T entity);

        /// <summary>
        /// Saves (commits) the changes to the data store.
        /// </summary>
        /// <remarks></remarks>
        int SaveChanges();

        // TModel GetByName(string name);
        bool Exists(int id);

        /// <summary>
        /// Gets the entity count.
        /// </summary>
        /// <returns></returns>
        int GetCount();

        /// <summary>
        /// Execute Any Store Procedure 
        /// </summary>
        /// <typeparam name="T">Data Type to return</typeparam>
        /// <param name="Entity"></param>
        /// <param name="StoreProcedure">Name of Store Procedure</param>
        /// <param name="args">Parameters to apply to the Store procedure</param>
        /// <returns></returns>
        List<T> ExecuteProcedure<T>(string StoreProcedure, params SqlParameter[] args);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        T Single<T>(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes) where T : class;

        /// <summary>
        ///  Get all info with filters or include join info
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        IEnumerable<T> GetAll(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);
    }
}
