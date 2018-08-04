using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.SqlClient;
using Rinku.Domain;
using Rinku.Contracts;

namespace Rinku.Data
{
    public class EfRepositoryBase<T> : IRepository<T>
            where T : class, IEntity, new()
    {
        private readonly IWorkspace _workspace;

        /// <summary>
        /// Initializes a new instance of the <see cref="EfRepositoryBase&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="workspace">The workspace.</param>
        /// <remarks></remarks>
        public EfRepositoryBase(IWorkspace workspace)
        {
            if (workspace == null)
                throw new ArgumentNullException("workspace");

            _workspace = workspace;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is read only.
        /// </summary>
        /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public virtual bool IsReadOnly
        {
            get { return _workspace.IsReadOnly; }
            set { _workspace.IsReadOnly = value; }
        }

        /// <summary>
        /// Gets the workspace object used by this repository.
        /// </summary>
        /// <remarks></remarks>
        public IWorkspace Workspace
        {
            get { return _workspace; }
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <remarks></remarks>
        public virtual T Add(T entity)
        {
            return _workspace.Add(entity);
        }

        public virtual IEnumerable<T> Add(IEnumerable<T> entities)
        {
            return _workspace.Add(entities);
        }

        /// <summary>
        /// Attaches the given entity to the context.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <remarks>It is used to repopulate a context with an entity that is known to already exist in the database.</remarks>
        public virtual T Attach(T entity)
        {
            return _workspace.Attach(entity);
        }

        public virtual void Delete(int id)
        {
            var found = Get(id);
            if (found != null)
            {
                Delete(found);
            }
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <remarks></remarks>
        public virtual void Delete(T entity)
        {
            _workspace.Delete(entity);
        }

        public virtual bool Exists(int id)
        {
            return _workspace.Query<T>(x => x.Id == id).Any();
        }

        /// <summary>
        /// Gets the entity by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual T Get(int id)
        {
            return _workspace.Single<T>(x => x.Id == id);
        }

        /// <summary>
        /// Returns all the entities of type TModel.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual IEnumerable<T> GetAll()
        {
            return _workspace.Query<T>().AsEnumerable();
        }

        public virtual IEnumerable<T> GetAll(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes)
        {
            return _workspace.Query<T>(expression, includes);
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual int GetCount()
        {
            return _workspace.Count<T>();
        }

        /// <summary>
        /// Saves (commits) the changes to the data store.
        /// </summary>
        /// <remarks></remarks>
        public virtual int SaveChanges()
        {
            return _workspace.SaveChanges();
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <remarks></remarks>
        public virtual T Update(T entity)
        {
            return _workspace.Update(entity);
        }


        public List<T> ExecuteProcedure<T>(string StoreProcedure, params SqlParameter[] args)
        {
            return _workspace.ExecuteProcedure<T>(StoreProcedure, args);
        }

        public virtual T Single<T>(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes) where T : class
        {
            return _workspace.Single<T>(expression, includes);
        }

        public virtual IQueryable<TResult> Select<TSource, TResult>(Expression<Func<TSource, TResult>> expression,
                                                                    Expression<Func<TSource, bool>> predicate)
            where TSource : class
        {
            return _workspace.Select<TSource, TResult>(expression, predicate);
        }

    }
}
