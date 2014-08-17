using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EXPLOSION_HUB.Server.Service
{
	public interface IServiceBase<TEntity>
	{
		TEntity Get(int id);

		IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
			string includeProperties = "");

		IEnumerable<TEntity> GetPaged(PageProperty pageProperty,
			Expression<Func<TEntity, bool>> filter = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
			string includeProperties = "");

		bool Delete(TEntity entity);

		int Create(TEntity entity);

		int Update(TEntity entity);
	}
}
