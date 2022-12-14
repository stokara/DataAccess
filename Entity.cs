using DataAccess.Interfaces;

namespace DataAccess; 


public class Entity<T> : IEntity<T> where T : class {
    // ReSharper disable once InconsistentNaming
    private static IRepository<T> repository { get; }
    public IRepository<T> Repository => repository;

    static Entity() {
        var repositoryType = typeof(IRepository<T>);
        if (Activator.CreateInstance(repositoryType) is not IRepository<T> repo) throw new InvalidOperationException();
        repository = repo;
    }
}