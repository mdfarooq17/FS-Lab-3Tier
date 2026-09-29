using ClassLibraryModel.Models;

namespace ClassLibraryBusiness.Business.Interfaces;

public interface IProductService
{
    List<ProductModel> GetAll();
    ProductModel? GetById(Guid id);
    void Create(ProductModel product);
    void Update(ProductModel product);
    void Delete(Guid id);
}
