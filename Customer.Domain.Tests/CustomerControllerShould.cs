using Moq;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
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
        public void ReturnCustomerForAnyIntIdNSubstitute()
        {
            //  We first create the dependency
            ICustomerService service = Substitute.For<ICustomerService>();

            //  Configuring the mock to return an object
            service.Get(Arg.Any<int>())
                .Returns(new Customer { Email = "mock@gmail.com", Id = 0, Name = "Mr Mock" });

            //  Dependency is now injected manually
            CustomerController controller = new CustomerController(service);


            Assert.IsType<Customer>(controller.Get(13));
        }

        [Fact]
        public void ThrowArgumentExceptionForOddIdNSubstitute()
        {
            //  We first create the dependency
            ICustomerService service = Substitute.For<ICustomerService>();

            //  Custom configuration to throw exception
            service.Get(Arg.Is<int>(i => i % 2 != 0))
                .Throws(new ArgumentException("Invalid id"));

            //  Dependency is now injected manually
            CustomerController controller = new CustomerController(service);

            Assert.Throws<ArgumentException>(() => controller.Get(13));
        }

        [Fact]
        public void ReturnCustomerForAnyIntIdMoq()
        {
            var mockWrapper = new Mock<ICustomerService>();
            mockWrapper
                .Setup(s => s.Get(It.IsAny<int>()))
                .Returns(new Customer { Email = "mock@gmail.com", Id = 0, Name = "Mr Mock" });

            ICustomerService service = mockWrapper.Object;

            //  Dependency is now injected manually
            CustomerController controller = new CustomerController(service);


            Assert.IsType<Customer>(controller.Get(13));
        }

        [Fact]
        public void ThrowArgumentExceptionForOddIdMoq()
        {
            var mockWrapper = new Mock<ICustomerService>();
            mockWrapper
                .Setup(s => s.Get(It.Is<int>(i => i % 2 != 0)))
                .Throws(new ArgumentException("Invalid id"));

            ICustomerService service = mockWrapper.Object;

            //  Dependency is now injected manually
            CustomerController controller = new CustomerController(service);

            Assert.Throws<ArgumentException>(() => controller.Get(13));
        }
    }
}