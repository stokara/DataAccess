using System.Data;
using DataAccess.Interfaces;

namespace DataAccess;

public class EntityWriter<T> : IEntityWriter<T> {
    public readonly IDbConnection dbConn;
    public readonly IDbTransaction dbTransaction;

    public EntityWriter() {
        dbConn = ConnectionManager.CreateConnection();
        dbTransaction = dbConn.BeginTransaction();
    }

}