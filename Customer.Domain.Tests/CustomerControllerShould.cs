using System.Text.RegularExpressions;
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
        public void ReturnCustomerForAnyIntIdMoq()
        {
            var mockWrapper = new Mock<ICustomerService>();

            //  Mock can be configured for multiple times
            mockWrapper
                .Setup(s => s.Get(It.IsInRange(10, 100, Range.Exclusive)))
                .Returns(new Customer { Email = "mock@gmail.com", Id = 0, Name = "Mr Mock" });

            mockWrapper
                .Setup(s => s.Get(It.IsRegex("[0-9]{4}$", RegexOptions.IgnoreCase)))
                .Returns(new Customer { Email = "mock@gmail.com", Id = 0, Name = "Mr Mock" });

            //  Please consider: https://github.com/Moq/moq4/wiki/Quickstart#matching-arguments
            //It.Is<T>();
            //It.IsIn();  //  for collection
            //It.IsInRange(); //  for range (inclusive or exclusive)
            //It.IsNotIn();   //  for collection
            //It.IsNotNull<T>();  //  not null constraint
            //It.IsRegex();   //  regex matching

            ICustomerService service = mockWrapper.Object;


            CustomerController controller = new CustomerController(service);


            Assert.IsType<Customer>(controller.Get(13));
            Assert.IsType<Customer>(controller.Get("Rajesh1245"));
        }
    }
}