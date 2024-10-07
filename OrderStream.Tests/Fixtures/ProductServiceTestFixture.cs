using Moq;
using OrderStream.Application.Interfaces.Repositories;
using OrderStream.Infrastructure.Implementations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderStream.Tests.Fixtures
{
    public  class ProductServiceTestFixture
    {
        Mock<IProductRepository> ProductRepositoryMock;
        public ProductService ProductService { get; private set; }
        public ProductServiceTestFixture()
        {
            ProductRepositoryMock= new Mock<IProductRepository> ();
            ProductService= new ProductService(ProductRepositoryMock.Object);
        }
    }
}
