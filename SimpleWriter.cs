namespace DataAccess;

public class SimpleWriter : Writer {
    public SimpleWriter(DbConnectionManager dbConnection, DatabaseMapper databaseMapper) : base(new SimpleSingleEntitySaveStrategy(dbConnection, databaseMapper)) { }
}