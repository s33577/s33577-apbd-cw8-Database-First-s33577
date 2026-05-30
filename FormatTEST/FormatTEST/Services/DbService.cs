namespace FormatTEST.Services;

public class DbService : IDbService
{
    private readonly string _connectionString;
    public DbService(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection") ?? string.Empty;
    }
    
    
}