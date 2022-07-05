using AutoBogus;
using BlueCustomer.Core.Customers.ValueObjects;
using BlueCustomer.Core.Customers;
using BlueCustomer.Core.Customers.Commands.Create;
using BlueCustomer.Core.Customers.Repositories;
using Microsoft.AspNetCore.DataProtection;
using NSubstitute;

namespace BlueCustomer.Core.Tests.Customers.Commands.Create
{
    [TestFixture]
    public class CreateCustomerHandlerTests
    {
        private ICustomerRepository _customerRepository;
        private IDataProtector _dataProtector;
        private CreateCustomerHandler _underTest;

        [SetUp]
        public void SetUp()
        {
            _customerRepository = Substitute.For<ICustomerRepository>();
            _dataProtector = Substitute.For<IDataProtector>();
            _underTest = new CreateCustomerHandler(_customerRepository, _dataProtector);
        }

        [Test]
        public async Task Handle_Shall_Return_Success_Result_When_Create_Succeeds()
        {
            var insertCustomerDto = new AutoFaker<CreateCustomer>().Generate();
            var insertCustomer = new Customer(insertCustomerDto.Id, new Name(insertCustomerDto.FirstName, insertCustomerDto.Surname), new Email(insertCustomerDto.Email), new Password(insertCustomerDto.Password));
            var cancellationToken = new CancellationToken();
            _customerRepository.CreateCustomer(Arg.Is<Customer>(c => c.Id == insertCustomerDto.Id), cancellationToken).Returns(Task.CompletedTask);

            var result = await _underTest.Handle(insertCustomerDto, cancellationToken);

            result.IsSuccess.Should().BeTrue();
            _dataProtector.Received(1).Protect(Arg.Any<byte[]>());
            _customerRepository.Received(1).CreateCustomer(Arg.Is<Customer>(c => c.Id == insertCustomerDto.Id), cancellationToken);
        }
    }
}
