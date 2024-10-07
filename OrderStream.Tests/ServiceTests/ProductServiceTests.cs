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

    }
}
