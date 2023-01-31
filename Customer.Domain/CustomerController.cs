namespace Customer.Domain
{
    public class CustomerController
    {
        //  dependency
        private readonly ICustomerService _customerService;

        //  inverting the dependency
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public Customer Get(int id)
        {
            return _customerService.Get(id);    //  delegation
        }

        public Customer Get(string name)
        {
            return _customerService.Get(name);    //  delegation
        }

        public void Save(Customer customer)
        {
            _customerService.Save(customer);
        }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public interface ICustomerService
    {
        void Save(Customer customer);
        Customer Get(int id);
        Customer Get(string name);
    }
}