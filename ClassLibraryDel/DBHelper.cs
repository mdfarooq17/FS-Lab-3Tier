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
            var connStr = !string.IsNullOrWhiteSpace(env)
                ? env
                : "Host=db.awpfseynateigdbdhqik.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=Farooq17supa;SSL Mode=Require;Trust Server Certificate=true";

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
