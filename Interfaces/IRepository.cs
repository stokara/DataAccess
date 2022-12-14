#nullable enable
using System.Collections.ObjectModel;

namespace DataAccess.Interfaces; 

public interface IRepository {




    Task<int> GetFilteredCountAsync(string filter, object values);
    Task<int> GetCountAsync();
}
public interface IRepository<T> : IRepository where T : class {
    Task<T> GetByIdAsync(int id);
    Task<T> GetOneAsync(string filter, object? values);
    Task<ReadOnlyCollection<T>> GetAllAsync(string? filter = null, object? values = null, int limit = 0);
}