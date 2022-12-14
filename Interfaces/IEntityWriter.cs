namespace DataAccess.Interfaces; 

public interface IEntityWriter<T> {
    static (bool saved, string failMessage) Persist(T entity) => (false, "");
}