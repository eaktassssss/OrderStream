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
    public class ProductServiceTests:IClassFixture<ProductServiceTestFixture>
    {
        private readonly IProductService _productService;
        public ProductServiceTests(ProductServiceTestFixture fixture)
        {
            _productService=fixture.ProductService;
        }

        [Fact]
        public void CreateProduct_ProductName_NullOrWhiteSpace_ReturnFalse()
        {
            //Arrange
            ProductModel model = new ProductModel();
            //Act
            var result = _productService.CreateProduct(model);
            Assert.False(result);
        }
    }
}
