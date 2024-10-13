using Moq;
using OrderStream.Application.Models;
using OrderStream.Domain.Entities;
using OrderStream.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderStream.Tests.ServiceTests
{
    public class OrderServiceTests : IClassFixture<OrderServiceTestFixture>
    {
        private readonly OrderServiceTestFixture _fixture;

        public OrderServiceTests(OrderServiceTestFixture fixture)
        {
            _fixture = fixture;
        }

        #region CreateOrder

        [Fact]
        public void CreateOrder_CustomerId_LessThanZero_ReturnFalse()
        {
            var model = _fixture.CreateModelInstance<OrderModel>();

            var result = _fixture.OrderService.CreateOrder(model);

            Assert.False(result);
        }


        [Fact]
        public void CreateOrder_CustomerId_EqualZero_ReturnFalse()
        {
            var model = _fixture.CreateModelInstance<OrderModel>();

            var result = _fixture.OrderService.CreateOrder(model);

            Assert.False(result);
        }

        [Fact]
        public void CreateOrder_OrderItems_IsNullOrEmpty_ReturnFalse()
        {
            var model = _fixture.CreateModelInstance<OrderModel>();
            model.CustomerId = 10;
            var result = _fixture.OrderService.CreateOrder(model);

            Assert.False(result);
        }

        [Fact]
        public void CreateOrder_GetByIdProductNull_ReturnFalse()
        {
            var model = _fixture.CreateModelInstance<OrderModel>();
            model.CustomerId = 10;
            model.Items = new List<OrderItemModel> { new OrderItemModel { Price = 1500, ProductId = "66f037e7a04b71bfcaad6487", Quantity = 10 } };

            _fixture.ProductRepositoryMock
                .Setup(x => x.GetById(It.IsAny<string>()))
                .Returns((Product)null);

            var result = _fixture.OrderService.CreateOrder(model);

            Assert.False(result);
        }

        [Fact]

        public void CreateOrder_GetByIdProduct_StockQuantityLessThanItemQantity_ReturnFalse()
        {
            var model = _fixture.CreateModelInstance<OrderModel>();
            model.CustomerId = 10;
            model.Items = new List<OrderItemModel> { new OrderItemModel { Price = 1500, ProductId = "66f037e7a04b71bfcaad6487", Quantity = 10 } };

            _fixture.ProductRepositoryMock
                .Setup(x => x.GetById(It.IsAny<string>()))
                .Returns(new Product { StockQuantity = 9, Name = "Test", Id = "66f037e7a04b71bfcaad6487" });

            var result = _fixture.OrderService.CreateOrder(model);

            Assert.False(result);

        }

        [Fact]

        public void CreateOrder_OrderItemQuantityLessThanZero_ReturnFalse()
        {
            var model = _fixture.CreateModelInstance<OrderModel>();
            model.CustomerId = 10;
            model.Items = new List<OrderItemModel> { new OrderItemModel { Price = 1500, ProductId = "66f037e7a04b71bfcaad6487", Quantity = -1 } };

            _fixture.ProductRepositoryMock
                .Setup(x => x.GetById(It.IsAny<string>()))
                .Returns(new Product { StockQuantity = 9, Name = "Test", Id = "66f037e7a04b71bfcaad6487" });

            var result = _fixture.OrderService.CreateOrder(model);

            Assert.False(result);
        }

        [Fact]

        public void CreateOrder_OrderItemQuantityEqualZero_ReturnFalse()
        {
            var model = _fixture.CreateModelInstance<OrderModel>();
            model.CustomerId = 10;
            model.Items = new List<OrderItemModel> { new OrderItemModel { Price = 1500, ProductId = "66f037e7a04b71bfcaad6487", Quantity = 0 } };

            _fixture.ProductRepositoryMock
                .Setup(x => x.GetById(It.IsAny<string>()))
                .Returns(new Product { StockQuantity = 9, Name = "Test", Id = "66f037e7a04b71bfcaad6487" });

            var result = _fixture.OrderService.CreateOrder(model);

            Assert.False(result);
        }

        [Fact]
        public void CreateOrder_OrderTotalAmountEqualZero_ReturnFalse()
        {
            var model = _fixture.CreateModelInstance<OrderModel>();
            model.CustomerId = 10;
            model.TotalAmount = 0;
            model.Items = new List<OrderItemModel> { new OrderItemModel { Price = 0, ProductId = "66f037e7a04b71bfcaad6487", Quantity = 5 } };

            _fixture.ProductRepositoryMock
                    .Setup(x => x.GetById(It.IsAny<string>()))
                    .Returns(new Product { StockQuantity = 6, Name = "Test", Id = "66f037e7a04b71bfcaad6487" });


            _fixture.ProductRepositoryMock
                    .Setup(x => x.Update(It.IsAny<Product>()))
                    .Returns(true);


            var result = _fixture.OrderService.CreateOrder(model);

            Assert.False(result);

        }

        [Fact]
        public void CreateOrder_ValidOrder_ReturnTrue()
        {
            var model = _fixture.CreateModelInstance<OrderModel>();
            model.CustomerId = 10;
            model.TotalAmount = 0;
            model.Items = new List<OrderItemModel> { new OrderItemModel { Price = 100, ProductId = "66f037e7a04b71bfcaad6487", Quantity = 5 } };

            _fixture.ProductRepositoryMock
                    .Setup(x => x.GetById(It.IsAny<string>()))
                    .Returns(new Product { StockQuantity = 6, Name = "Test", Id = "66f037e7a04b71bfcaad6487" });


            _fixture.ProductRepositoryMock
                    .Setup(x => x.Update(It.IsAny<Product>()))
                    .Returns(true);


            _fixture.OrderRepositoryMock
                    .Setup(x => x.Add(It.IsAny<Order>()))
                    .Returns(true);


            var result = _fixture.OrderService.CreateOrder(model);

            Assert.True(result);
        }
        #endregion

    }
}
