using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Data.SqlClient;
using Rinku.Contracts;
using Rinku.Domain;
using Rinku.Data.Properties;

namespace Rinku.Data
{
    public class EfWorkspace : IWorkspace
    {
        private readonly DbContext _context;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EfWorkspace" /> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <remarks></remarks>
        public EfWorkspace(DbContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            _context = context;
        }

        #region IWorkspace Members

        /// <summary>
        ///     Gets or sets a value indicating whether the entities retrieved are read only.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>No changes are tracked when in read only mode. Also, methods to update, insert or delete must not be invoked when this property is set to true.</remarks>
        public virtual bool IsReadOnly { get; set; }

        /// <summary>
        ///     Adds the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <remarks></remarks>
        public virtual T Add<T>(T entity) where T : class
        {
            ChechIsNotReadOnly();

            return _context.Set<T>().Add(entity);
        }

        /// <summary>
        ///     Adds the specified entities.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities">The entities.</param>
        /// <remarks></remarks>
        public virtual IEnumerable<T> Add<T>(IEnumerable<T> entities) where T : class
        {
            ChechIsNotReadOnly();

            return entities.Select(Add);
        }

        /// <summary>
        ///     Attaches the given entity to the context.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <remarks>It is used to repopulate a context with an entity that is known to already exist in the database.</remarks>
        public virtual T Attach<T>(T entity) where T : class
        {
            return _context.Set<T>().Attach(entity);
        }

        /// <summary>
        ///     Counts the specified predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual int Count<T>(Expression<Func<T, bool>> predicate = null) where T : class
        {
            return Query(predicate).Count();
        }

        /// <summary>
        ///     Deletes the entity specified in the expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <remarks></remarks>
        public virtual void Delete<T>(Expression<Func<T, bool>> expression) where T : class
        {
            ChechIsNotReadOnly();

            foreach (var entity in QueryInternal<T>().Where(expression))
                Delete(entity);
        }

        /// <summary>
        ///     Deletes the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <remarks></remarks>
        public virtual void Delete<T>(T entity) where T : class
        {
            ChechIsNotReadOnly();

            EnsureAttached(entity);

            _context.Set<T>().Remove(entity);
        }

        /// <summary>
        ///     Deletes all the entities of the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <remarks></remarks>
        public virtual void DeleteAll<T>() where T : class
        {
            ChechIsNotReadOnly();

            foreach (T entity in QueryInternal<T>())
                Delete(entity);
        }

        /// <summary>
        ///     Checks whether the entity is attached to the context. If not, the entity gets attached.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <remarks></remarks>
        public virtual void EnsureAttached<T>(T entity) where T : class
        {
            if (_context.Set<T>().Local.All(e => e != entity))
            {
                Attach(entity);
            }
        }

        /// <summary>
        ///     Executes the given DDL/DML command against the database.
        /// </summary>
        /// <param name="sql">The command string.</param>
        /// <param name="parameters">The parameters to apply to the command string.</param>
        /// <returns>The result returned by the database after executing the command.</returns>
        public int ExecuteCommand(string sql, params object[] parameters)
        {
            return _context.Database.ExecuteSqlCommand(sql, parameters);
        }

        /// <summary>
        ///     Gets the last entity in the set ordering by Id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual T Last<T>() where T : class, IEntity
        {
            return QueryInternal<T>().OrderByDescending(x => x.Id).FirstOrDefault();
        }

        /// <summary>
        ///     Gets the last entity in the set ordering by the specified property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        public virtual T Last<T, TProperty>(Expression<Func<T, TProperty>> orderSelector) where T : class
        {
            return QueryInternal<T>().OrderByDescending(orderSelector).FirstOrDefault();
        }

        public virtual IQueryable<T> Query<T>() where T : class
        {
            return Query<T>(null);
        }

        /// <summary>
        ///     Returns a query using the specified predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="includes">The include expressionss.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual IQueryable<T> Query<T>(Expression<Func<T, bool>> predicate,
                                              params Expression<Func<T, object>>[] includes) where T : class
        {
            if (includes != null && predicate != null)
                return includes.Aggregate(QueryInternal<T>(), (current, include) => current.Include(include)).Where(predicate);
            if (includes != null)
                return includes.Aggregate(QueryInternal<T>(), (current, include) => current.Include(include));
            if (predicate != null)
                return QueryInternal<T>().Where(predicate);
            return QueryInternal<T>();
        }

