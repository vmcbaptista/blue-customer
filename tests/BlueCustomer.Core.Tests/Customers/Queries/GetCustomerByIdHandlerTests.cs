using BlueCustomer.Core.Customers;
using BlueCustomer.Core.Customers.Errors;
using BlueCustomer.Core.Customers.Queries.GetById;
using BlueCustomer.Core.Customers.Repositories;
using BlueCustomer.Tests.Common;
using NSubstitute;

namespace BlueCustomer.Core.Tests.Customers.Queries
{
    [TestFixture]
    public class GetCustomerByIdHandlerTests
    {
        private ICustomerRepository _customerRepository;
        private GetCustomerByIdHandler _underTest;

        [SetUp]
        public void SetUp()
        {
            _customerRepository = Substitute.For<ICustomerRepository>();
            _underTest = new GetCustomerByIdHandler(_customerRepository);
        }

        [Test]
        public async Task Handle_Shall_Return_The_Corresponding_Customer_When_Exists()
        {
            var faker = new CustomerFaker();
            var customer = faker.Generate();
            var cancellationToken = new CancellationToken();
            _customerRepository.GetCustomer(customer.Id, cancellationToken).Returns(customer);

            var result = await _underTest.Handle(new GetCustomerById(customer.Id), cancellationToken);

            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            _customerRepository.Received(1).GetCustomer(customer.Id, cancellationToken);
        }

        [Test]
        public async Task Handle_Shall_Return_Not_Found_When_The_Customer_Does_Not_Exists()
        {
            var customerToFind = Guid.NewGuid();
            var cancellationToken = new CancellationToken();
            _customerRepository.GetCustomer(customerToFind, cancellationToken).Returns((Customer?)null);

            var result = await _underTest.Handle(new GetCustomerById(customerToFind), cancellationToken);


            result.Should().NotBeNull();
            result.HasError<CustomerNotFound>().Should().BeTrue();
            _customerRepository.Received(1).GetCustomer(customerToFind, cancellationToken);
        }
    }
}
