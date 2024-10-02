

using OrderStream.Domain.Entities;

namespace OrderStream.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        bool Add(Product product);

        Product GetById(string id);

        IEnumerable<Product> GetAll();

        bool Update(Product product);

        bool Delete(string id);
    }
}