using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Repository.Infrastructure
{
    public abstract class DapperDBContext : IContext
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private int? _commandTimeout = null;
        private readonly DapperDBContextOptions _options;

        public bool IsTransactionStarted { get; private set; }

        protected abstract IDbConnection CreateConnection(string connectionString);

        protected DapperDBContext(IOptions<DapperDBContextOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;

            _connection = CreateConnection(_options.Configuration);
            _connection.Open();

            DebugPrint("Connection started.");
        }

        #region Transaction

        public void BeginTransaction()
        {
            if (IsTransactionStarted)
                throw new InvalidOperationException("Transaction is already started.");

            _transaction = _connection.BeginTransaction();
            IsTransactionStarted = true;

            DebugPrint("Transaction started.");
        }

        public void Commit()
        {
            if (!IsTransactionStarted)
                throw new InvalidOperationException("No transaction started.");

            _transaction.Commit();
            _transaction = null;

            IsTransactionStarted = false;

            DebugPrint("Transaction committed.");
        }

        public void Rollback()
        {
            if (!IsTransactionStarted)
                throw new InvalidOperationException("No transaction started.");

            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;

            IsTransactionStarted = false;

            DebugPrint("Transaction rollbacked and disposed.");
        }

        #endregion Transaction

        #region Dapper Execute & Query
        public async Task<TKey> InsertAsync<TKey, TEntity>(TEntity entityToInsert)
        {
            return await _connection.InsertAsync<TKey, TEntity>(entityToInsert, _transaction, _commandTimeout);
        }

        public async Task<int?> InsertAsync<TEntity>(TEntity entityToInsert)
        {
            return await _connection.InsertAsync(entityToInsert, _transaction, _commandTimeout);
        }

        public async Task<int> DeleteAsync<T>(object id)
        {
            return await _connection.DeleteAsync(id, _transaction, _commandTimeout);
        }

        public async Task<int> DeleteAsync<T>(T entityToDelete)
        {
            return await _connection.DeleteAsync(entityToDelete, _transaction, _commandTimeout);
        }

        public async Task<int> UpdateAsync<TEntity>(TEntity entityToUpdate)
        {
            return await _connection.UpdateAsync(entityToUpdate, _transaction, _commandTimeout);
        }

        public async Task<T> GetAsync<T>(object id)
        {
            return await _connection.GetAsync<T>(id, _transaction, _commandTimeout);
        }

        public async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null)
        {
            return await _connection.QuerySingleOrDefaultAsync<T>(sql, param, _transaction, _commandTimeout);
        }

        public async Task<IEnumerable<T>> GetListPagedAsync<T>(int pageNumber, int rowsPerPage, string conditions, string orderby, object parameters = null)
        {
            return await _connection.GetListPagedAsync<T>(pageNumber, rowsPerPage, conditions, orderby, parameters, _transaction, _commandTimeout);
        }

        public async Task<int> RecordCountAsync<T>(string conditions = "", object parameters = null)
        {
            return await _connection.RecordCountAsync<T>(conditions, parameters, _transaction, _commandTimeout);
        }

        public async Task<int> RecordCountAsync<T>(object whereConditions)
        {
            return await _connection.RecordCountAsync<T>(whereConditions, _transaction, _commandTimeout);
        }
        #endregion Dapper Execute & Query

        public void Dispose()
        {
            if (IsTransactionStarted)
                Rollback();

            _connection.Close();
            _connection.Dispose();
            _connection = null;

            DebugPrint("Connection closed and disposed.");
        }

        private void DebugPrint(string message)
        {
#if DEBUG
            Debug.Print(">>> UnitOfWorkWithDapper - Thread {0}: {1}", Thread.CurrentThread.ManagedThreadId, message);
#endif
        }
    }
}
