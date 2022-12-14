namespace DataAccess.Interfaces; 

public interface IEntity<T> where T : class {
    IRepository<T> Repository { get; }
}
