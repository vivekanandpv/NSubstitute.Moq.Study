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
        public void SubsequentReturnsInNSubstitute()
        {
            ICustomerService service = Substitute.For<ICustomerService>();

            service.Get(Arg.Any<int>())
                .Returns(
                    _ => new Customer { Email = "mock@gmail.com", Id = 1, Name = "Mr Mock" }, 
                    _ => new Customer { Email = "mock2@gmail.com", Id = 2, Name = "Mr Mock2" }, 
                    _ => new Customer { Email = "mock3@gmail.com", Id = 3, Name = "Mr Mock3" },
                    _ => throw new ArgumentException("Invalid id")
                    );   //  throws at 4th invocation

            CustomerController controller = new CustomerController(service);

            Assert.Equal(1, controller.Get(12).Id);
            Assert.Equal(2, controller.Get(12).Id);
            Assert.Equal(3, controller.Get(12).Id);
            Assert.Throws<ArgumentException>(() => controller.Get(12));
        }

        [Fact]
        public void SubsequentReturnsInMoq()
        {
            var mockWrapper = new Mock<ICustomerService>();

            mockWrapper
                .SetupSequence(s => s.Get(It.IsAny<int>()))
                .Returns(new Customer { Email = "mock@gmail.com", Id = 1, Name = "Mr Mock" })
                .Returns(new Customer { Email = "mock2@gmail.com", Id = 2, Name = "Mr Mock2" })
                .Returns(new Customer { Email = "mock3@gmail.com", Id = 3, Name = "Mr Mock3" })
                .Throws(new ArgumentException("Invalid id"));   //  Throws at 4th invocation

            ICustomerService service = mockWrapper.Object;

            CustomerController controller = new CustomerController(service);

            Assert.Equal(1, controller.Get(12).Id);
            Assert.Equal(2, controller.Get(12).Id);
            Assert.Equal(3, controller.Get(12).Id);
            Assert.Throws<ArgumentException>(() => controller.Get(12));
        }
    }
}