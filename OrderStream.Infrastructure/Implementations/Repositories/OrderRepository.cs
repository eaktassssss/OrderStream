using MongoDB.Driver;
using OrderStream.Application.Interfaces.Repositories;
using OrderStream.Domain.Entities;

namespace OrderStream.Infrastructure.Implementations.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MongoDbContext _context;

        public OrderRepository(MongoDbContext context)
        {
            _context = context;
        }

        public bool Add(Order order)
        {
            try
            {
                _context.Orders.InsertOne(order);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Order GetById(string id)
        {
            return _context.Orders.Find(o => o.Id == id).FirstOrDefault();
        }

        public IEnumerable<Order> GetAll()
        {
            return _context.Orders.Find(_ => true).ToList();
        }

        public bool Update(Order order)
        {
            try
            {
                _context.Orders.ReplaceOne(o => o.Id == order.Id, order);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(string id)
        {
            try
            {
                _context.Orders.DeleteOne(o => o.Id == id);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}