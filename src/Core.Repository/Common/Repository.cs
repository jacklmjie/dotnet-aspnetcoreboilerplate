using Core.Common.Entity;
using Core.Common.Security.Claims;
using Core.Repository.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Core.Repository
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
       where TEntity : class, IEntity<TKey>
    {
        private readonly DapperDBContext _context;
        private readonly IPrincipal _principal;
        public Repository(DapperDBContext context,
            IPrincipal principal)
        {
            _context = context;
            _principal = principal;
        }

        #region Dapper Execute & Query
        public async Task<TKey> InsertAsync(TEntity entityToInsert)
        {
            CheckInsert(entityToInsert);
            return await _context.InsertAsync<TKey, TEntity>(entityToInsert);
        }

        public async Task<int> DeleteAsync(TEntity entityToDelete)
        {
            CheckDelete(entityToDelete);
            return await _context.DeleteAsync<TEntity>(entityToDelete);
        }

        public async Task<int> UpdateAsync(TEntity entityToUpdate)
        {
            CheckUpdate(entityToUpdate);
            return await _context.UpdateAsync<TEntity>(entityToUpdate);
        }

        public async Task<TEntity> GetAsync(object id)
        {
            return await _context.GetAsync<TEntity>(id);
        }

        public async Task<TEntity> QuerySingleOrDefaultAsync(string sql, object param = null)
        {
            return await _context.QuerySingleOrDefaultAsync<TEntity>(sql, param);
        }

        public async Task<IEnumerable<TEntity>> GetListPagedAsync(int pageNumber, int rowsPerPage, string conditions, string orderby, object parameters = null)
        {
            return await _context.GetListPagedAsync<TEntity>(pageNumber, rowsPerPage, conditions, orderby, parameters);
        }

        public async Task<int> RecordCountAsync(string conditions = "", object parameters = null)
        {
            return await _context.RecordCountAsync<TEntity>(conditions, parameters);
        }

        public async Task<int> RecordCountAsync(object whereConditions)
        {
            return await _context.RecordCountAsync<TEntity>(whereConditions);
        }
        #endregion Dapper Execute & Query

        #region private methods
        private void CheckInsert(TEntity entity)
        {
            entity.CheckICreatedTime<TEntity, TKey>();

            string userIdTypeName = _principal?.Identity.GetClaimValueFirstOrDefault("userIdTypeName");
            if (userIdTypeName == null)
            {
                return;
            }
            if (userIdTypeName == typeof(int).FullName)
            {
                entity.CheckICreationAudited<TEntity, TKey, int>(_principal);
            }
            else if (userIdTypeName == typeof(Guid).FullName)
            {
                entity.CheckICreationAudited<TEntity, TKey, Guid>(_principal);
            }
            else
            {
                entity.CheckICreationAudited<TEntity, TKey, long>(_principal);
            }
        }

        private void CheckUpdate(TEntity entity)
        {
            string userIdTypeName = _principal?.Identity.GetClaimValueFirstOrDefault("userIdTypeName");
            if (userIdTypeName == null)
            {
                return;
            }
            if (userIdTypeName == typeof(int).FullName)
            {
                entity.CheckIUpdateAudited<TEntity, TKey, int>(_principal);
            }
            else if (userIdTypeName == typeof(Guid).FullName)
            {
                entity.CheckIUpdateAudited<TEntity, TKey, Guid>(_principal);
            }
            else
            {
                entity.CheckIUpdateAudited<TEntity, TKey, long>(_principal);
            }
        }

        private void CheckDelete(TEntity entity)
        {
            string userIdTypeName = _principal?.Identity.GetClaimValueFirstOrDefault("userIdTypeName");
            if (userIdTypeName == null)
            {
                return;
            }
            if (userIdTypeName == typeof(int).FullName)
            {
                entity.CheckIDeleteAudited<TEntity, TKey, int>(_principal);
            }
            else if (userIdTypeName == typeof(Guid).FullName)
            {
                entity.CheckIDeleteAudited<TEntity, TKey, Guid>(_principal);
            }
            else
            {
                entity.CheckIDeleteAudited<TEntity, TKey, long>(_principal);
            }
        }
        #endregion
    }
}
