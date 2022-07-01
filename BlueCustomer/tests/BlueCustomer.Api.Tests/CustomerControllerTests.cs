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
                customersList[i].Id.Should().Be(customers[i].Id);
                customersList[i].FirstName.Should().Be(customers[i].Name.FirstName);
                customersList[i].Surname.Should().Be(customers[i].Name.Surname);
                customersList[i].Email.Should().Be(customers[i].Email.Value);
            }
        }
    }
}