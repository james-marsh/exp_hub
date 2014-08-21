//This repository implementation has basic CRUD functionalities.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Linq.Expressions;

namespace EXPLOSION_HUB.Server.Repository
{
    public class GenericRepository<TEntity, TDbContext> : IGenericRepository<TEntity>, IDisposable
        where TEntity : class
        where TDbContext : DbContext, new()
    {
        private TDbContext dbContext = null;
        private DbSet<TEntity> dbSet = null;

		private bool isDisposed = false;

		public GenericRepository()
		{
			this.dbContext = new TDbContext();
			this.dbSet = this.dbContext.Set<TEntity>();
		}

		public GenericRepository(TDbContext dbContext)
		{
			this.dbContext = dbContext;
			this.dbSet = this.dbContext.Set<TEntity>();
		}

        private void Filter(IQueryable<TEntity> query, Expression<Func<TEntity, bool>> filter = null)
        {
            if (filter != null)
                query = query.Where(filter);
        }

        private void IncludeProperties(IQueryable<TEntity> query, string includeProperties = "")
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

        private IEnumerable<TEntity> PagedData(IQueryable<TEntity> query, PageProperty pageProperty)
        {
	        pageProperty.TotalRecords = query.Count();
			pageProperty.TotalPages = (int)Math.Ceiling(pageProperty.TotalRecords / (float)pageProperty.Rows);
			return query.Skip((pageProperty.Page - 1) * pageProperty.Rows).Take(pageProperty.Rows);
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            Filter(query, filter);

            IncludeProperties(query, includeProperties);

            return OrderBy(query, orderBy);
        }

        /// <summary>
        /// To be used when paging is involved.
        /// </summary>
		public virtual IEnumerable<TEntity> GetPaged(PageProperty pageProperty,
                                                     Expression<Func<TEntity, bool>> filter = null,
                                                     string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;
            Filter(query, filter);
            IncludeProperties(query, includeProperties);
			var orderedData = OrderBy(query, pageProperty.OrderByColumn, pageProperty.SortDirection);
			return PagedData(orderedData, pageProperty);
        }

        public virtual TEntity GetById(object id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {   
            dbSet.Add(entity);
        }

        public virtual void Update(TEntity entityToUpdate)
        {  
            dbSet.Attach(entityToUpdate);
            this.dbContext.Entry(entityToUpdate).State = EntityState.Modified;
        }

		public virtual int Save()
		{
			return this.dbContext.SaveChanges();
		}

		
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if(!isDisposed && disposing)
			{
				this.dbContext.Dispose();
			}

			isDisposed = true;
		}
	}
}