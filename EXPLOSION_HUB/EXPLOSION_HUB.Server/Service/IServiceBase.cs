using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EXPLOSION_HUB.Server.Service
{
	public interface IServiceBase<TEntity> : IDisposable
	{
		IEnumerable<TEntity> GetAll();
		IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null);
		TEntity Get(int id);
		IEnumerable<TEntity> GetPaged(PageProperty pagePrope, Expression<Func<TEntity, bool>> filter = null);
	}
}
