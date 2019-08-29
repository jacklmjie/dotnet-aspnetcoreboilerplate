using Core.Common.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Repository
{
    /// <summary>
    /// 定义实体仓储模型的数据标准操作
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TKey">主键类型</typeparam>
    public interface IRepository<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
        #region Dapper Execute & Query
        Task<TKey> InsertAsync(TEntity entityToInsert);

        Task<int> DeleteAsync(TEntity entityToDelete);

        Task<int> UpdateAsync(TEntity entityToUpdate);

        Task<TEntity> GetAsync(object id);

        Task<IEnumerable<TEntity>> GetListPagedAsync(int pageNumber, int rowsPerPage, string conditions, string orderby, object parameters = null);

        Task<int> RecordCountAsync(string conditions = "", object parameters = null);

        Task<int> RecordCountAsync(object whereConditions);
        #endregion Dapper Execute & Query
    }
}
