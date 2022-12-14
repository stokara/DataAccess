namespace DataAccess;

public class DatabaseMapper {
    private readonly Dictionary<Type, ITableInfo> tableInfos = new();

    public DatabaseMapper(IEnumerable<ITableInfo>? config = null) {
        if (config is not null) LoadMap(config);
    }

    public void LoadMap(IEnumerable<ITableInfo>? config = null) {
        tableInfos.Clear();
        if (config is null) return;

        foreach (var entry in config) {
            tableInfos.Add(entry.EntityType, entry);
        }
    }

    public TableInfo<T> GetTableInfo<T>() {
        var type = typeof(T);
        if (tableInfos.ContainsKey(type)) return  (TableInfo<T>) tableInfos[type];
        var newTableInfo = new TableInfo<T>();
        tableInfos.Add(type,newTableInfo );
        return newTableInfo;
    }
    public ITableInfo GetTableInfo(Type entityType)
    {
        if (tableInfos.ContainsKey(entityType)) return tableInfos[entityType];
        var newTableInfo = Activator.CreateInstance(typeof(TableInfo<>).MakeGenericType(entityType));
        if (newTableInfo is null) throw new InvalidOperationException($"Could not create TableInfo<{entityType}>");
        tableInfos.Add(entityType, (ITableInfo)newTableInfo);
        return (ITableInfo)newTableInfo;
    }
}