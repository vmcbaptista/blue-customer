using AutoBogus;
using BlueCustomer.Core.Customers;
using BlueCustomer.Core.Customers.Repositories;
using NSubstitute;
using BlueCustomer.Core.Customers.Errors;
using BlueCustomer.Core.Customers.Commands.Update;
using Microsoft.AspNetCore.DataProtection;

namespace BlueCustomer.Core.Tests.Customers.Commands.Update
{
    [TestFixture]
    public class UpdateCustomerHandlerTests
    {
        private ICustomerRepository _customerRepository;
        private IDataProtector _dataProtector;
        private UpdateCustomerHandler _underTest;

        [SetUp]
        public void SetUp()
        {
            _customerRepository = Substitute.For<ICustomerRepository>();
            _dataProtector = Substitute.For<IDataProtector>();
            _underTest = new UpdateCustomerHandler(_customerRepository, _dataProtector);
        }

        [Test]
        public async Task Handle_Shall_Return_Success_Result_When_Update_Succeeds()
        {
            var originalCustomer = new AutoFaker<Customer>().Generate();
            var customerToUpdate = new AutoFaker<UpdateCustomer>().RuleFor(c => c.Id, originalCustomer.Id).Generate();
            var cancellationToken = new CancellationToken();
            _customerRepository.GetCustomer(customerToUpdate.Id, cancellationToken).Returns(originalCustomer);
            _customerRepository.UpdateCustomer(Arg.Is<Customer>(c => c.Id == customerToUpdate.Id), cancellationToken).Returns(Task.CompletedTask);

            var result = await _underTest.Handle(customerToUpdate, cancellationToken);

            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            _customerRepository.Received(1).UpdateCustomer(Arg.Is<Customer>(c => c.Id == customerToUpdate.Id), cancellationToken);

        }

        [Test]
        public async Task Handle_Shall_Return_Not_Found_When_Attempting_Update_Of_Unavailable_Customer()
        {
            var customerToUpdate = new AutoFaker<UpdateCustomer>().Generate();
            var cancellationToken = new CancellationToken();
            _customerRepository.GetCustomer(customerToUpdate.Id, cancellationToken).Returns((Customer?)null);

            var result = await _underTest.Handle(customerToUpdate, cancellationToken);

            result.Should().NotBeNull();
            result.HasError<CustomerNotFound>().Should().BeTrue();
            _customerRepository.DidNotReceive().UpdateCustomer(Arg.Any<Customer>(), Arg.Any<CancellationToken>());
        }
    }
}
