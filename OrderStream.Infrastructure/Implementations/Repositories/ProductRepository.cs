namespace OrderStream.Infrastructure.Implementations.Repositories
{
    using MongoDB.Driver;
    using OrderStream.Application.Interfaces.Repositories;
    using OrderStream.Domain.Entities;

    public class ProductRepository : IProductRepository
    {
        private readonly MongoDbContext _context;

        public ProductRepository(MongoDbContext context)
        {
            _context = context;
        }

        public bool Add(Product product)
        {
            try
            {
                _context.Products.InsertOne(product);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Product GetById(string id)
        {
            return _context.Products.Find(p => p.Id == id).FirstOrDefault();
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products.Find(_ => true).ToList();
        }

        public bool Update(Product product)
        {
            try
            {
                _context.Products.ReplaceOne(p => p.Id == product.Id, product);
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
                _context.Products.DeleteOne(p => p.Id == id);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}