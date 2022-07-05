using AutoBogus;
using BlueCustomer.Api.Controllers;
using BlueCustomer.Api.Models;
using BlueCustomer.Core.Customers;
using BlueCustomer.Core.Customers.Commands.Create;
using BlueCustomer.Core.Customers.Commands.Delete;
using BlueCustomer.Core.Customers.Commands.Update;
using BlueCustomer.Core.Customers.Errors;
using BlueCustomer.Core.Customers.Queries.GetAll;
using BlueCustomer.Core.Customers.Queries.GetById;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlueCustomer.Api.Tests
{
    [TestFixture]
    public class CustomerControllerTests
    {
        private IUpdateCustomerHandler _updateCommandHandler;
        private IGetCustomerByIdHandler _getCustomerByIdHandler;
        private IGetAllCustomersHandler _getAllCustomersHandler;
        private ICreateCustomerHandler _createCustomerHandler;
        private IDeleteCustomerHandler _deleteCustomerHandler;
        private CustomerController _underTest;

        [SetUp]
        public void Setup()
        {
            _updateCommandHandler = Substitute.For<IUpdateCustomerHandler>();
            _getCustomerByIdHandler = Substitute.For<IGetCustomerByIdHandler>();
            _getAllCustomersHandler = Substitute.For<IGetAllCustomersHandler>();
            _createCustomerHandler = Substitute.For<ICreateCustomerHandler>();
            _deleteCustomerHandler = Substitute.For<IDeleteCustomerHandler>();
            _underTest = new CustomerController(_updateCommandHandler, _getCustomerByIdHandler, _createCustomerHandler, _deleteCustomerHandler, _getAllCustomersHandler);
        }

        [Test]
        public async Task Get_Shall_Return_Collection_Of_All_Customers()
        {
            var faker = new AutoFaker<Customer>();
            var customers = faker.Generate(3);
            var cancellationToken = new CancellationToken();
            _getAllCustomersHandler.Handle(new GetAllCustomers(), cancellationToken).Returns(customers);

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
            _getCustomerByIdHandler.Handle(new GetCustomerById(customer.Id), cancellationToken).Returns(customer);

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
            _getCustomerByIdHandler.Handle(new GetCustomerById(userToFind), cancellationToken).Returns(Result.Fail(new CustomerNotFound()));

            var result = await _underTest.GetById(userToFind, cancellationToken);

            var notFoundResult = result.Result.Should().BeOfType<NotFoundResult>().Subject;
            notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Test]
        public async Task Post_Shall_Return_201_When_Create_Succeeds()
        {
            var insertCustomerDto = new AutoFaker<CreateCustomer>().Generate();
            var cancellationToken = new CancellationToken();
            _createCustomerHandler.Handle(insertCustomerDto, cancellationToken).Returns(Result.Ok());

            var result = await _underTest.Post(insertCustomerDto, cancellationToken);

            var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.StatusCode.Should().Be(StatusCodes.Status201Created);
            var customerDto = createdResult.Value.Should().BeOfType<CustomerDto>().Subject;
            customerDto.Id.Should().Be(insertCustomerDto.Id);
            customerDto.FirstName.Should().Be(insertCustomerDto.FirstName);
            customerDto.Surname.Should().Be(insertCustomerDto.Surname);
            customerDto.Email.Should().Be(insertCustomerDto.Email);
        }

        [Test]
        public async Task Delete_Shall_Return_204_When_Delete_Succeeds()
        {
            var customerToDelete = Guid.NewGuid();
            var cancellationToken = new CancellationToken();
            _deleteCustomerHandler.Handle(new DeleteCustomer(customerToDelete), cancellationToken).Returns(Result.Ok());

            var result = await _underTest.DeleteAsync(customerToDelete, cancellationToken);

            var createdResult = result.Should().BeOfType<NoContentResult>().Subject;
            createdResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Test]
        public async Task Delete_Shall_Return_404_When_Attempting_Delete_Of_Unavailable_Customer()
        {
            var customerToDelete = Guid.NewGuid();
            var cancellationToken = new CancellationToken();
            _deleteCustomerHandler.Handle(new DeleteCustomer(customerToDelete), cancellationToken).Returns(Result.Fail(new CustomerNotFound()));

            var result = await _underTest.DeleteAsync(customerToDelete, cancellationToken);

            var createdResult = result.Should().BeOfType<NotFoundResult>().Subject;
            createdResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Test]
        public async Task Put_Shall_Return_204_When_Update_Succeeds()
        {
            var customerToUpdate = new AutoFaker<UpdateCustomer>().Generate();
            var cancellationToken = new CancellationToken();
            _updateCommandHandler.Handle(customerToUpdate, cancellationToken).Returns(Result.Ok());

            var result = await _underTest.PutAsync(customerToUpdate.Id, customerToUpdate, cancellationToken);

            var createdResult = result.Should().BeOfType<NoContentResult>().Subject;
            createdResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Test]
        public async Task Put_Shall_Return_400_When_Route_Id_Does_Not_Match_Body_Id()
        {
            var customerToUpdate = new AutoFaker<UpdateCustomer>().Generate();
            var cancellationToken = new CancellationToken();

            var result = await _underTest.PutAsync(Guid.NewGuid(), customerToUpdate, cancellationToken);

            var createdResult = result.Should().BeOfType<BadRequestResult>().Subject;
            createdResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Test]
        public async Task Put_Shall_Return_404_When_Attempting_Update_Of_Unavailable_Customer()
        {
            var customerToUpdate = new AutoFaker<UpdateCustomer>().Generate();
            var cancellationToken = new CancellationToken();
            _updateCommandHandler.Handle(customerToUpdate, cancellationToken).Returns(Result.Fail(new CustomerNotFound()));

            var result = await _underTest.PutAsync(customerToUpdate.Id, customerToUpdate, cancellationToken);

            var createdResult = result.Should().BeOfType<NotFoundResult>().Subject;
            createdResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
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