        /// <summary>
        ///     Commits the changes to the data store.
        /// </summary>
        /// <returns>The number of objects written to the underlying database.</returns>
        public virtual int SaveChanges()
        {
            return _context.SaveChanges();
        }

        /// <summary>
        ///     Returns a query using the specified predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual IQueryable<TResult> Select<TSource, TResult>(Expression<Func<TSource, TResult>> expression,
                                                                    Expression<Func<TSource, bool>> predicate)
            where TSource : class
        {
            if (predicate != null)
                return QueryInternal<TSource>().Where(predicate).Select(expression);
            return QueryInternal<TSource>().Select(expression);
        }

        /// <summary>
        ///     Gets a single entity specified by the expression and includes the properties especified in the expressions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="includes">The include expressionss.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual T Single<T>(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes)
            where T : class
        {
            if (includes == null || includes.Length < 1)
                return QueryInternal<T>().Where(expression).SingleOrDefault();
            IQueryable<T> result =
                includes.Aggregate(QueryInternal<T>(), (current, include) => current.Include(include)).Where(expression);
            return result.SingleOrDefault();
        }

        /// <summary>
        ///     Creates a raw SQL query that will return elements of the given generic type.
        ///     The type can be any type that has properties that match the names of the
        ///     columns returned from the query, or can be a simple primitive type. The type
        ///     does not have to be an entity type. The results of this query are never tracked
        ///     by the context even if the type of object returned is an entity type.
        /// </summary>
        /// <typeparam name="T">The type of object returned by the query.</typeparam>
        /// <param name="sql">The SQL query string.</param>
        /// <param name="parameters">The parameters to apply to the SQL query string.</param>
        /// <returns>
        ///     A System.Collections.Generic.IEnumerable&lt;T&gt; object that will execute the
        ///     query when it is enumerated.
        /// </returns>
        public IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters)
        {
            return _context.Database.SqlQuery<T>(sql, parameters);
        }

        /// <summary>
        ///     Updates the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <remarks></remarks>
        public virtual T Update<T>(T entity) where T : class
        {
            ChechIsNotReadOnly();

            EnsureAttached(entity);
            var entry = _context.Entry(entity);
            //ObjectStateEntry state = ObjContext.ObjectStateManager.GetObjectStateEntry(entity);
            if (entry.State == EntityState.Unchanged)
                entry.State = (EntityState.Modified);

            return entry.Entity;
            //var idf = entity as IEntity;
            //if (idf != null && idf.Id == 0)
            //    Add(entity);
        }

        #endregion IWorkspace Members

        /// <summary>
        ///     Queryables this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <remarks></remarks>
        protected IQueryable<T> QueryInternal<T>() where T : class
        {
            if (IsReadOnly)
            {
                // Option 1
                _context.Configuration.AutoDetectChangesEnabled = false;
                _context.Configuration.LazyLoadingEnabled = false;
                _context.Configuration.ProxyCreationEnabled = false;

                // Option 2
                //ObjectContext objectContext = ObjContext;
                //objectContext.ContextOptions.ProxyCreationEnabled = false;
                //objectContext.ContextOptions.LazyLoadingEnabled = false;
                //ObjectSet<T> result = objectContext.CreateObjectSet<T>();
                //result.MergeOption = MergeOption.NoTracking;
                //return result;
            }

            DbQuery<T> set = _context.Set<T>();
            return !IsReadOnly ? set : set.AsNoTracking();
        }

        /// <summary>
        ///     Cheches if this instance is not in read only mode.
        /// </summary>
        /// <remarks></remarks>
        private void ChechIsNotReadOnly()
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Resources.NotAllowInReadOnlyMode);
        }

        /// <summary>
        ///  Execute SP and return the entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="StoreProcedure"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public List<T> ExecuteProcedure<T>(string StoreProcedure, params SqlParameter[] args)
        {
            return _context.Database.SqlQuery<T>("Exec " + StoreProcedure, args).ToList();
        }
    }
}
