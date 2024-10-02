using OrderStream.Application.Models;
using OrderStream.Domain.Enums;

namespace OrderStream.Application.Services
{
    public interface IOrderService
    {
        bool CreateOrder(OrderModel order);

        OrderModel GetOrderById(string id);

        IEnumerable<OrderModel> GetAllOrders();

        IEnumerable<OrderModel> GetOrdersByCustomer(int customerId);

        IEnumerable<OrderModel> GetPendingOrders();

        bool CancelOrder(string orderId);

        bool CompleteOrder(string orderId);

        bool UpdateOrderItems(string orderId, List<OrderItemModel> updatedItems);

        bool ChangeOrderStatus(string orderId, OrderStatus newStatus);

        bool RefundOrder(string orderId);

        bool ReopenOrder(string orderId);

        bool DeleteOrder(string orderId);

        bool ProcessOrderInStages(string orderId, List<OrderStatus> stages);

        bool AdjustProductPricingBasedOnStock(string productId, int lowStockThreshold, decimal priceIncreasePercentage, int highStockThreshold, decimal priceDecreasePercentage);

        bool PartialShipOrder(string orderId, List<OrderItemModel> shippedItems);

        bool ReturnOrder(string orderId, List<OrderItemModel> returnedItems);

        bool DiscontinueLowSellingProducts(int salesThreshold);
    }
}