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
    public class OrderServiceTestFixture
    {
        public readonly Mock<IOrderRepository> 
            OrderRepositoryMock;
        public readonly Mock<IProductRepository> 
             ProductRepositoryMock;

        public OrderService OrderService;
        public OrderServiceTestFixture()
        {
            ProductRepositoryMock = new Mock<IProductRepository>();
            OrderRepositoryMock = new Mock<IOrderRepository>();
            OrderService = new OrderService(OrderRepositoryMock.Object, ProductRepositoryMock.Object);
        }
        public T CreateModelInstance<T>() where T : new()
        {
            return new T();
        }
    }
}
