using AutoBogus;
using BlueCustomer.Api.Controllers;
using BlueCustomer.Api.Models;
using BlueCustomer.Core.Entities;
using BlueCustomer.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlueCustomer.Api.Tests
{
    public class CustomerControllerTests
    {
        private ICustomerRepository _customerRepository;
        private CustomerController _underTest;

        [SetUp]
        public void Setup()
        {
            _customerRepository = Substitute.For<ICustomerRepository>();
            _underTest = new CustomerController(_customerRepository);
        }

        [Test]
        public async Task Get_Shall_Return_Collection_Of_All_Customers()
        {
            var faker = new AutoFaker<Customer>();
            var customers = faker.Generate(3);
            var cancellationToken = new CancellationToken();
            _customerRepository.GetCustomers(cancellationToken).Returns(customers);

            var result = await _underTest.Get(cancellationToken);

            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            var customersList = okResult.Value.Should().BeAssignableTo<IEnumerable<CustomerDto>>().Subject.ToList();
            customersList.Count.Should().Be(3);
            for (int i = 0; i < 3; i++)
            {
                AssertCustomerToDtoMap(customers[i], customersList[i]);
            }
        }

        [Test]
        public async Task GetById_Shall_Return_The_Corresponding_Customer_When_Exists()
        {
            var faker = new AutoFaker<Customer>();
            var customer = faker.Generate();
            var cancellationToken = new CancellationToken();
            _customerRepository.GetCustomer(customer.Id, cancellationToken).Returns(customer);

            var result = await _underTest.GetById(customer.Id, cancellationToken);

            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            var customerDto = okResult.Value.Should().BeAssignableTo<CustomerDto>().Subject;
            AssertCustomerToDtoMap(customer, customerDto);

        }

        [Test]
        public async Task GetById_Shall_Return_Not_Found_When_The_Customer_Does_Not_Exists()
        {
            var userToFind = Guid.NewGuid();
            var cancellationToken = new CancellationToken();
            _customerRepository.GetCustomer(userToFind, cancellationToken).Returns((Customer?)null);

            var result = await _underTest.GetById(userToFind, cancellationToken);

            var okResult = result.Result.Should().BeOfType<NotFoundResult>().Subject;
            okResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);

        }
        private static void AssertCustomerToDtoMap(Customer customer, CustomerDto customerDto)
        {
            customerDto.Id.Should().Be(customer.Id);
            customerDto.FirstName.Should().Be(customer.Name.FirstName);
            customerDto.Surname.Should().Be(customer.Name.Surname);
            customerDto.Email.Should().Be(customer.Email.Value);
        }
    }
}