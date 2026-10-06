using ClassLibraryDel; 
using ClassLibraryDel.DataAccess.Interfaces;
using ClassLibraryModel;
using ClassLibraryModel.Models;
using Npgsql;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
namespace DataAccessLayer
{
    public class SupabaseProductRepo : IProductRepository
    {
        // ---------- CREATE ----------
        public void Add(ProductModel product)
        {
            const string query = @"INSERT INTO ""Products"" (""Name"", ""Price"", ""Category"")
VALUES (@Name, @Price, @Category)";
            using (NpgsqlConnection con = DBHelper.GetConnection())
            {
                // Log masked connection string for debugging (do not log passwords)
                try
                {
                    var masked = Regex.Replace(con.ConnectionString ?? string.Empty, "(Password\\s*=\\s*)([^;]+)", "$1****", RegexOptions.IgnoreCase);
                    Console.WriteLine($"[DEBUG] DB Connection: {masked}");
                }
                catch { }

                con.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("Name", product.Name);
                    cmd.Parameters.AddWithValue("Price", product.Price);

                
cmd.Parameters.AddWithValue("Category", (object?)product.Category ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        // ---------- READ (all) ----------
        public List<ProductModel> GetAll()
        {
            List<ProductModel> products = new List<ProductModel>();
            const string query = @"SELECT ""Id"", ""Name"", ""Price"", ""Category""
FROM ""Products""
ORDER BY ""Id""";
            using (NpgsqlConnection con = DBHelper.GetConnection())
            {
                con.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, con))
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(MapProduct(reader));
                    }
                }
            }
            return products;
        }
        // ---------- READ (by id) ----------
        public ProductModel? GetById(Guid id)
        {
            const string query = @"SELECT ""Id"", ""Name"", ""Price"", ""Category""
FROM ""Products""
WHERE ""Id"" = @Id";
            using (NpgsqlConnection con = DBHelper.GetConnection())
            {
                con.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("Id", id);
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        return reader.Read() ? MapProduct(reader) : null;
                    }
                }
            }
        }
        // ---------- UPDATE ----------
        public void Update(ProductModel product)
        {
            const string query = @"UPDATE ""Products""
SET ""Name"" = @Name,
""Price"" = @Price,
""Category"" = @Category
WHERE ""Id"" = @Id";
            using (NpgsqlConnection con = DBHelper.GetConnection())
            {
                con.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("Id", product.Id);
                    cmd.Parameters.AddWithValue("Name", product.Name);

               
                cmd.Parameters.AddWithValue("Price", product.Price);
                    cmd.Parameters.AddWithValue("Category", (object?)product.Category ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        // ---------- DELETE ----------
        public void Delete(Guid id)
        {
            const string query = @"DELETE FROM ""Products"" WHERE ""Id"" = @Id";
            using (NpgsqlConnection con = DBHelper.GetConnection())
            {
                con.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        // ---------- Helper: maps a reader row to a ProductModel ----------
        private static ProductModel MapProduct(NpgsqlDataReader reader)
        {
            return new ProductModel
            {
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                Category = reader.IsDBNull(reader.GetOrdinal("Category"))
            ? string.Empty
            : reader.GetString(reader.GetOrdinal("Category"))
            };
        }
    }
}
