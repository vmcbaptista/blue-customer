using AutoBogus;
using BlueCustomer.Core.Customers;
using BlueCustomer.Core.Customers.Repositories;
using NSubstitute;
using BlueCustomer.Core.Customers.Commands.Delete;
using BlueCustomer.Core.Customers.Errors;

namespace BlueCustomer.Core.Tests.Customers.Commands.Delete
{
    [TestFixture]
    public class UpdateCustomerHandlerTests
    {
        private ICustomerRepository _customerRepository;
        private DeleteCustomerHandler _underTest;

        [SetUp]
        public void SetUp()
        {
            _customerRepository = Substitute.For<ICustomerRepository>();
            _underTest = new DeleteCustomerHandler(_customerRepository);
        }

        [Test]
        public async Task Handle_Shall_Return_Success_Result_When_Delete_Succeeds()
        {
            var customerToDelete = new AutoFaker<Customer>().Generate();
            var cancellationToken = new CancellationToken();
            _customerRepository.GetCustomer(customerToDelete.Id, cancellationToken).Returns(customerToDelete);
            _customerRepository.DeleteCustomer(customerToDelete, cancellationToken).Returns(Task.CompletedTask);

            var result = await _underTest.Handle(new DeleteCustomer(customerToDelete.Id), cancellationToken);

            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            _customerRepository.Received(1).DeleteCustomer(customerToDelete, cancellationToken);
        }

        [Test]
        public async Task Handle_Shall_Return_404_When_Attempting_Delete_Of_Unavailable_Customer()
        {
            var customerToDelete = new AutoFaker<Customer>().Generate();
            var cancellationToken = new CancellationToken();
            _customerRepository.GetCustomer(customerToDelete.Id, cancellationToken).Returns((Customer?)null);

            var result = await _underTest.Handle(new DeleteCustomer(customerToDelete.Id), cancellationToken);

            result.Should().NotBeNull();
            result.HasError<CustomerNotFound>().Should().BeTrue();
            _customerRepository.DidNotReceive().DeleteCustomer(Arg.Any<Customer>(), Arg.Any<CancellationToken>());
        }
    }
}
