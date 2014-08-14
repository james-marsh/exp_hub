//This repository implementation has basic CRUD functionalities.
//Repository is not responsible of disposing the DbContext. Its the Service which has this responsibility.
//Update: 7/28/2013: Actually based on design of DbContext, developer dont have to worry about disposing it.
//DbContext handles the opening and closing of connection. It closes the connection as its finished retrieving the results from query.
//But it is still good practice to call Dispose.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;
using System.Linq.Expressions;
using System.Data;

namespace EXPLOSION_HUB.Server.Repository
{
    public class GenericRepository<TEntity, VDbContext> : IGenericRepository<TEntity>
        where TEntity : class
        where VDbContext : DbContext
    {
        private VDbContext dbContext = null;
        private DbSet<TEntity> dbSet = null;

        public VDbContext Context { 
			set { 
					this.dbContext = value; 
			} 
		}
        public DbSet<TEntity> DbSet { 
			set { 
					this.dbSet = value; 
			} 
		} 

        private void Filter(ref IQueryable<TEntity> query, Expression<Func<TEntity, bool>> filter = null)
        {
            if (filter != null)
                query = query.Where(filter);
        }

        private void IncludeProperties(ref IQueryable<TEntity> query, string includeProperties = "")
        {
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        private IQueryable<TEntity> OrderBy(IQueryable<TEntity> query, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

        private IQueryable<TEntity> OrderBy(IQueryable<TEntity> query, string orderColumn, string direction)
        {
            var type = typeof(TEntity);
            var property = type.GetProperty(orderColumn);
            if (property == null)
                return query;

            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = null;
            if (string.IsNullOrEmpty(direction.Trim()) || direction.ToLower() == "asc")
                resultExp = Expression.Call(typeof(Queryable), "OrderBy", new Type[] { type, property.PropertyType },
                query.Expression, Expression.Quote(orderByExpression));
            else
                resultExp = Expression.Call(typeof(Queryable), "OrderByDescending", new Type[] { type, property.PropertyType },
                query.Expression, Expression.Quote(orderByExpression));

            return query.Provider.CreateQuery<TEntity>(resultExp);
        }

        private IQueryable<TEntity> PagedData(IQueryable<TEntity> query, int page, int row, out int totalPages, out int totalRecords)
        {
            totalPages = 0;
            totalRecords = 0;
			totalRecords = query.Count();
			totalPages = (int)Math.Ceiling(totalRecords / (float)row);
            return query.Skip((page - 1) * row).Take(row);
        }

        private IQueryable<TEntity> PagedData(IQueryable<TEntity> query, int page, int row)
        {   
            return query.Skip((page - 1) * row).Take(row);
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            Filter(ref query, filter);

            IncludeProperties(ref query, includeProperties);

            return OrderBy(query, orderBy);
        }

        public virtual IEnumerable<TEntity> Get(
            string orderByColumn, string direction,
            Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            Filter(ref query, filter);

            IncludeProperties(ref query, includeProperties);

            return OrderBy(query, orderByColumn, direction);
        }

        public virtual IEnumerable<TEntity> GetPaged(int page, int rows, out int totalPages, out int totalRecords,
                                                     string orderByColumn, string direction,
                                                     Expression<Func<TEntity, bool>> filter = null,
                                                     string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;
            Filter(ref query, filter);
            IncludeProperties(ref query, includeProperties);
            var orderedData = OrderBy(query, orderByColumn, direction);
            return PagedData(orderedData, page, rows, out totalPages, out totalRecords);
        }

        public virtual IEnumerable<TEntity> GetPaged(int page, int rows,
                                                     string orderByColumn, string direction,
                                                     Expression<Func<TEntity, bool>> filter = null,
                                                     string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;
            Filter(ref query, filter);
            IncludeProperties(ref query, includeProperties);
            var orderedData = OrderBy(query, orderByColumn, direction);
            return PagedData(orderedData, page, rows);
        }

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {   
            dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (this.dbContext.Entry(entityToDelete).State == EntityState.Detached)
            {   
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {  
            dbSet.Attach(entityToUpdate);
            this.dbContext.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}