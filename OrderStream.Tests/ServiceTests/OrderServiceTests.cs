using OrderStream.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderStream.Tests.ServiceTests
{
    public class OrderServiceTests:IClassFixture<OrderServiceTestFixture>
    {
        private readonly OrderServiceTestFixture _fixture;
        public OrderServiceTests(OrderServiceTestFixture fixture)
        {
            _fixture = fixture;
        }
    }
}
