using OrderStream.Application.Models;

namespace OrderStream.Application.Services
{
    public interface IProductService
    {
        bool CreateProduct(ProductModel product);

        ProductModel GetProductById(string id);

        IEnumerable<ProductModel> GetAllProducts();

        bool UpdateProduct(ProductModel product);

        bool DeleteProduct(string id);

        bool RestockProduct(string productId, int quantity);

        bool DiscountProduct(string productId, decimal discountPercentage);

        IEnumerable<ProductModel> GetOutOfStockProducts();

        bool ArchiveProduct(string productId);

        bool BulkUpdateProducts(List<ProductModel> products);
    }
}