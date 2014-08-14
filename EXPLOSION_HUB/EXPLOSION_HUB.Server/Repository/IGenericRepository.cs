using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace EXPLOSION_HUB.Server.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        IEnumerable<TEntity> Get(
            string orderByColumn, string direction,
            Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = "");

        IEnumerable<TEntity> GetPaged(int page, int rows, out int totalPages, out int totalRecords,
                                                     string orderByColumn, string direction,
                                                     Expression<Func<TEntity, bool>> filter = null,
                                                     string includeProperties = "");

        IEnumerable<TEntity> GetPaged(int page, int rows,
                                        string orderByColumn, string direction,
                                        Expression<Func<TEntity, bool>> filter = null,
                                        string includeProperties = "");

        TEntity GetByID(object id);

        void Insert(TEntity entity);

        void Delete(object id);

        void Delete(TEntity entityToDelete);

        void Update(TEntity entityToUpdate);
    }
}
