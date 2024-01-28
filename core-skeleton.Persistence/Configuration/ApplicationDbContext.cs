using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace core_skeleton.Persistence.Configuration;

public class ApplicationDbContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connStr;

    public ApplicationDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connStr = configuration.GetConnectionString("connStr")!;
    }

    /// <summary>
    /// Create connection
    /// </summary>
    /// <returns></returns>
    public IDbConnection CreateConnection() => new SqlConnection(_connStr);
}
