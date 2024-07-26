using System.Data;
using Autofac;
using Microsoft.Data.SqlClient;
using Para.Data.UnitOfWork;

namespace Para.Api.AutoFac;

public class AutoFac : Module
{
    private readonly IConfiguration _configuration;

    public AutoFac(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void Load(ContainerBuilder builder)
    {
        // Register UnitOfWork with IUnitOfWork
        builder.RegisterType<UnitOfWork>()
            .As<IUnitOfWork>()
            .InstancePerLifetimeScope();

        // Retrieve connection string from configuration
        var connectionString = _configuration.GetConnectionString("MsSqlConnection");

        // Register SqlConnection with IDbConnection
        builder.Register(c => 
            {
                var connection = new SqlConnection(connectionString);
                connection.Open(); // Open the connection immediately if needed
                return connection;
            })
            .As<IDbConnection>()
            .InstancePerLifetimeScope();
    }
}