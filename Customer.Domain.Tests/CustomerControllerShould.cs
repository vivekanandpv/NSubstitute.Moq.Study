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
        public void EventRaisingInNSubstituteWithManualApproach()
        {
            ICustomerService service = Substitute.For<ICustomerService>();

            bool eventRaised = false;

            service.CustomerSaved += (sender, args) =>
            {
                eventRaised = true;
            };

            service.CustomerSaved += Raise.EventWith<EventArgs>(new object(), new EventArgs());

            //  service.CustomerSaved += Raise.Event(); //  For simple cases

            Assert.True(eventRaised);
        }

        [Fact]
        public void EventRaisingInNSubstituteWithXUnitApproach()
        {
            ICustomerService service = Substitute.For<ICustomerService>();

            Assert.Raises<EventArgs>(
                handler => service.CustomerSaved += handler,
                handler => service.CustomerSaved -= handler,
                () =>
                {
                    service.CustomerSaved += Raise.EventWith<EventArgs>(new object(), new EventArgs());
                });
        }

        [Fact]
        public void EventRaisingWithMoq()
        {
            var mockWrapper = new Mock<ICustomerService>();


            ICustomerService service = mockWrapper.Object;

            CustomerController controller = new CustomerController(service);

            Assert.Raises<EventArgs>(
                handler => service.CustomerSaved += handler,
                handler => service.CustomerSaved -= handler,
                () =>
                {
                    mockWrapper.Raise(s => s.CustomerSaved += null, new EventArgs());
                });
        }
    }
}