using Core.Common;
using Core.Entity;
using EasyCaching.Core.Interceptor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.IRepository
{
    public interface IStudentRepository
    {
        Task<bool> Add(Student entity);

        [EasyCachingEvict(IsBefore = true, CacheKeyPrefix = "AspectCore")]
        Task<bool> Delete(Student entity);

        [EasyCachingPut(CacheKeyPrefix = "AspectCore")]
        Task<bool> Update(Student entity);

        [EasyCachingAble(Expiration = 10 * 60, CacheKeyPrefix = "AspectCore")]
        Task<Student> Get(long Id);

        Task<int> GetCount(QueryRequestByPage reqMsg);
        Task<IEnumerable<Student>> GetListPaged(QueryRequestByPage reqMsg);
    }
}
