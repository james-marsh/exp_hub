using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data.Entity;

namespace EXPLOSION_HUB.Server.Repository
{
	public interface IGenericRepository<TEntity, TDbContext>
		where TEntity : class
		where TDbContext : DbContext, new()
    {

        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        IEnumerable<TEntity> GetPaged(PageProperty pageProperty,
			Expression<Func<TEntity, bool>> filter = null,
			string includeProperties = "");

        TEntity GetById(object id);

        void Insert(TEntity entity);
		
        void Update(TEntity entity);

		int Save();
    }
}
