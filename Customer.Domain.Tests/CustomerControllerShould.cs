using Moq;
using NSubstitute;
using Xunit.Abstractions;

namespace Customer.Domain.Tests
{
    public class CustomerControllerShould
    {
        private readonly ITestOutputHelper _helper;

        public CustomerControllerShould(ITestOutputHelper helper)
        {
            _helper = helper;
        }

        [Fact]
        public void GetMockCreatedInNSubstitute()
        {
            //  We first create the dependency
            ICustomerService service = Substitute.For<ICustomerService>();

            //  Dependency is now injected manually
            CustomerController controller = new CustomerController(service);

            //  An implementation is automatically created
            //  Type is: Castle.Proxies.ObjectProxy
            _helper.WriteLine(service.GetType().ToString());

            Assert.NotNull(service);
            Assert.IsAssignableFrom<ICustomerService>(service);
        }

        [Fact]
        public void GetMockCreatedInMoq()
        {
            var mockWrapper = new Mock<ICustomerService>();
            ICustomerService service = mockWrapper.Object;

            //  Dependency is now injected manually
            CustomerController controller = new CustomerController(service);

            //  An implementation is automatically created
            //  Type is: Castle.Proxies.ICustomerServiceProxy
            _helper.WriteLine(service.GetType().ToString());

            Assert.NotNull(service);
            Assert.IsAssignableFrom<ICustomerService>(service);
        }
    }
}