using System;
using System.Net;
using System.Text.RegularExpressions;
using Npgsql;

namespace ClassLibraryDel
{
    public class DBHelper
    {
        public static NpgsqlConnection GetConnection()
        {
            // Allow overriding the connection string via an environment variable so the host
            // (and other credentials) can be fixed without recompiling.
            // Set the environment variable SUPABASE_DB_CONNECTION to a full Npgsql connection string.
            var env = Environment.GetEnvironmentVariable("SUPABASE_DB_CONNECTION");
            if (string.IsNullOrWhiteSpace(env))
            {
                // Fail fast and require the connection string to be provided via environment
                // variable so credentials are not stored in source control.
                throw new InvalidOperationException(
                    "Database connection string not configured. Set the SUPABASE_DB_CONNECTION environment variable to a valid Npgsql connection string before starting the application.");
            }

            var connStr = env;

            // Perform a quick DNS check for the Host value so failures are reported clearly.
            var m = Regex.Match(connStr, @"Host\s*=\s*([^;]+)", RegexOptions.IgnoreCase);
            if (m.Success)
            {
                var host = m.Groups[1].Value;
                try
                {
                    // This will throw if the host cannot be resolved.
                    Dns.GetHostAddresses(host);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Unable to resolve database host '{host}'. Check SUPABASE_DB_CONNECTION value and network/DNS.", ex);
                }
            }

            return new NpgsqlConnection(connStr);
        }
    }
}
