
using OrderStream.Domain.Entities;
namespace OrderStream.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        bool Add(Order order);

        Order GetById(string id);

        IEnumerable<Order> GetAll();

        bool Update(Order order);

        bool Delete(string id);
    }
}