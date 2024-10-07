using OrderStream.Application.Interfaces.Repositories;
using OrderStream.Application.Models;
using OrderStream.Application.Services;
using OrderStream.Domain.Entities;

namespace OrderStream.Infrastructure.Implementations.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public bool CreateProduct(ProductModel product)
        {
            if (string.IsNullOrWhiteSpace(product.Name)) return false;
            if (product.Price <= 0) return false;
            if (product.StockQuantity <= 0) return false;

            var newProduct = new Product
            {
                IsDiscontinued = false,
                IsArchived = false,
                Name = product.Name,
                Price = product.Price,
                StockQuantity = product.StockQuantity
            };

            return _productRepository.Add(newProduct);
        }

        public ProductModel GetProductById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            var existingProduct = _productRepository.GetById(id);
            if (existingProduct == null) return null;

            return new ProductModel
            {
                Id = existingProduct.Id,
                Name = existingProduct.Name,
                Price = existingProduct.Price,
                StockQuantity = existingProduct.StockQuantity
            };
        }

        public IEnumerable<ProductModel> GetAllProducts()
        {
            return _productRepository.GetAll().Select(product => new ProductModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                StockQuantity = product.StockQuantity
            }).ToList();
        }

        public bool UpdateProduct(ProductModel product)
        {
            if (string.IsNullOrWhiteSpace(product.Name) || product.Price <= 0 || product.StockQuantity < 0)
                return false;

            var existingProduct = _productRepository.GetById(product.Id);
            if (existingProduct == null) return false;

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.StockQuantity = product.StockQuantity;

            return _productRepository.Update(existingProduct);
        }

        public bool DeleteProduct(string id)
        {
            var existingProduct = _productRepository.GetById(id);
            if (existingProduct == null) return false;

            return _productRepository.Delete(id);
        }

        public bool RestockProduct(string productId, int quantity)
        {
            var product = _productRepository.GetById(productId);
            if (product == null || quantity <= 0) return false;

            product.StockQuantity += quantity;
            return _productRepository.Update(product);
        }

        public bool DiscountProduct(string productId, decimal discountPercentage)
        {
            var product = _productRepository.GetById(productId);
            if (product == null || discountPercentage <= 0 || discountPercentage > 100) return false;

            product.Price -= product.Price * (discountPercentage / 100);
            return _productRepository.Update(product);
        }

        public IEnumerable<ProductModel> GetOutOfStockProducts()
        {
            return _productRepository.GetAll().Where(p => p.StockQuantity == 0).Select(product => new ProductModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                StockQuantity = product.StockQuantity
            }).ToList();
        }

        public bool ArchiveProduct(string productId)
        {
            var product = _productRepository.GetById(productId);
            if (product == null) return false;

            product.StockQuantity = 0;
            product.Price = 0;
            product.IsArchived = true;
            return _productRepository.Update(product);
        }

        public bool BulkUpdateProducts(List<ProductModel> products)
        {
            bool result = true;

            if (products == null || !products.Any()) return false;

            foreach (var product in products)
            {
                if (string.IsNullOrWhiteSpace(product.Name) || product.Price <= 0 || product.StockQuantity < 0)
                {
                    return false; // Geçersiz bir ürün varsa işlem başarısız olur
                }

                var existingProduct = _productRepository.GetById(product.Id);
                if (existingProduct == null) return false; // Ürün bulunamazsa başarısız olur

                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.StockQuantity = product.StockQuantity;
                var updateResult = _productRepository.Update(existingProduct);

                if (!updateResult)
                    return false;
            }
            return true;
        }
    }
}