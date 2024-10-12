using Moq;
using OrderStream.Application.Interfaces.Repositories;
using OrderStream.Application.Models;
using OrderStream.Application.Services;
using OrderStream.Domain.Entities;
using OrderStream.Infrastructure.Implementations.Services;
using OrderStream.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderStream.Tests.ServiceTests
{
    public class ProductServiceTests : IClassFixture<ProductServiceTestFixture>
    {
        private readonly ProductServiceTestFixture _fixture;
        public ProductServiceTests(ProductServiceTestFixture fixture)
        {
            _fixture = fixture;
        }

        #region CreateProduct Test
        [Fact]
        public void CreateProduct_ProductName_NullOrWhiteSpace_ReturnFalse()
        {
            ProductModel productModel = new ProductModel();

            var result = _fixture.ProductService.CreateProduct(productModel);

            Assert.False(result);
        }
        [Fact]
        public void CreateProduct_Price_LessThanZero_ReturnFalse()
        {
            ProductModel productModel = new ProductModel();
            productModel.Price = -1;
            productModel.Name = "Test";
            productModel.StockQuantity = 50;

            var result = _fixture.ProductService.CreateProduct(productModel);

            Assert.False(result);
        }
        [Fact]
        public void CreateProduct_Price_LessThanZeroOrEqualZero_ReturnFalse()
        {
            ProductModel productModel = new ProductModel();
            productModel.Price = -1;
            productModel.Name = "Test";
            productModel.StockQuantity = 50;

            var result = _fixture.ProductService.CreateProduct(productModel);

            Assert.False(result);
        }
        [Fact]
        public void CreateProduct_Price_EqualZero_ReturnFalse()
        {
            ProductModel productModel = new ProductModel();
            productModel.Name = "Test";
            productModel.StockQuantity = 50;
            productModel.Price = 1500;

            var result = _fixture.ProductService.CreateProduct(productModel);

            Assert.False(result);
        }
        [Fact]
        public void CreateProduct_StockQuantity_LessThanZero_ReturnFalse()
        {
            ProductModel productModel = new ProductModel();
            productModel.StockQuantity = -1;
            productModel.Price = 1500;
            productModel.Name = "Test";

            var result = _fixture.ProductService.CreateProduct(productModel);

            Assert.False(result);
        }
        [Fact]
        public void CreateProduct_StockQuantity_LessThanZeroOrEqualZero_ReturnFalse()
        {
            ProductModel productModel = new ProductModel();
            productModel.Price = 1500;
            productModel.Name = "Test";
            productModel.StockQuantity = -1;

            var result = _fixture.ProductService.CreateProduct(productModel);

            Assert.False(result);
        }
        [Fact]
        public void CreateProduct_StockQuantity_EqualZero_ReturnFalse()
        {
            ProductModel productModel = new ProductModel();
            productModel.Price = 1500;
            productModel.Name = "Test";
            productModel.StockQuantity = 0;

            var result = _fixture.ProductService.CreateProduct(productModel);

            Assert.False(result);
        }

        [Fact]
        public void CreateProduct_ValidProduct_ReturnTrue()
        {
            _fixture.ProductRepositoryMock.
                Setup(setup => setup.Add(It.IsAny<Product>()))
                .Returns(true);

            var result = _fixture
                .ProductService.
                CreateProduct(new ProductModel { Name = "Test", Price = 1500, StockQuantity = 10 });

            Assert.True(result);
        }
        #endregion

        #region GetProductById Test
        [Fact]
        public void GetProductById_IdNull_ReturnNull()
        {
            var result = _fixture.ProductService.GetProductById(null);
            Assert.Null(result);
        }

        [Theory]
        [InlineData("66f037e7a04b71bfcaad6487")]
        public void GetProductById_Invalid_ProductId_ReturnNull(string id)
        {
            _fixture.ProductRepositoryMock.Setup(x => x.GetById(It.IsAny<string>())).Returns((Product)null);

            var result = _fixture.ProductService.GetProductById(id);

            Assert.Null(result);
        }


        [Theory]
        [InlineData("66f037e7a04b71bfcaad6487")]
        public void GetProductById_Valid_ProductId_ReturnProduct(string id)
        {

            _fixture.ProductRepositoryMock
                .Setup(x => x.GetById(It.IsAny<string>()))
                .Returns(new Product { Id = "66f037e7a04b71bfcaad6487", Name = "Test", Price = 1500, StockQuantity = 10 });

            var result = _fixture.ProductService.GetProductById(id);

            Assert.NotNull(result);
        }
        #endregion

        #region UpdateProduct
        [Fact]
        public void UpdateProduct_ProductName_NullOrWhiteSpace_ReturnFalse()
        {
            ProductModel productModel = new ProductModel();

            var result = _fixture.ProductService.UpdateProduct(productModel);

            Assert.False(result);
        }

        [Fact]
        public void UpdateProduct_Price_LessThanZero_ReturnFalse()
        {
            ProductModel productModel = new ProductModel();
            productModel.Price = -1;
            productModel.Name = "Test";
            productModel.StockQuantity = 50;

            var result = _fixture.ProductService.UpdateProduct(productModel);

            Assert.False(result);
        }

        [Fact]
        public void UpdateProduct_Price_LessThanZeroOrEqualZero_ReturnFalse()
        {
            ProductModel productModel = new ProductModel();
            productModel.Price = 0;
            productModel.Name = "Test";
            productModel.StockQuantity = 50;

            var result = _fixture.ProductService.UpdateProduct(productModel);

            Assert.False(result);
        }

        [Fact]

        public void UpdateProduct_StockQuantity_LessThanZero_ReturnFalse()
        {
            ProductModel productModel = new ProductModel();
            productModel.StockQuantity = -1;
            productModel.Price = 1500;
            productModel.Name = "Test";

            var result = _fixture.ProductService.UpdateProduct(productModel);

            Assert.False(result);
        }
        [Fact]
        public void UpdateProduct_StockQuantity_LessThanZeroOrEqualZero_ReturnFalse()
        {
            ProductModel productModel = new ProductModel();
            productModel.Price = 1500;
            productModel.Name = "Test";
            productModel.StockQuantity = 0;

            var result = _fixture.ProductService.UpdateProduct(productModel);

            Assert.False(result);
        }

        [Fact]
        public void UpdateProduct_NotExistingProduct_ReturnFalse()
        {
            _fixture.ProductRepositoryMock
                .Setup(x => x.GetById(It.IsAny<string>()))
                .Returns((Product)null);

            var result = _fixture.ProductService
                .UpdateProduct(new ProductModel { Id = "66f037e7a04b71bfcaad6487", Name = "Test", Price = 1500, StockQuantity = 10 });

            Assert.False(result);
        }

        [Fact]
        public void UpdateProduct_ValidProduct_ReturnTrue()
        {
            ProductModel productModel = new ProductModel
            {
                Id = "66f037e7a04b71bfcaad6487",
                Name = "Test",
                Price = 1500,
                StockQuantity = 10
            };

            _fixture.ProductRepositoryMock.Setup(x => x.GetById(It.IsAny<string>()))
                .Returns(new Product { Id = "66f037e7a04b71bfcaad6487", Name = "Test", Price = 1500, StockQuantity = 10 });

            _fixture.ProductRepositoryMock.Setup(x => x.Update(It.IsAny<Product>()))
                .Returns(true);


            var result = _fixture.ProductService.UpdateProduct(productModel);
        }
        #endregion


        #region DeleteProduct
        [Fact]
        public void DeleteProduct_IdNull_ReturnFalse()
        {
            var result = _fixture.ProductService.DeleteProduct(null);
            Assert.False(result);
        }

        [Theory]
        [InlineData("66f037e7a04b71bfcaad6487")]
        public void DeleteProduct_NotExistingProduct_ReturnFalse(string id)
        {
            _fixture.ProductRepositoryMock.Setup(x => x.GetById(It.IsAny<string>())).Returns((Product)null);

            var result = _fixture.ProductService.DeleteProduct(id);

            Assert.False(result);
        }
        [Theory]
        [InlineData("66f037e7a04b71bfcaad6487")]
        public void DeleteProduct_ValidProduct_ReturnTrue(string id)
        {
            _fixture.ProductRepositoryMock.Setup(x => x.GetById(It.IsAny<string>()))
                .Returns(new Product { Id = "66f037e7a04b71bfcaad6487", Name = "Test", Price = 1500, StockQuantity = 10 });

            _fixture.ProductRepositoryMock.Setup(x => x.Delete(It.IsAny<string>()))
                .Returns(true);

            var result = _fixture.ProductService.DeleteProduct(id);

            Assert.True(result);
        }
        #endregion


        #region RestockProduct

        [Theory]
        [InlineData("66f037e7a04b71bfcaad6487", 0, false)]
        [InlineData("66f037e7a04b71bfcaad6487", -1, false)]
        public void RestockProduct_Quantity_LessThanZeroOrEqualZero_ReturnFalse(string productId, int quantity, bool expected)
        {
            var result = _fixture.ProductService.RestockProduct(productId, quantity);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("", 2, false)]
        [InlineData(null, 1, false)]
        public void RestockProduct_ProductId_IsNullOrWhiteSpace_ReturnFalse(string productId, int quantity, bool expected)
        {
            var result = _fixture.ProductService.RestockProduct(productId, quantity);
            Assert.Equal(expected, result);
        }


        [Fact]
        public void RestockProduct_NotExistingProduct_ReturnFalse()
        {
            _fixture.ProductRepositoryMock.Setup(x => x.GetById(It.IsAny<string>())).Returns((Product)null);

            var result = _fixture.ProductService.RestockProduct("66f037e7a04b71bfcaad6487", 10);

            Assert.False(result);
        }

        [Fact]
        public void RestockProduct_ValidProduct_ReturnTrue()
        {
            _fixture.ProductRepositoryMock
               .Setup(x => x.GetById(It.IsAny<string>()))
               .Returns(new Product { Id = "66f037e7a04b71bfcaad6487", Name = "Test", Price = 1500, StockQuantity = 10 });


            _fixture.ProductRepositoryMock
                .Setup(x => x.Update(It.IsAny<Product>()))
                .Returns(true);

            var result = _fixture.ProductService.RestockProduct("66f037e7a04b71bfcaad6487", 10);
            Assert.True(result);
        }


        [Theory]
        [InlineData("66f037e7a04b71bfcaad6487", 10)]
        public void RestockProduct_ValidProduct_Update_Quantity_QuentityValue20(string productId, int quantity)
        {
            _fixture.ProductRepositoryMock
                     .Setup(x => x.GetById(It.IsAny<string>()))
                     .Returns(new Product { Id = "66f037e7a04b71bfcaad6487", Name = "Test", Price = 1500, StockQuantity = 10 });


            _fixture.ProductRepositoryMock.
                Setup(x => x.Update(It.IsAny<Product>()))
                .Returns(true);

            var result = _fixture.ProductService.RestockProduct(productId, quantity);
            Assert.True(result);
            Assert.Equal(20, _fixture.ProductService.GetProductById(productId).StockQuantity);

        }
        #endregion

        #region DiscountProduct

        [Theory]
        [InlineData("", 10, false)]
        [InlineData(null, 10, false)]
        public void DiscountProduct_ProductId_IsNullOrWhiteSpace_ReturnFalse(string productId, decimal discountPercentage, bool expected)
        {
            var result = _fixture.ProductService.DiscountProduct(productId, discountPercentage);
            Assert.Equal(expected, result);
        }
        [Theory]
        [InlineData("66f037e7a04b71bfcaad6487", 0, false)]
        [InlineData("66f037e7a04b71bfcaad6487", -1, false)]
        [InlineData("66f037e7a04b71bfcaad6487", 101, false)]
        public void DiscountProduct_DiscountPercentage_LessThanZeroOrEqualZero_ReturnFalse(string productId, decimal discountPercentage, bool expected)
        {
            var result = _fixture.ProductService.DiscountProduct(productId, discountPercentage);
            Assert.Equal(expected, result);
        }
        [Theory]
        [InlineData("66f037e7a04b71bfcaad6487", 10)]
        public void DiscountProduct_NotExistingProduct_ReturnFalse(string productId, decimal discountPercentage)
        {
            _fixture.ProductRepositoryMock.Setup(x => x.GetById(It.IsAny<string>())).Returns((Product)null);

            var result = _fixture.ProductService.DiscountProduct(productId, discountPercentage);

            Assert.False(result);
        }

        [Theory]
        [InlineData("66f037e7a04b71bfcaad6487", 10)]
        public void DiscountProduct_ValidProduct_ReturnTrue(string productId, decimal discountPercentage)
        {
            _fixture.ProductRepositoryMock
                .Setup(x => x.GetById(It.IsAny<string>()))
                .Returns(new Product { Id = "66f037e7a04b71bfcaad6487", Name = "Test", Price = 1500, StockQuantity = 10 });

            _fixture.ProductRepositoryMock
                .Setup(x => x.Update(It.IsAny<Product>()))
                .Returns(true);

            var result = _fixture.ProductService.DiscountProduct(productId, discountPercentage);

            Assert.True(result);
        }
        #endregion


        #region GetOutOfStockProducts
        [Fact]
        public void GetOutOfStockProducts_ReturnsOutOfStockProducts()
        {
           
            var outOfStockProduct = new Product { Id = "1", Name = "Out of Stock Product", Price = 100, StockQuantity = 0 };
            var inStockProduct = new Product { Id = "2", Name = "In Stock Product", Price = 150, StockQuantity = 10 };

            _fixture.ProductRepositoryMock
                .Setup(x => x.GetAll())
                .Returns(new List<Product> { outOfStockProduct, inStockProduct });

           
            var result = _fixture.ProductService.GetOutOfStockProducts();

           
            Assert.Single(result);
            Assert.Equal(outOfStockProduct.Id, result.First().Id);
            Assert.Equal(outOfStockProduct.Name, result.First().Name);
        }
        [Fact]
        public void GetOutOfStockProducts_ReturnsEmptyList_WhenNoProducts()
        {
            _fixture.ProductRepositoryMock
                .Setup(x => x.GetAll())
                .Returns(new List<Product>());

            var result = _fixture.ProductService.GetOutOfStockProducts();

            Assert.Empty(result);
        }

        [Fact]
        public void GetOutOfStockProducts_ReturnsEmptyList_WhenAllProductsInStock()
        {
            var inStockProduct1 = new Product { Id = "1", Name = "Product 1", Price = 100, StockQuantity = 10 };
            var inStockProduct2 = new Product { Id = "2", Name = "Product 2", Price = 150, StockQuantity = 5 };

            _fixture.ProductRepositoryMock
                .Setup(x => x.GetAll())
                .Returns(new List<Product> { inStockProduct1, inStockProduct2 });

            var result = _fixture.ProductService.GetOutOfStockProducts();

            Assert.Empty(result);
        }

        #endregion


        #region ArchiveProduct

        [Theory]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void ArchiveProduct_ProductId_IsNullOrWhiteSpace_ReturnFalse(string productId, bool expected)
        {
            var result = _fixture.ProductService.ArchiveProduct(productId);
            Assert.Equal(expected, result);
        }
        [Fact]
        public void ArchiveProduct_NotExistingProduct_ReturnFalse()
        {
            _fixture.ProductRepositoryMock.Setup(x => x.GetById(It.IsAny<string>())).Returns((Product)null);

            var result = _fixture.ProductService.ArchiveProduct("66f037e7a04b71bfcaad6487");

            Assert.False(result);
        }

        [Fact]
        public void ArchiveProduct_ValidProduct_ReturnTrue()
        {
            _fixture.ProductRepositoryMock
                .Setup(x => x.GetById(It.IsAny<string>()))
                .Returns(new Product { Id = "66f037e7a04b71bfcaad6487", Name = "Test", Price = 1500, StockQuantity = 10 });

            _fixture.ProductRepositoryMock
                .Setup(x => x.Update(It.IsAny<Product>()))
                .Returns(true);

            var result = _fixture.ProductService.ArchiveProduct("66f037e7a04b71bfcaad6487");

            Assert.True(result);
        }

        #endregion



        #region BulkUpdateProducts

        [Fact]
        public void BulkUpdateProducts_Products_IsNull_ReturnFalse()
        {
            var result = _fixture.ProductService.BulkUpdateProducts(null);
            Assert.False(result);
        }
        [Fact]
        public void BulkUpdateProducts_Products_IsEmpty_ReturnFalse()
        {
            var result = _fixture.ProductService.BulkUpdateProducts(new List<ProductModel>());
            Assert.False(result);
        }
        [Fact]
        public void BulkUpdateProducts_ProductName_IsNullOrWhiteSpace_ReturnFalse()
        {
            var products = new List<ProductModel>
            {
                new ProductModel { Name = "", Price = 1500, StockQuantity = 10 }
            };

            var result = _fixture.ProductService.BulkUpdateProducts(products);
            Assert.False(result);
        }
        [Fact]
        public void BulkUpdateProducts_Price_LessThanZero_ReturnFalse()
        {
            var products = new List<ProductModel>
            {
                new ProductModel { Name = "Test", Price = -1, StockQuantity = 10 }
            };

            var result = _fixture.ProductService.BulkUpdateProducts(products);
            Assert.False(result);
        }
        [Fact]
        public void BulkUpdateProducts_Price_LessThanZeroOrEqualZero_ReturnFalse()
        {
            var products = new List<ProductModel>
            {
                new ProductModel { Name = "Test", Price = 0, StockQuantity = 10 }
            };

            var result = _fixture.ProductService.BulkUpdateProducts(products);
            Assert.False(result);
        }
        [Fact]
        public void BulkUpdateProducts_StockQuantity_LessThanZero_ReturnFalse()
        {
            var products = new List<ProductModel>
            {
                new ProductModel { Name = "Test", Price = 1500, StockQuantity = -1 }
            };

            var result = _fixture.ProductService.BulkUpdateProducts(products);
            Assert.False(result);
        }
        [Fact]
        public void BulkUpdateProducts_StockQuantity_LessThanZeroOrEqualZero_ReturnFalse()
        {
            var products = new List<ProductModel>
            {
                new ProductModel { Name = "Test", Price = 1500, StockQuantity = 0 }
            };

            var result = _fixture.ProductService.BulkUpdateProducts(products);
            Assert.False(result);
        }

        [Fact]
        public void BulkUpdateProducts_NotExistingProduct_ReturnFalse()
        {
            var products = new List<ProductModel>
                {
                    new ProductModel { Id = "66f037e7a04b71bfcaad6487", Name = "Test", Price = 1500, StockQuantity = 10 }
                };


            _fixture.ProductRepositoryMock
              .Setup(x => x.GetById(It.IsAny<string>()))
              .Returns((Product)null);


            var result = _fixture.ProductService.BulkUpdateProducts(products);
            Assert.False(result);

         
        }

        [Fact]
        public void BulkUpdateProducts_ValidProduct_ReturnTrue()
        {
            var products = new List<ProductModel>
            {
                new ProductModel { Id = "66f037e7a04b71bfcaad6487", Name = "Test", Price = 1500, StockQuantity = 10 }
            };

            _fixture.ProductRepositoryMock
                .Setup(x => x.GetById(It.IsAny<string>()))
                .Returns(new Product { Id = "66f037e7a04b71bfcaad6487", Name = "Test", Price = 1500, StockQuantity = 10 });

            _fixture.ProductRepositoryMock
                .Setup(x => x.Update(It.IsAny<Product>()))
                .Returns(true);

            var result = _fixture.ProductService.BulkUpdateProducts(products);
            Assert.True(result);
        }

        #endregion

        #region MyRegion
        [Fact]
        public void GetAllProducts_ReturnsAllProducts()
        {
            var product1 = new Product { Id = "1", Name = "Product 1", Price = 100, StockQuantity = 10 };
            var product2 = new Product { Id = "2", Name = "Product 2", Price = 150, StockQuantity = 5 };

            _fixture.ProductRepositoryMock
                .Setup(x => x.GetAll())
                .Returns(new List<Product> { product1, product2 });

            var result = _fixture.ProductService.GetAllProducts();

            Assert.Equal(2, result.Count());
            Assert.Contains(result, p => p.Id == product1.Id);
            Assert.Contains(result, p => p.Id == product2.Id);
        }
        [Fact]
        public void GetAllProducts_ReturnsEmptyList_WhenNoProducts()
        {
            _fixture.ProductRepositoryMock
                .Setup(x => x.GetAll())
                .Returns(new List<Product>());

            var result = _fixture.ProductService.GetAllProducts();

            Assert.Empty(result);
        }


        [Fact]
        public void GetAllProducts_ReturnsCorrectProductDetails()
        {
            var product = new Product { Id = "1", Name = "Test Product", Price = 200, StockQuantity = 15 };

            _fixture.ProductRepositoryMock
                .Setup(x => x.GetAll())
                .Returns(new List<Product> { product });

            var result = _fixture.ProductService.GetAllProducts().First();

            Assert.Equal(product.Id, result.Id);
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(product.Price, result.Price);
            Assert.Equal(product.StockQuantity, result.StockQuantity);
        }
        [Fact]
        public void GetAllProducts_ReturnsDifferentProducts()
        {
            var product1 = new Product { Id = "1", Name = "Product 1", Price = 100, StockQuantity = 10 };
            var product2 = new Product { Id = "2", Name = "Product 2", Price = 150, StockQuantity = 5 };

            _fixture.ProductRepositoryMock
                .Setup(x => x.GetAll())
                .Returns(new List<Product> { product1, product2 });

            var result = _fixture.ProductService.GetAllProducts();

            Assert.Equal(2, result.Count());
            Assert.Contains(result, p => p.Id == product1.Id);
            Assert.Contains(result, p => p.Id == product2.Id);
        }


        #endregion


    }

}