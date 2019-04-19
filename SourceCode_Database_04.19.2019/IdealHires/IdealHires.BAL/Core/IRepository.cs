using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace IdealHires.BAL.Core
{
    public interface IRepository<TEntity> where TEntity : class
    {
        ///<summary>
        ///<para>Gets a Record By Id</para>
        ///<para>Must only be used for tables with Integer Identity</para>
        ///</summary>
        TEntity Get(int id);

        ///<summary>
        ///<para>Gets a Record By Id</para>
        ///<para>Must only be used for tables with string Identity</para>
        ///</summary>
        TEntity Get(string id);

        ///<summary>
        ///<para>Gets a Record By Id</para>
        ///<para>Must only be used for tables with Guid Identity</para>
        ///</summary>
        TEntity Get(Guid id);

        /// <summary>
        /// Get all entities from db
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        List<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Get query for entity
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        /// <summary>
        /// Get first or default entity by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        TEntity GetFirstOrDefault(
            Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Update entity in db
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);

        ///<summary>
        ///<para>Gets All Record</para>
        ///</summary>
        IEnumerable<TEntity> GetAll();

        ///<summary>
        ///<para>Finds Record By a predicate</para>
        ///</summary>
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        ///<summary>
        ///<para>Gets SingleOrDefault Record By a predicate</para>
        ///</summary>
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        ///<summary>
        ///<para>Adds a Record</para>
        ///<para>All required properties must be provided</para>
        ///</summary>
        void Add(TEntity entity);

        ///<summary>
        ///<para>Adds Multiple Record</para>
        ///<para>All required properties must be provided</para>
        ///</summary>
        void AddRange(IEnumerable<TEntity> entities);

        ///<summary>
        ///<para>Removes a Record</para>
        ///<para>Record must be loaded from database first</para>
        ///</summary>
        void Remove(TEntity entity);

        ///<summary>
        ///<para>Removes multiple Record</para>
        ///<para>Records must be loaded from database first</para>
        ///</summary>
        void RemoveRange(IEnumerable<TEntity> entities);

    }
}
