using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using MTOGO_Api_Service.Controllers;
using MTOGO_API_Service.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTOGO.Tests
{
    public class CustomerControllerTests
    {
        private readonly Mock<IDBManager> _mockDbManager;
        private readonly CustomerController _controller;

        public CustomerControllerTests()
        {
            _mockDbManager = new Mock<IDBManager>();
            _controller = new CustomerController(_mockDbManager.Object);
        }

        [Fact]
        public void AddNewCustomer_ValidCustomer_ReturnsOk()
        {
            // Arrange
            var customer = new Customer
            {
                CustomerId = ObjectId.GenerateNewId(),
                Name = "John Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "12345678",
                UserName = "johnny123",
                Password = "password123"
            };

            // Simuler AddCustomer-opkaldet i DBManager
            _mockDbManager.Setup(db => db.AddCustomer(It.IsAny<Customer>())).Verifiable();

            // Act
            var result = _controller.AddNewCustomer(customer);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result); // Access the Result property
            Assert.Equal(customer, okResult.Value);
        }

        [Fact]
        public void AddNewCustomer_InvalidCustomer_ThrowsException_ReturnsBadRequest()
        {
            // Arrange
            var customer = new Customer
            {
                CustomerId = ObjectId.GenerateNewId(),
                Name = "John Doe",
                Email = "invalid-email", // Ugyldig email
                PhoneNumber = "12345678",
                UserName = "johnny123",
                Password = "password123"
            };

            // Simulerer, at AddCustomer kaster en exception
            _mockDbManager.Setup(db => db.AddCustomer(It.IsAny<Customer>())).Throws(new Exception("Invalid customer data"));

            // Act
            var result = _controller.AddNewCustomer(customer);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result); // Access the Result property
            Assert.Equal("Error: Invalid customer data", badRequestResult.Value);
        }

        [Fact]
        public void GetCustomerById_ValidCustomer_ReturnsOk()
        {
            // Arrange
            var customerId = ObjectId.GenerateNewId().ToString();
            var customer = new Customer
            {
                CustomerId = ObjectId.Parse(customerId),
                Name = "John Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "12345678",
                UserName = "johnny123",
                Password = "password123"
            };

            // Simuler, at GetCustomerById returnerer en kunde
            _mockDbManager.Setup(db => db.GetCustomerById(It.IsAny<ObjectId>())).Returns(customer);

            // Act
            var result = _controller.GetCustomerById(customerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(customer, okResult.Value);
        }

        [Fact]
        public void GetCustomerById_CustomerNotFound_ReturnsNotFound()
        {
            // Arrange
            var customerId = ObjectId.GenerateNewId().ToString();

            // Simuler, at GetCustomerById returnerer null (kunde ikke fundet)
            _mockDbManager.Setup(db => db.GetCustomerById(It.IsAny<ObjectId>())).Returns((Customer)null);

            // Act
            var result = _controller.GetCustomerById(customerId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Customer not found.", notFoundResult.Value);
        }

        [Fact]
        public void DeleteCustomer_ValidCustomer_ReturnsOk()
        {
            // Arrange
            var customerId = ObjectId.GenerateNewId().ToString();

            // Simuler at DeleteCustomer bliver kaldt på DBManager
            _mockDbManager.Setup(db => db.DeleteCustomer(It.IsAny<ObjectId>())).Verifiable();

            // Act
            var result = _controller.DeleteCustomer(customerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Customer deleted successfully.", okResult.Value);
        }

        [Fact]
        public void UpdateCustomer_ValidCustomer_ReturnsOk()
        {
            // Arrange
            var customerId = ObjectId.GenerateNewId().ToString();
            var updatedCustomer = new Customer
            {
                Name = "John Updated",
                Email = "john.updated@example.com",
                PhoneNumber = "98765432",
                UserName = "johnnyUpdated",
                Password = "newpassword123"
            };

            // Simuler at UpdateCustomer bliver kaldt på DBManager
            _mockDbManager.Setup(db => db.UpdateCustomer(It.IsAny<ObjectId>(), It.IsAny<Customer>())).Verifiable();

            // Act
            var result = _controller.UpdateCustomer(customerId, updatedCustomer);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Customer updated successfully.", okResult.Value);
        }
    }

}
