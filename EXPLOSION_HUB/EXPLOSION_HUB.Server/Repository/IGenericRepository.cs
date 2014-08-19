using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EXPLOSION_HUB.Server.Repository
{
	public interface IGenericRepository<TEntity>
		where TEntity : class
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
