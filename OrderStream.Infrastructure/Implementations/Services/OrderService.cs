using OrderStream.Application.Interfaces.Repositories;
using OrderStream.Application.Models;
using OrderStream.Application.Services;
using OrderStream.Domain.Entities;
using OrderStream.Domain.Enums;

namespace OrderStream.Infrastructure.Implementations.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public bool CreateOrder(OrderModel order)
        {
            if (order == null) return false;

            if (order.CustomerId <= 0) return false;
            if (order.Items == null || !order.Items.Any()) return false;

            foreach (var item in order.Items)
            {
                var product = _productRepository.GetById(item.ProductId);
                if (product == null || product.StockQuantity < item.Quantity || item.Quantity <= 0) return false;

                // Ürün stokları güncelleniyor ve satış miktarı artırılıyor
                product.StockQuantity -= item.Quantity;
                product.SalesCount += item.Quantity; // Satış adedi artırılıyor
                _productRepository.Update(product);
            }

            order.TotalAmount = order.Items.Sum(i => i.Price * i.Quantity);
            if (order.TotalAmount <= 0) return false;

            var newOrder = new Order
            {
                CustomerId = order.CustomerId,
                OrderItems = order.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList(),
                OrderDate = DateTime.Now,
                OrderStatus = OrderStatus.Pending,
                TotalAmount = order.TotalAmount
            };

            return _orderRepository.Add(newOrder);
        }

        public OrderModel GetOrderById(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            OrderModel order = null;
            var existingOrder = _orderRepository.GetById(id);
            if (existingOrder == null) return null;

            order = new OrderModel
            {
                CustomerId = existingOrder.CustomerId,
                Items = existingOrder.OrderItems.Select(i => new OrderItemModel
                {
                    ProductId = i.ProductId,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList(),
                TotalAmount = existingOrder.TotalAmount,
                OrderDate = existingOrder.OrderDate,
                OrderStatus = existingOrder.OrderStatus
            };

            return order;
        }

        public IEnumerable<OrderModel> GetAllOrders()
        {
            return _orderRepository.GetAll().Select(order => new OrderModel
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                Items = order.OrderItems.Select(i => new OrderItemModel
                {
                    ProductId = i.ProductId,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList(),
                TotalAmount = order.TotalAmount,
                OrderDate = order.OrderDate,
                OrderStatus = order.OrderStatus
            }).ToList();
        }

        public IEnumerable<OrderModel> GetOrdersByCustomer(int customerId)
        {
            if(customerId<=0) return null;

            return _orderRepository.GetAll().Where(o => o.CustomerId == customerId).Select(order => new OrderModel
            {
                CustomerId = order.CustomerId,
                Items = order.OrderItems.Select(i => new OrderItemModel
                {
                    ProductId = i.ProductId,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList(),
                TotalAmount = order.TotalAmount,
                OrderDate = order.OrderDate,
                OrderStatus = order.OrderStatus
            }).ToList();
        }

        public IEnumerable<OrderModel> GetPendingOrders()
        {

            return _orderRepository.GetAll().Where(o => o.OrderStatus == OrderStatus.Pending).Select(order => new OrderModel
            {
                CustomerId = order.CustomerId,
                Items = order.OrderItems.Select(i => new OrderItemModel
                {
                    ProductId = i.ProductId,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList(),
                TotalAmount = order.TotalAmount,
                OrderDate = order.OrderDate,
                OrderStatus = order.OrderStatus
            }).ToList();
        }

        public bool CancelOrder(string orderId)
        {
            if (string.IsNullOrEmpty(orderId)) return false;

            var existingOrder = _orderRepository.GetById(orderId);
            if (existingOrder == null) return false;

            existingOrder.OrderStatus = OrderStatus.Cancelled;
            return _orderRepository.Update(existingOrder);
        }

        public bool CompleteOrder(string orderId)
        {
            if (string.IsNullOrEmpty(orderId)) return false;


            var existingOrder = _orderRepository.GetById(orderId);
            if (existingOrder == null) return false;

            // Sipariş tamamlanıyor, her ürünün satış adedi güncelleniyor
            foreach (var item in existingOrder.OrderItems)
            {
                var product = _productRepository.GetById(item.ProductId);
                if (product != null)
                {
                    product.SalesCount += item.Quantity; // Satılan miktarı ekle
                    _productRepository.Update(product);
                }
            }

            existingOrder.OrderStatus = OrderStatus.Completed;
            return _orderRepository.Update(existingOrder);
        }

        public bool UpdateOrderItems(string orderId, List<OrderItemModel> updatedItems)
        {
            if (string.IsNullOrEmpty(orderId)) return false;

            if (updatedItems == null || !updatedItems.Any()) return false;

            var quantityCheck = updatedItems.Any(i => i.Quantity <= 0);
            if (quantityCheck) return false;

            var priceCheck = updatedItems.Any(i => i.Price <= 0); // Fiyat kontrolü
            if (priceCheck) return false;

            var existingOrder = _orderRepository.GetById(orderId);
                if (existingOrder == null) 
                return false;


            foreach (var item in updatedItems)
            {
                var product = _productRepository.GetById(item.ProductId);
                if (product == null || product.StockQuantity < item.Quantity)
                    return false;
              
            }

            existingOrder.OrderItems = updatedItems.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Price = i.Price,
                Quantity = i.Quantity
            }).ToList();

            existingOrder.TotalAmount = updatedItems.Sum(i => i.Price * i.Quantity);
            return _orderRepository.Update(existingOrder);
        }

        public bool ChangeOrderStatus(string orderId, OrderStatus newStatus)
        {
            if (string.IsNullOrEmpty(orderId)) return false;

            var existingOrder = _orderRepository.GetById(orderId);
            if (existingOrder == null) return false;

            existingOrder.OrderStatus = newStatus;
            return _orderRepository.Update(existingOrder);
        }

        public bool RefundOrder(string orderId)
        {
            if (string.IsNullOrEmpty(orderId)) return false;

            var existingOrder = _orderRepository.GetById(orderId);
            if (existingOrder == null || existingOrder.OrderStatus != OrderStatus.Completed) return false;

            existingOrder.OrderStatus = OrderStatus.Refunded;
            return _orderRepository.Update(existingOrder);
        }

        public bool ReopenOrder(string orderId)
        {
            if (string.IsNullOrEmpty(orderId)) return false;

            var existingOrder = _orderRepository.GetById(orderId);
            if (existingOrder == null || existingOrder.OrderStatus != OrderStatus.Cancelled) return false;

            existingOrder.OrderStatus = OrderStatus.Pending;
            return _orderRepository.Update(existingOrder);
        }

        public bool DeleteOrder(string orderId)
        {
            if(string.IsNullOrEmpty(orderId)) return false;

            var existingOrder = _orderRepository.GetById(orderId);
            if (existingOrder == null) return false;

            return _orderRepository.Delete(orderId);
        }

        public bool ProcessOrderInStages(string orderId, List<OrderStatus> stages)
        {
            if (string.IsNullOrEmpty(orderId)) return false;

            if (stages == null || !stages.Any()) return false;

            var existingOrder = _orderRepository.GetById(orderId);
            if (existingOrder == null) return false;

            foreach (var stage in stages)
            {
                if (stage == OrderStatus.Pending && existingOrder.OrderStatus != OrderStatus.Pending) return false;
                if (stage == OrderStatus.Shipped && existingOrder.OrderStatus != OrderStatus.Pending) return false;
                if (stage == OrderStatus.Delivered && existingOrder.OrderStatus != OrderStatus.Shipped) return false;

                existingOrder.OrderStatus = stage;
                _orderRepository.Update(existingOrder);
            }

            return true;
        }

        /// <summary>
        /// Eğer stok belirli bir eşiğin altına düşerse fiyat artırılır, eğer stok fazlaysa fiyat indirilir.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="lowStockThreshold"></param>
        /// <param name="priceIncreasePercentage"></param>
        /// <param name="highStockThreshold"></param>
        /// <param name="priceDecreasePercentage"></param>
        /// <returns></returns>
        public bool AdjustProductPricingBasedOnStock(string productId, int lowStockThreshold, decimal priceIncreasePercentage, int highStockThreshold, decimal priceDecreasePercentage)
        {
            if (string.IsNullOrEmpty(productId)) return false;

            var product = _productRepository.GetById(productId);
            if (product == null) return false;

            if (product.StockQuantity <= lowStockThreshold)
            {
                product.Price += product.Price * (priceIncreasePercentage / 100);
            }
            else if (product.StockQuantity >= highStockThreshold)
            {
                product.Price -= product.Price * (priceDecreasePercentage / 100);
            }

            _productRepository.Update(product);
            return true;
        }

        public bool PartialShipOrder(string orderId, List<OrderItemModel> shippedItems)
        {
            if (string.IsNullOrEmpty(orderId)) return false;

            if (shippedItems == null || !shippedItems.Any()) return false;

            var existingOrder = _orderRepository.GetById(orderId);
            if (existingOrder == null || existingOrder.OrderStatus != OrderStatus.Pending) return false;

            foreach (var shippedItem in shippedItems)
            {
                var item = existingOrder.OrderItems.FirstOrDefault(i => i.ProductId == shippedItem.ProductId);
                if (item == null || item.Quantity < shippedItem.Quantity) return false; // Miktar fazla olamaz

                // Sevk edilen miktarı çıkar
                item.Quantity -= shippedItem.Quantity;

                // Eğer miktar sıfıra indiyse, ürünü siparişten çıkar
                if (item.Quantity == 0)
                {
                    existingOrder.OrderItems.Remove(item);
                }

                // Satış miktarını artır
                var product = _productRepository.GetById(item.ProductId);
                if (product != null)
                {
                    product.SalesCount += shippedItem.Quantity;
                    _productRepository.Update(product);
                }
            }

            // Eğer tüm ürünler sevk edildiyse sipariş tamamlandı olarak işaretlenir
            if (!existingOrder.OrderItems.Any()) existingOrder.OrderStatus = OrderStatus.Completed;

            return _orderRepository.Update(existingOrder);
        }

        public bool ReturnOrder(string orderId, List<OrderItemModel> returnedItems)
        {
            if (string.IsNullOrEmpty(orderId)) return false;

            if (returnedItems == null || !returnedItems.Any()) return false;

            bool productUpdate = true;

            var existingOrder = _orderRepository.GetById(orderId);
            if (existingOrder == null || existingOrder.OrderStatus != OrderStatus.Completed) return false;

            foreach (var returnedItem in returnedItems)
            {
                var item = existingOrder.OrderItems.FirstOrDefault(i => i.ProductId == returnedItem.ProductId);
                if (item == null || item.Quantity < returnedItem.Quantity) return false;

                var product = _productRepository.GetById(item.ProductId);
                if (product != null)
                {
                    // İade edilen miktarı düş
                    product.StockQuantity += returnedItem.Quantity;
                    product.SalesCount -= returnedItem.Quantity; // Satış sayısını azalt
                    productUpdate = _productRepository.Update(product);
                }
                if (!productUpdate)
                    return false;
            }
            existingOrder.OrderStatus = OrderStatus.Refunded;
            return _orderRepository.Update(existingOrder);
        }

        /// <summary>
        /// Satışı az olan ürünleri otomatik olarak devre dışı bırakır (discontinue)
        /// </summary>
        /// <param name="salesThreshold"></param>
        /// <returns></returns>
        public bool DiscontinueLowSellingProducts(int salesThreshold)
        {

            var products = _productRepository.GetAll();
            foreach (var product in products)
            {
                if (product.SalesCount < salesThreshold)
                {
                    product.IsDiscontinued = true; // Ürün satıştan kaldırılıyor
                    var updateResult = _productRepository.Update(product);

                    if (!updateResult)
                        return false;
                }
            }
            return true;
        }
    }
}