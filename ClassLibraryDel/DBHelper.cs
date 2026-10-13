using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Text;
namespace DataAccessLayer
{
    public class DBHelper
    {
        private static string GetConnectionString()
        {
            // Allow overriding the connection string via environment variables
            // (e.g. ConnectionStrings__DefaultConnection) so credentials can be
            // supplied at runtime without editing appsettings.json.
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
            .Build();

            return configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "DefaultConnection is missing from appsettings.json");
        }

        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(GetConnectionString());
        }
    }
}

