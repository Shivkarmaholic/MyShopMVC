using MyShop.Models;

namespace MyShop.Repositories.Interfaces
{
    public interface IProductRegistration
    {
        public Task<int> InsertProduct(ProductModel model);
        public Task<int> UpdateProduct(ProductModel model);
        public Task<int> DeleteProduct(DeleteModel model);        
        public Task<IEnumerable<ProductModel>> GetAllProducts();

    }
}
