using System.Data;
using Dapper;
using DataAccess.Interfaces;

namespace DataAccess; 

public sealed class UnitOfWork : IDisposable {
    private readonly IDbConnection dbConn;
    private readonly IDbTransaction dbTransaction;
    public UnitOfWork() {
        dbConn = ConnectionManager.CreateConnection();
        dbTransaction = dbConn.BeginTransaction();
    }

    public Task InsertAsync(string sql, object? param = null) => dbConn.ExecuteScalarAsync<int>(sql, param, dbTransaction);
    public Task<int> InsertAsync<T>(T entity, string insertSql, string? sequenceName = null) {
        var nextSeqStmt = getSequenceNextValueStatement();
        var sql = $"DECLARE @NextId INT;SELECT @NextId={nextSeqStmt};{insertSql};SELECT @NextId";
        return dbConn.ExecuteScalarAsync<int>(sql, entity, dbTransaction);

        string getSequenceNextValueStatement() => $"NEXT VALUE FOR {sequenceName ?? typeof(T).Name}_id_seq";
    }
    //public Task<int> InsertAsync<T>(Entity<T> entity) where T : class, IEntity<T> => InsertAsync(entity, entity.Repository.InsertSql, entity.Repository.SequenceName);

    //public Task InsertManyAsync<T>(IEnumerable<T> entities, string insertSql) => dbConn.ExecuteScalarAsync<int>(insertSql, entities, dbTransaction);

    //public Task UpdateAsync(string sql, object? param = null) => dbConn.ExecuteAsync(sql, param, dbTransaction);
    //public Task UpdateAsync<T>(Entity<T> entity) where T : class => UpdateAsync(entity.Repository.UpdateSql, entity);

    //public Task UpdateManyAsync(IEnumerable<object> values, string updateSql) => dbConn.ExecuteAsync(updateSql, values, dbTransaction);

    //public Task DeleteAsync<T>(Entity<T> entity) where T : class => dbConn.ExecuteAsync(entity.Repository.DeleteSql,
    //    new {entity.Repository.TableName, entity.Repository.PrimaryKeyColumnName}, dbTransaction);

    //public Task ExecuteAsync(string sql, object? entity = null) => dbConn.ExecuteAsync(sql, entity, dbTransaction);

    public void Commit() => dbTransaction.Commit();
    public void Rollback() => dbTransaction.Rollback();

    public void Dispose() {
        dbConn.Dispose();
        dbTransaction.Dispose();
    }
}