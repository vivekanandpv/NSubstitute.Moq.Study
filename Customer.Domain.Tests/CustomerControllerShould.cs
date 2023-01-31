using Moq;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit.Abstractions;
using Range = Moq.Range;

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
        public void VerificationNSubstitute()
        {
            ICustomerService service = Substitute.For<ICustomerService>();

            service.Get(Arg.Any<int>())
                .Returns(
                    _ => new Customer { Email = "mock@gmail.com", Id = 1, Name = "Mr Mock" }
                );

            CustomerController controller = new CustomerController(service);

            Customer customer = controller.Get(12);

            service.Received().Get(Arg.Any<int>());
            service.DidNotReceive().Get(Arg.Is<int>(i => i > 100));
            service.Received(1).Get(12);
            service.Received(1).Get(Arg.Is<int>(i => i > 10));
        }

        [Fact]
        public void SubsequentReturnsInMoq()
        {
            var mockWrapper = new Mock<ICustomerService>();

            mockWrapper
                .Setup(s => s.Get(It.IsAny<int>()))
                .Returns(new Customer { Email = "mock@gmail.com", Id = 1, Name = "Mr Mock" });   //  Throws at 4th invocation

            ICustomerService service = mockWrapper.Object;

            CustomerController controller = new CustomerController(service);

            controller.Get(12);

            mockWrapper.Verify(s => s.Get(It.IsAny<int>()), Times.Exactly(1));
            
            //  Other API for times specification
            //Times.AtLeast(4);
            //Times.AtLeastOnce();
            //Times.AtMost(2);
            //Times.AtMostOnce();
            //Times.Between(4, 8, Range.Inclusive);   //  Also Exclusive
            //Times.Never();
            //Times.Once();
        }
    }
}