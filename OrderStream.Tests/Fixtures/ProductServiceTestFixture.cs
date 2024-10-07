using Moq;
using OrderStream.Application.Interfaces.Repositories;
using OrderStream.Application.Models;
using OrderStream.Application.Services;
using OrderStream.Infrastructure.Implementations.Repositories;
using OrderStream.Infrastructure.Implementations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderStream.Tests.Fixtures
{
    public class ProductServiceTestFixture
    {
        public Mock<IProductRepository> ProductRepositoryMock { get; private set; }
        public IProductService ProductService { get; set; }

        public ProductServiceTestFixture()
        {
            ProductRepositoryMock = new Mock<IProductRepository>();
            ProductService = new ProductService(ProductRepositoryMock.Object);
        }
    }
}
