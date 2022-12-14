using System.Collections.ObjectModel;
using Dapper;

namespace DataAccess;

public class Reader<T> : IReader<T> where T : class {
    protected readonly DbConnectionManager dbConnectionService;
    private readonly SqlBuilder sqlBuilder;

    public Reader(DbConnectionManager dbConnectionService, DatabaseMapper databaseMapper) {
        this.dbConnectionService = dbConnectionService;
        var tableInfo = databaseMapper.GetTableInfo<T>();
        sqlBuilder = new SqlBuilder(tableInfo);
    }
    
    public virtual async Task<ReadOnlyCollection<T>> GetAllAsync(string where = "", object? args = null, int pageSize = 0, int pageNum = 1, string orderBy = "") {
        var sql = sqlBuilder.GetSelectSql(where, pageSize, pageNum-1);
        using var conn = dbConnectionService.CreateConnection();
        var result = await conn.QueryAsync<T>(sql, args).ConfigureAwait(false);
        return result.ToList().AsReadOnly();
    }

    public virtual async Task<T> GetByIdAsync(int id) {
        var result = await GetAllAsync("Id=@id", new { id }).ConfigureAwait(false);
        if (result.Count != 1) throw new KeyNotFoundException($"{result.Count} rows returned for Id={id}.");
        return result.Single();
    }

    public virtual async Task<T?> TryGetByIdAsync(int id) {
        var rows = await GetAllAsync("Id=@id", new { id }).ConfigureAwait(false);
        return rows.Count != 1 ? null : rows.Single();
    }

    public virtual async Task<T> GetOneAsync(string filter, object? values) {
        var result = await GetAllAsync(filter, values).ConfigureAwait(false);
        if (result.Count != 1) throw new InvalidDataException($"{result.Count} rows returned for {ObjectDumper.Dump(filter)}");
        return result.Single();
    }

    public virtual async Task<T?> TryGetOneAsync(string filter, object? values) {
        var rows = await GetAllAsync(filter, values).ConfigureAwait(false);
        return rows.Count != 1 ? null : rows.Single();
    }

    public virtual async Task<int> GetCountAsync() {
        using var conn = dbConnectionService.CreateConnection();
        return await conn.ExecuteScalarAsync<int>(sqlBuilder.GetCountSql());
    }

    public virtual  async Task<int> GetFilteredCountAsync(string filter, object? values) {
        using var conn = dbConnectionService.CreateConnection();
        return await conn.ExecuteScalarAsync<int>(sqlBuilder.GetCountSql(filter), values );
    }
}