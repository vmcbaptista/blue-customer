using AutoBogus;
using BlueCustomer.Api.Models;
using BlueCustomer.Core.Customers.Commands.Create;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Text;

namespace BlueCustomer.Api.IntegrationTests
{
    public class CustomerApiTests
    {
        private readonly string _endpoint = "/api/customer";
        private string _url;
        private string _connectionString;
        private HttpClient _httpClient;

        [SetUp]
        public void Setup()
        {
            _url = Environment.GetEnvironmentVariable("API_URL") + _endpoint;
            _connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            _httpClient = new HttpClient();
        }

        [Test]
        public async Task Demo_All_Endpoints()
        {
            var customerToInsert = new AutoFaker<CreateCustomer>().RuleFor(c => c.Email, f => f.Internet.Email()).Generate();

            var createResponse = await _httpClient.PostAsync($"{_url}", new StringContent(JsonConvert.SerializeObject(customerToInsert), Encoding.UTF8, "application/json"));
            createResponse.EnsureSuccessStatusCode();

            var createJsonResponse = await createResponse.Content.ReadAsStringAsync();
            var insertedCustomer = JsonConvert.DeserializeObject<CustomerDto>(createJsonResponse);

            insertedCustomer.Id.Should().Be(customerToInsert.Id);
            insertedCustomer.FirstName.Should().Be(customerToInsert.FirstName);

            var getByIdResponse = await _httpClient.GetAsync($"{_url}/{customerToInsert.Id}");
            getByIdResponse.EnsureSuccessStatusCode();

            var getByIdJsonResponse = await createResponse.Content.ReadAsStringAsync();
            var retrievedCustomer = JsonConvert.DeserializeObject<CustomerDto>(getByIdJsonResponse);

            retrievedCustomer.Id.Should().Be(customerToInsert.Id);
            retrievedCustomer.FirstName.Should().Be(customerToInsert.FirstName);

            var customerToUpdate = customerToInsert with { FirstName = $"{customerToInsert.FirstName} updated" };

            var updateResponse = await _httpClient.PutAsync($"{_url}/{customerToUpdate.Id}", new StringContent(JsonConvert.SerializeObject(customerToUpdate), Encoding.UTF8, "application/json"));
            updateResponse.EnsureSuccessStatusCode();

            var getAllResponse = await _httpClient.GetAsync($"{_url}");
            getAllResponse.EnsureSuccessStatusCode();

            var getAllJsonResponse = await getAllResponse.Content.ReadAsStringAsync();
            var customers = JsonConvert.DeserializeObject<List<CustomerDto>>(getAllJsonResponse);

            customers[0].Id.Should().Be(customerToUpdate.Id);
            customers[0].FirstName.Should().Be(customerToUpdate.FirstName);


            var deleteResponse = await _httpClient.DeleteAsync($"{_url}/{customerToUpdate.Id}");
            deleteResponse.EnsureSuccessStatusCode();

            var getAllResponseAfterDelete = await _httpClient.GetAsync($"{_url}");
            getAllResponseAfterDelete.EnsureSuccessStatusCode();

            var getAllJsonResponseAfterDelete = await getAllResponseAfterDelete.Content.ReadAsStringAsync();
            var customersAfterDelete = JsonConvert.DeserializeObject<List<CustomerDto>>(getAllJsonResponseAfterDelete);

            customersAfterDelete.Should().BeEmpty();
        }

        [TearDown]
        public void TearDown()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "truncate table customers";
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}