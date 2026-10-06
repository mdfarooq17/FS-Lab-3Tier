using System;
using System.Collections.Generic;
using System.Text;
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
            if (!string.IsNullOrWhiteSpace(env))
            {
                return new NpgsqlConnection(env);
            }

            // Fallback connection (kept for backward compatibility). If DNS resolution fails
            // for the host here, set SUPABASE_DB_CONNECTION to a correct value or check network/DNS.
            return new NpgsqlConnection("Host=db.awpfseynateigdbdhqik.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=Farooq17supa;SSL Mode=Require;Trust Server Certificate=true");
        }
    }
}