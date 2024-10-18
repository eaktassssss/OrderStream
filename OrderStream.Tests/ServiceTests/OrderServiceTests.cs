using Moq;
using OrderStream.Application.Models;
using OrderStream.Domain.Entities;
using OrderStream.Domain.Enums;
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

        #region GetOrderById
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetOrderById_IsNullOrEmpty_ReturnFalse(string id)
        {
            var result = _fixture.OrderService.GetOrderById(id);

            Assert.Null(result);
        }
        [Theory]
        [InlineData("66f037e7a04b71bfcaad6487")]
        public void GetOrderById_NotExistingOrder_ReturnFalse(string id)
        {
            _fixture.OrderRepositoryMock
                .Setup(x => x.GetById(It.IsAny<string>()))
                .Returns((Order)null);

            var result = _fixture.OrderService.GetOrderById(id);

            Assert.Null(result);
        }

        [Fact]
        public void GetOrderById_InvalidOrder_ReturnOrder()
        {
            string id = "66f037e7a04b71bfcaad6487";

            var order = _fixture.CreateModelInstance<Order>();
            order = new Order { Id = id, CustomerId = 1, OrderDate = DateTime.Now, OrderStatus = Domain.Enums.OrderStatus.Completed, TotalAmount = 1000, OrderItems = new List<OrderItem> { new OrderItem { Price = 50, ProductId = "66f037e7a04b71bfcaad6483", Quantity = 20 } } };


            _fixture.OrderRepositoryMock
                .Setup(x => x.GetById(id))
                .Returns(order);

            var result = _fixture.OrderService.GetOrderById(id);

            Assert.NotNull(result);
            Assert.IsType<OrderModel>(result);
        }
        #endregion

        #region CancelOrder

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CancelOrder_IsNullOrEmpty_ReturnFalse(string id)
        {
            var result = _fixture.OrderService.CancelOrder(id);

            Assert.False(result);
        }


        [Theory]
        [InlineData("66f037e7a04b71bfcaad6487")]
        public void CancelOrder_NotExistingOrder_ReturnFalse(string id)
        {
            _fixture.OrderRepositoryMock
                .Setup(x => x.GetById(It.IsAny<string>()))
                .Returns((Order)null);

            var result = _fixture.OrderService.CancelOrder(id);

            Assert.False(result);
        }

        [Fact]
        public void CancelOrder_ValidOrder_ReturnTrue()
        {
            string id = "66f037e7a04b71bfcaad6487";
            var order = _fixture.CreateModelInstance<Order>();
            order = new Order { Id = id, CustomerId = 1, OrderDate = DateTime.Now, OrderStatus = Domain.Enums.OrderStatus.Completed, TotalAmount = 1000, OrderItems = new List<OrderItem> { new OrderItem { Price = 50, ProductId = "66f037e7a04b71bfcaad6483", Quantity = 20 } } };

            _fixture.OrderRepositoryMock
                .Setup(x => x.GetById(id))
                .Returns(order);

            _fixture.OrderRepositoryMock
                .Setup(x => x.Update(It.IsAny<Order>()))
                .Returns(true);

            var result = _fixture.OrderService.CancelOrder(id);

            Assert.True(result);
        }
        #endregion

        #region DeleteOrder 
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void DeleteOrder_OrderId_IsNullOrEmpty_ReturnFalse(string id)
        {
            var result = _fixture.OrderService.DeleteOrder(id);

            Assert.False(result);
        }

        [Theory]
        [InlineData("66f037e7a04b71bfcaad6487")]
        public void DeleteOrder_NotExistingOrder_ReturnFalse(string id)
        {
            _fixture.OrderRepositoryMock
                .Setup(x => x.GetById(It.IsAny<string>()))
                .Returns((Order)null);

            var result = _fixture.OrderService.DeleteOrder(id);

            Assert.False(result);
        }


        [Theory]
        [InlineData("66f037e7a04b71bfcaad6487")]
        public void DeleteOrder_ValidOrder_ReturnTrue(string id)
        {

            var order = _fixture.CreateModelInstance<Order>();
            order = new Order { Id = id, CustomerId = 1, OrderDate = DateTime.Now, OrderStatus = Domain.Enums.OrderStatus.Completed, TotalAmount = 1000, OrderItems = new List<OrderItem> { new OrderItem { Price = 50, ProductId = "66f037e7a04b71bfcaad6483", Quantity = 20 } } };


            _fixture.OrderRepositoryMock
                .Setup(x => x.GetById(It.IsAny<string>()))
                .Returns(order);

            _fixture.OrderRepositoryMock.Setup(x => x.Delete(It.IsAny<string>()))
                .Returns(true);

            var result = _fixture.OrderService.DeleteOrder(id);

            Assert.True(result);
        }
        #endregion


        #region RefundOrder
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void RefundOrder_OrderId_IsNullOrEmpty_ReturnFalse(string id)
        {
            var result = _fixture.OrderService.RefundOrder(id);

            Assert.False(result);
        }

        [Theory]
        [InlineData("66f037e7a04b71bfcaad6487")]
        public void RefundOrder_NotExistingOrder_ReturnFalse(string id)
        {
            _fixture.OrderRepositoryMock
                .Setup(x => x.GetById(It.IsAny<string>()))
                .Returns((Order)null);

            var result = _fixture.OrderService.RefundOrder(id);

            Assert.False(result);
        }



        [Theory]
        [InlineData("66f037e7a04b71bfcaad6487")]
        public void RefundOrder_ExistingOrder_OrderStatusCompleted_ReturnFalse(string id)
        {
            var order = _fixture.CreateModelInstance<Order>();

            order = new Order { Id = id, CustomerId = 1, OrderDate = DateTime.Now, OrderStatus = Domain.Enums.OrderStatus.Delivered, TotalAmount = 1000, OrderItems = new List<OrderItem> { new OrderItem { Price = 50, ProductId = "66f037e7a04b71bfcaad6483", Quantity = 20 } } };

            _fixture.OrderRepositoryMock
                .Setup(x => x.GetById(It.IsAny<string>()))
                .Returns(order);

            var result = _fixture.OrderService.RefundOrder(id);

            Assert.False(result);
        }


        [Theory]
        [InlineData("66f037e7a04b71bfcaad6487")]
        public void RefundOrder_ValidOrder_ReturnTrue(string id)
        {

            var order = _fixture.CreateModelInstance<Order>();

            order = new Order { Id = id, CustomerId = 1, OrderDate = DateTime.Now, OrderStatus = Domain.Enums.OrderStatus.Completed, TotalAmount = 1000, OrderItems = new List<OrderItem> { new OrderItem { Price = 50, ProductId = "66f037e7a04b71bfcaad6483", Quantity = 20 } } };

            _fixture.OrderRepositoryMock.
                Setup(x => x.GetById(It.IsAny<string>()))
                .Returns(order);

            _fixture.OrderRepositoryMock.
                Setup(x => x.Update(It.IsAny<Order>())).
                Returns(true);

            var result = _fixture.OrderService
                .RefundOrder(id);

            Assert.True(result);





        }

        #endregion


        #region ReopenOrder
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ReopenOrder_OrderId_IsNullOrEmpty_ReturnFalse(string id)
        {
            var result = _fixture.OrderService.ReopenOrder(id);

            Assert.False(result);
        }

        [Theory]
        [InlineData("66f037e7a04b71bfcaad6487")]
        public void ReopenOrder_NotExistingOrder_ReturnFalse(string id)
        {
            _fixture.OrderRepositoryMock
                .Setup(x => x.GetById(It.IsAny<string>()))
                .Returns((Order)null);

            var result = _fixture.OrderService.ReopenOrder(id);

            Assert.False(result);
        }



        [Theory]
        [InlineData("66f037e7a04b71bfcaad6487")]
        public void ReopenOrder_ExistingOrder_OrderStatusCancelled_ReturnFalse(string id)
        {
            var order = _fixture.CreateModelInstance<Order>();

            order = new Order { Id = id, CustomerId = 1, OrderDate = DateTime.Now, OrderStatus = Domain.Enums.OrderStatus.Delivered, TotalAmount = 1000, OrderItems = new List<OrderItem> { new OrderItem { Price = 50, ProductId = "66f037e7a04b71bfcaad6483", Quantity = 20 } } };

            _fixture.OrderRepositoryMock
                .Setup(x => x.GetById(It.IsAny<string>()))
                .Returns(order);

            var result = _fixture.OrderService.ReopenOrder(id);

            Assert.False(result);
        }


        [Theory]
        [InlineData("66f037e7a04b71bfcaad6487")]
        public void ReopenOrder_ValidOrder_ReturnTrue(string id)
        {

            var order = _fixture.CreateModelInstance<Order>();

            order = new Order { Id = id, CustomerId = 1, OrderDate = DateTime.Now, OrderStatus = Domain.Enums.OrderStatus.Cancelled, TotalAmount = 1000, OrderItems = new List<OrderItem> { new OrderItem { Price = 50, ProductId = "66f037e7a04b71bfcaad6483", Quantity = 20 } } };

            _fixture.OrderRepositoryMock.
                Setup(x => x.GetById(It.IsAny<string>()))
                .Returns(order);

            _fixture.OrderRepositoryMock.
                Setup(x => x.Update(It.IsAny<Order>())).
                Returns(true);

            var result = _fixture.OrderService
                .ReopenOrder(id);

            Assert.True(result);
        }

        #endregion



        #region ChangeOrderStatus
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ChangeOrderStatus_OrderId_IsNullOrEmpty_ReturnFalse(string id)
        {
            var result = _fixture.OrderService.ChangeOrderStatus(id, Domain.Enums.OrderStatus.Completed);

            Assert.False(result);
        }

        [Theory]
        [InlineData("66f037e7a04b71bfcaad6487")]
        public void ChangeOrderStatus_NotExistingOrder_ReturnFalse(string id)
        {
            _fixture.OrderRepositoryMock
                .Setup(x => x.GetById(It.IsAny<string>()))
                .Returns((Order)null);

            var result = _fixture.OrderService.ChangeOrderStatus(id, Domain.Enums.OrderStatus.Completed);

            Assert.False(result);
        }





        [Theory]
        [InlineData("66f037e7a04b71bfcaad6487", OrderStatus.Completed)]
        public void ChangeOrderStatus_ValidOrder_ReturnTrue(string id, OrderStatus newStatus)
        {

            var order = _fixture.CreateModelInstance<Order>();

            order = new Order { Id = id, CustomerId = 1, OrderDate = DateTime.Now, OrderStatus = Domain.Enums.OrderStatus.Delivered, TotalAmount = 1000, OrderItems = new List<OrderItem> { new OrderItem { Price = 50, ProductId = "66f037e7a04b71bfcaad6483", Quantity = 20 } } };

            _fixture.OrderRepositoryMock.
                Setup(x => x.GetById(It.IsAny<string>()))
                .Returns(order);

            _fixture.OrderRepositoryMock.
                Setup(x => x.Update(It.IsAny<Order>())).
                Returns(true);

            var result = _fixture.OrderService
                .ChangeOrderStatus(id, newStatus);

            Assert.True(result);
        }

        #endregion

        #region GetAllOrders
        [Fact]
        public void GetAllOrders_ReturnOrders()
        {
            var orders = _fixture.CreateModelInstance<List<Order>>();


            orders = new List<Order>
            {
                new Order { Id = "66f037e7a04b71bfcaad6487", CustomerId = 1, OrderDate = DateTime.Now, OrderStatus = Domain.Enums.OrderStatus.Completed, TotalAmount = 1000, OrderItems = new List<OrderItem> { new OrderItem { Price = 50, ProductId = "66f037e7a04b71bfcaad6483", Quantity = 20 } } },
                new Order { Id = "66f037e7a04b71bfcaad6488", CustomerId = 2, OrderDate = DateTime.Now, OrderStatus = Domain.Enums.OrderStatus.Completed, TotalAmount = 2000, OrderItems = new List<OrderItem> { new OrderItem { Price = 100, ProductId = "66f037e7a04b71bfcaad6484", Quantity = 20 } } }
            };

            _fixture.OrderRepositoryMock
                .Setup(x => x.GetAll())
                .Returns(orders);

            var result = _fixture.OrderService.GetAllOrders();

            Assert.NotNull(result);
            Assert.IsType<List<OrderModel>>(result);
            Assert.Equal(2, result.Count());
            Assert.True(result.Any());
        }


        [Fact]
        public void GetAllOrders_ReturnEmptyList()
        {
            var orders = _fixture.CreateModelInstance<List<Order>>();

            orders = new List<Order>();

            _fixture.OrderRepositoryMock
                .Setup(x => x.GetAll())
                .Returns(orders);

            var result = _fixture.OrderService.GetAllOrders();

            Assert.NotNull(result);
            Assert.IsType<List<OrderModel>>(result);
            Assert.Empty(result);
        }
        #endregion

        #region UpdateOrderItems
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void UpdateOrderItems_OrderId_IsNullOrEmpty_ReturnFalse(string id)
        {
            var result = _fixture.OrderService.UpdateOrderItems(id, new List<OrderItemModel>());

            Assert.False(result);
        }

        [Fact]
        public void UpdateOrderItems_OrderItems_IsNullOrEmpty_ReturnFalse()
        {
            var result = _fixture.OrderService.UpdateOrderItems("66f037e7a04b71bfcaad6487", new List<OrderItemModel>());

            Assert.False(result);
        }

        [Theory]
        [InlineData("66f037e7a04b71bfcaad6487")]
        public void UpdateOrderItems_NotExistingOrder_ReturnFalse(string orderId)
        {
            var orderItems = _fixture.CreateModelInstance<List<OrderItemModel>>();
            orderItems = new List<OrderItemModel> { new OrderItemModel { Price = 1500, ProductId = "66f037e7a04b71bfcaad6487", Quantity = 10 } };

            _fixture.OrderRepositoryMock
                .Setup(x => x.GetById(It.IsAny<string>()))
                .Returns((Order)null);


            var result = _fixture.OrderService.UpdateOrderItems(orderId, orderItems);

            Assert.False(result);
        }

        [Fact]
        public void UpdateOrderItems_GetByIdProductNull_ReturnFalse()
        {
            string orderId = "66f037e7a04b71bfcaad6487";

            var orderItems = _fixture.CreateModelInstance<List<OrderItemModel>>();
            orderItems = new List<OrderItemModel> { new OrderItemModel { Price = 1500, ProductId = "66f037e7a04b71bfcaad6487", Quantity = 10 } };


            _fixture.OrderRepositoryMock
                .Setup(x => x.GetById(It.IsAny<string>()))
                .Returns(new Order { Id = orderId, CustomerId = 1, OrderDate = DateTime.Now, OrderStatus = Domain.Enums.OrderStatus.Completed, TotalAmount = 1000, OrderItems = new List<OrderItem> { new OrderItem { Price = 50, ProductId = "66f037e7a04b71bfcaad6483", Quantity = 20 } } });

            _fixture.ProductRepositoryMock.Setup(x => x.GetById(It.IsAny<string>()))
                .Returns((Product)null);


            var result = _fixture.OrderService.UpdateOrderItems(orderId, orderItems);


            Assert.False(result);


        }

        [Fact]
        public void UpdateOrderItems_GetByProduct_StockQuantityLessThanItemQuantity_ReturnFalse()
        {
            string orderId = "66f037e7a04b71bfcaad6487";

            var orderItems = _fixture.CreateModelInstance<List<OrderItemModel>>();
            orderItems = new List<OrderItemModel> { new OrderItemModel { Price = 1500, ProductId = "66f037e7a04b71bfcaad6487", Quantity = 10 } };


            var order = _fixture.CreateModelInstance<Order>();

            order = new Order { Id = orderId, CustomerId = 1, OrderDate = DateTime.Now, OrderStatus = Domain.Enums.OrderStatus.Completed, TotalAmount = 1000, OrderItems = new List<OrderItem> { new OrderItem { Price = 50, ProductId = "66f037e7a04b71bfcaad6483", Quantity = 20 } } };

            _fixture.OrderRepositoryMock
                .Setup(x => x.GetById(It.IsAny<string>()))
                .Returns(order);

            var product = _fixture.CreateModelInstance<Product>();
            product = new Product { StockQuantity = 9, Name = "Test", Id = "66f037e7a04b71bfcaad6487" };

            _fixture.ProductRepositoryMock
                .Setup(x => x.GetById(It.IsAny<string>()))
                .Returns(product);

            var result = _fixture.OrderService.UpdateOrderItems(orderId, orderItems);

            Assert.False(result);
        }

        [Fact]
        public void UpdateOrderItems_OrderItemQuantityLessThanZero_ReturnFalse()
        {
            string orderId = "66f037e7a04b71bfcaad6487";

            var orderItems = _fixture.CreateModelInstance<List<OrderItemModel>>();
            orderItems = new List<OrderItemModel> { new OrderItemModel { Price = 1500, ProductId = "66f037e7a04b71bfcaad6487", Quantity = -1 }, new OrderItemModel { Price = 1500, ProductId = "66f037e7a04b71bfcaad6487", Quantity = 10 }, new OrderItemModel { Price = 1500, ProductId = "66f037e7a04b71bfcaad6487", Quantity = 5 } };

            var result = _fixture.OrderService.UpdateOrderItems(orderId, orderItems);

            Assert.False(result);

        }

        [Fact]
        public void UpdateOrderItems_OrderItemQuantityEqualZero_ReturnFalse()
        {
            string orderId = "66f037e7a04b71bfcaad6487";

            var orderItems = _fixture.CreateModelInstance<List<OrderItemModel>>();
            orderItems = new List<OrderItemModel> { new OrderItemModel { Price = 1500, ProductId = "66f037e7a04b71bfcaad6487", Quantity = -1 }, new OrderItemModel { Price = 1500, ProductId = "66f037e7a04b71bfcaad6487", Quantity = 10 }, new OrderItemModel { Price = 1500, ProductId = "66f037e7a04b71bfcaad6487", Quantity = 0 } };

            var result = _fixture.OrderService.UpdateOrderItems(orderId, orderItems);

            Assert.False(result);

        }


        [Fact]
        public void UpdateOrderItems_OrderItemPriceLessThanZero_ReturnFalse()
        {
            string orderId = "66f037e7a04b71bfcaad6487";

            var orderItems = _fixture.CreateModelInstance<List<OrderItemModel>>();
            orderItems = new List<OrderItemModel> { new OrderItemModel { Price = 1500, ProductId = "66f037e7a04b71bfcaad6487", Quantity = -1 }, new OrderItemModel { Price = 1500, ProductId = "66f037e7a04b71bfcaad6487", Quantity = 10 }, new OrderItemModel { Price = 1500, ProductId = "66f037e7a04b71bfcaad6487", Quantity = 5 } };

            var result = _fixture.OrderService.UpdateOrderItems(orderId, orderItems);

            Assert.False(result);

        }

        [Fact]
        public void UpdateOrderItems_OrderItemPriceEqualZero_ReturnFalse()
        {
            string orderId = "66f037e7a04b71bfcaad6487";

            var orderItems = _fixture.CreateModelInstance<List<OrderItemModel>>();
            orderItems = new List<OrderItemModel> { new OrderItemModel { Price = 0, ProductId = "66f037e7a04b71bfcaad6487", Quantity = 10 }, new OrderItemModel { Price = -1, ProductId = "66f037e7a04b71bfcaad6487", Quantity = 10 }, new OrderItemModel { Price = 1500, ProductId = "66f037e7a04b71bfcaad6487", Quantity = 10 } };

            var result = _fixture.OrderService.UpdateOrderItems(orderId, orderItems);

            Assert.False(result);

        }

        [Fact]
        public void UpdateOrderItems_ValidOrder_ReturnTrue()
        {
            string orderId = "66f037e7a04b71bfcaad6487";

            var orderItems = _fixture.CreateModelInstance<List<OrderItemModel>>();

            orderItems = new List<OrderItemModel> { new OrderItemModel { Price = 1500, ProductId = "66f037e7a04b71bfcaad6487", Quantity = 10 }, new OrderItemModel { Price = 1500, ProductId = "66f037e7a04b71bfcaad6487", Quantity = 10 }, new OrderItemModel { Price = 1500, ProductId = "66f037e7a04b71bfcaad6487", Quantity = 10 } };


            var order = _fixture.CreateModelInstance<Order>();

            order = new Order { Id = orderId, CustomerId = 1, OrderDate = DateTime.Now, OrderStatus = Domain.Enums.OrderStatus.Completed, TotalAmount = 1000, OrderItems = new List<OrderItem> { new OrderItem { Price = 50, ProductId = "66f037e7a04b71bfcaad6483", Quantity = 20 } } };

            _fixture.OrderRepositoryMock
                .Setup(x => x.GetById(It.IsAny<string>()))
                .Returns(order);

            var product = _fixture.CreateModelInstance<Product>();
            product = new Product { StockQuantity = 50, Name = "Test", Id = "66f037e7a04b71bfcaad6487" };

            _fixture.ProductRepositoryMock
                .Setup(x => x.GetById(It.IsAny<string>()))
                .Returns(product);

            _fixture.OrderRepositoryMock
                .Setup(x => x.Update(It.IsAny<Order>()))
                .Returns(true);

            var result = _fixture.OrderService.UpdateOrderItems(orderId, orderItems);

            Assert.True(result);
        }

        #endregion


        #region GetOrdersByCustomer

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void GetOrdersByCustomer_CustomerId_LessThanZero_ReturnFalse(int customerId)
        {
            var result = _fixture.OrderService.GetOrdersByCustomer(customerId);

            Assert.Null(result);
        }
        [Theory]
        [InlineData(0)]
        public void GetOrdersByCustomer_CustomerId_EqualZero_ReturnFalse(int customerId)
        {
            var result = _fixture.OrderService.GetOrdersByCustomer(customerId);

            Assert.Null(result);
        }
        [Theory]
        [InlineData(1)]
        public void GetOrdersByCustomer_ReturnOrders(int customerId)
        {
            var orders = _fixture.CreateModelInstance<List<Order>>();


            orders = new List<Order>
            {
                new Order { Id = "66f037e7a04b71bfcaad6487", CustomerId = 1, OrderDate = DateTime.Now, OrderStatus = Domain.Enums.OrderStatus.Completed, TotalAmount = 1000, OrderItems = new List<OrderItem> { new OrderItem { Price = 50, ProductId = "66f037e7a04b71bfcaad6483", Quantity = 20 } } },
                new Order { Id = "66f037e7a04b71bfcaad6488", CustomerId = 1, OrderDate = DateTime.Now, OrderStatus = Domain.Enums.OrderStatus.Completed, TotalAmount = 2000, OrderItems = new List<OrderItem> { new OrderItem { Price = 100, ProductId = "66f037e7a04b71bfcaad6484", Quantity = 20 } } }
            };

            _fixture.OrderRepositoryMock
                .Setup(x => x.GetAll())
                .Returns(orders);

            var result = _fixture.OrderService.GetOrdersByCustomer(customerId);
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(1)]
        public void GetOrdersByCustomer_ReturnEmptyList(int customerId)
        {
            var orders = _fixture.CreateModelInstance<List<Order>>();

            orders = new List<Order>();

            _fixture.OrderRepositoryMock
                .Setup(x => x.GetAll())
                .Returns(orders);

            var result = _fixture.OrderService.GetOrdersByCustomer(customerId);

            Assert.NotNull(result);
            Assert.IsType<List<OrderModel>>(result);
            Assert.Empty(result);
        }
 
        #endregion
    }

}