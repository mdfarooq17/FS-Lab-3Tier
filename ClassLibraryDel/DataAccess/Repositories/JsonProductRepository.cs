using System.Text.Json;
using ClassLibraryDel.DataAccess.Interfaces;
using ClassLibraryModel.Models;
namespace ClassLibraryDel.DataAccess.Repositories;

public class JsonProductRepository : IProductRepository
{
    private readonly string _filePath;
    private readonly object _fileLock = new();

    public JsonProductRepository()
    {
        var contentRoot = Directory.GetCurrentDirectory();
        var dataFolder = Path.Combine(contentRoot, "DataAccess", "Data");
        Directory.CreateDirectory(dataFolder);
        _filePath = Path.Combine(dataFolder, "products.json");
        if (!File.Exists(_filePath))
        {
            var seedProducts = new List<ProductModel>
            {
                new ProductModel { Id = Guid.NewGuid(), Name = "Laptop", Price = 120000, Category = "Electronics" },
                new ProductModel { Id = Guid.NewGuid(), Name = "Keyboard", Price = 5000, Category = "Accessories" },
                new ProductModel { Id = Guid.NewGuid(), Name = "Mouse", Price = 2500, Category = "Accessories" }
            };
            WriteToFile(seedProducts);
        }
    }

    private List<ProductModel> ReadFromFile()
    {
        lock (_fileLock)
        {
            if (!File.Exists(_filePath))
                return new List<ProductModel>();
            var json = File.ReadAllText(_filePath);
            if (string.IsNullOrWhiteSpace(json))
                return new List<ProductModel>();
            return JsonSerializer.Deserialize<List<ProductModel>>(json) ?? new List<ProductModel>();
        }
    }

    private void WriteToFile(List<ProductModel> products)
    {
        lock (_fileLock)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(products, options);
            File.WriteAllText(_filePath, json);
        }
    }

    public List<ProductModel> GetAll() => ReadFromFile();

    public ProductModel? GetById(Guid id) => ReadFromFile().FirstOrDefault(x => x.Id == id);

    public void Add(ProductModel product)
    {
        var products = ReadFromFile();
        products.Add(product);
        WriteToFile(products);
    }

    public void Update(ProductModel product)
    {
        var products = ReadFromFile();
        var existing = products.FirstOrDefault(x => x.Id == product.Id);
        if (existing == null) return;
        existing.Name = product.Name;
        existing.Price = product.Price;
        existing.Category = product.Category;
        WriteToFile(products);
    }

    public void Delete(Guid id)
    {
        var products = ReadFromFile();
        var existing = products.FirstOrDefault(x => x.Id == id);
        if (existing == null) return;
        products.Remove(existing);
        WriteToFile(products);
    }
}
