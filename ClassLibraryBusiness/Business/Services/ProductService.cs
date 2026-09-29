using ClassLibraryBusiness.Business.Interfaces;
using ClassLibraryDel.DataAccess.Interfaces;
using ClassLibraryModel.Models;

namespace ClassLibraryBusiness.Business.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public List<ProductModel> GetAll() => _productRepository.GetAll();

    public ProductModel? GetById(Guid id) => _productRepository.GetById(id);

    public void Create(ProductModel product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
            throw new ArgumentException("Product name is required.");
        if (product.Price <= 0)
            throw new ArgumentException("Product price must be greater than zero.");
        if (string.IsNullOrWhiteSpace(product.Category))
            throw new ArgumentException("Category is required.");
        product.Id = Guid.NewGuid();
        _productRepository.Add(product);
    }

    public void Update(ProductModel product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
            throw new ArgumentException("Product name is required.");
        if (product.Price <= 0)
            throw new ArgumentException("Product price must be greater than zero.");
        _productRepository.Update(product);
    }

    public void Delete(Guid id) => _productRepository.Delete(id);
}
