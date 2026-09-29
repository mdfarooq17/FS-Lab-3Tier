using ClassLibraryModel.Models;

namespace ClassLibraryDel.DataAccess.Interfaces;

public interface IProductRepository
{
    List<ProductModel> GetAll();
    ProductModel? GetById(Guid id);
    void Add(ProductModel product);
    void Update(ProductModel product);
    void Delete(Guid id);
}
