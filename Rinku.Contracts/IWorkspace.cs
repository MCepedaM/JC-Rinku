using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.SqlClient;
using Rinku.Domain;

namespace Rinku.Contracts
{
    /// <summary>
    /// Abstracts the use of persistence frameworks and the implementation of the unit of work pattern.
    /// </summary>
    /// <remarks></remarks>
    public interface IWorkspace
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is read only.
        /// </summary>
        /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        bool IsReadOnly { get; set; }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <remarks></remarks>
        T Add<T>(T entity) where T : class;

        /// <summary>
        /// Adds the specified entities.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities">The entities.</param>
        /// <remarks></remarks>
        IEnumerable<T> Add<T>(IEnumerable<T> entities) where T : class;

        /// <summary>
        /// Attaches the given entity to the context.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <remarks>It is used to repopulate a context with an entity that is known to already exist in the database.</remarks>
        T Attach<T>(T entity) where T : class;

        /// <summary>
        /// Counts the specified predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        int Count<T>(Expression<Func<T, bool>> predicate = null) where T : class;

        /// <summary>
        /// Deletes the entity specified in the expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <remarks></remarks>
        void Delete<T>(Expression<Func<T, bool>> expression) where T : class;

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <remarks></remarks>
        void Delete<T>(T entity) where T : class;

        /// <summary>
        /// Deletes all the entities of the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <remarks></remarks>
        void DeleteAll<T>() where T : class;

        /// <summary>
        /// Checks whether the entity is attached to the context. If not, the entity gets attached.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <remarks></remarks>
        void EnsureAttached<T>(T entity) where T : class;

        /// <summary>
        /// Executes the given DDL/DML command against the database.
        /// </summary>
        /// <param name="sql">The command string.</param>
        /// <param name="parameters">The parameters to apply to the command string.</param>
        /// <returns>The result returned by the database after executing the command.</returns>
        int ExecuteCommand(string sql, params object[] parameters);

        /// <summary>
        /// Gets the last entity in the set ordering by Id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        T Last<T>() where T : class, IEntity;

        /// <summary>
        /// Gets the last entity in the set ordering by the specified property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        T Last<T, TProperty>(Expression<Func<T, TProperty>> orderSelector) where T : class;

        /// <summary>
        /// Returns a query without a predicate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IQueryable<T> Query<T>() where T : class;

        /// <summary>
        /// Returns a query using the specified predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="includes">The include expressions expressions.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        IQueryable<T> Query<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class;

        /// <summary>
        /// Commits the changes to the data store.
        /// </summary>
        /// <returns>The number of objects written to the underlying database.</returns>
        int SaveChanges();

        /// <summary>
        /// Returns a query using the specified predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        IQueryable<TResult> Select<TSource, TResult>(Expression<Func<TSource, TResult>> expression, Expression<Func<TSource, bool>> predicate) where TSource : class;

        /// <summary>
        /// Gets a single entity specified by the expression and includes the properties especified in the expressions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="includes">The include expressions expressions.</param>
        T Single<T>(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes) where T : class;

        /// <summary>
        /// Creates a raw SQL query that will return elements of the given generic type.
        /// The type can be any type that has properties that match the names of the
        /// columns returned from the query, or can be a simple primitive type. The type
        /// does not have to be an entity type. The results of this query are never tracked
        /// by the context even if the type of object returned is an entity type.
        /// </summary>
        /// <typeparam name="T">The type of object returned by the query.</typeparam>
        /// <param name="sql">The SQL query string.</param>
        /// <param name="parameters">The parameters to apply to the SQL query string.</param>
        /// <returns>A System.Collections.Generic.IEnumerable&lt;T&gt; object that will execute the
        /// query when it is enumerated.</returns>
        IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <remarks></remarks>
        T Update<T>(T entity) where T : class;

        /// <summary>
        /// Execute Any Store Procedure 
        /// </summary>
        /// <typeparam name="T">Data Type to return</typeparam>
        /// <param name="StoreProcedure">Name of Store Procedure</param>
        /// <param name="args">Parameters to apply to the Store procedure</param>
        /// <returns></returns>
        List<T> ExecuteProcedure<T>(string StoreProcedure, params SqlParameter[] args);
    }
}