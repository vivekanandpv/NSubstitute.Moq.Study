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
            ICustomerService service = Substitute.For<ICustomerService>();


            service.Get(Arg.Any<int>())
                .Returns(new Customer { Email = "mock@gmail.com", Id = 0, Name = "Mr Mock" });


            CustomerController controller = new CustomerController(service);


            Assert.IsType<Customer>(controller.Get(13));
        }

        [Fact]
        public void ThrowArgumentExceptionForOddIdNSubstitute()
        {
            ICustomerService service = Substitute.For<ICustomerService>();


            service.Get(Arg.Is<int>(i => i % 2 != 0))
                .Throws(new ArgumentException("Invalid id"));


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


            CustomerController controller = new CustomerController(service);

            Assert.Throws<ArgumentException>(() => controller.Get(13));
        }
    }
}