using SQLite;

public class LocationDatabase
{
    readonly SQLiteAsyncConnection database;

    public LocationDatabase(string dbPath)
    {
        database = new SQLiteAsyncConnection(dbPath);
        database.CreateTableAsync<Location>().Wait();
    }

    public Task<List<Location>> GetLocationsAsync()
    {
        return database.Table<Location>().ToListAsync();
    }

    public Task<int> SaveLocationAsync(Location location)
    {
        return database.InsertAsync(location);
    }
}