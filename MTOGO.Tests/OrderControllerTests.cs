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
    public class OrderControllerTests
    {
        private readonly Mock<IDBManager> _mockDbManager;
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            _mockDbManager = new Mock<IDBManager>();
            _controller = new OrderController(_mockDbManager.Object);
        }

        [Fact]
        public void AddOrder_ReturnsOk()
        {
            // Arrange
            var order = new Order
            {
                OrderId = ObjectId.GenerateNewId(),
                CustomerId = ObjectId.GenerateNewId(),
                Status = "Pending",
                OrderDate = DateTime.UtcNow
            };

            // Simuler AddOrder-opkaldet i DBManager
            _mockDbManager.Setup(db => db.AddOrder(It.IsAny<Order>())).Verifiable();

            // Act
            var result = _controller.AddOrder(order);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Order added successfully.", okResult.Value);
        }

        [Fact]
        public void AddOrder_ReturnsBadRequest()
        {
            // Arrange
            var order = new Order
            {
                OrderId = ObjectId.GenerateNewId(),
                CustomerId = ObjectId.GenerateNewId(),
                Status = "Invalid Status", // Eksempel på et ugyldigt ordrestatus
                OrderDate = DateTime.UtcNow
            };

            // Simulerer, at AddOrder kaster en exception
            _mockDbManager.Setup(db => db.AddOrder(It.IsAny<Order>())).Throws(new Exception("Invalid order data"));

            // Act
            var result = _controller.AddOrder(order);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error: Invalid order data", badRequestResult.Value);
        }

        [Fact]
        public void AddMenuItemsToOrder_ReturnsOk()
        {
            // Arrange
            var orderId = ObjectId.GenerateNewId().ToString();
            var menuItems = new List<MenuItem>
        {
            new MenuItem { MenuItemId = ObjectId.GenerateNewId(), MenuItemName = "Test Dish", Price = 10.0 },
            new MenuItem { MenuItemId = ObjectId.GenerateNewId(), MenuItemName = "Another Dish", Price = 15.0 }
        };

            _mockDbManager.Setup(db => db.AddMenuItemsToOrder(It.IsAny<ObjectId>(), It.IsAny<List<MenuItem>>())).Verifiable();

            // Act
            var result = _controller.AddMenuItemsToOrder(orderId, menuItems);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Menu items added to order successfully.", okResult.Value);
        }

        [Fact]
        public void GetOrderById_OrderNotFound_ReturnsNotFound()
        {
            // Arrange
            var orderId = ObjectId.GenerateNewId().ToString();

            // Simuler at GetOrderById returnerer null (ordre ikke fundet)
            _mockDbManager.Setup(db => db.GetOrderById(It.IsAny<ObjectId>())).Returns((Order)null);

            // Act
            var result = _controller.GetOrderById(orderId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Order not found.", notFoundResult.Value);
        }

        [Fact]
        public void DeleteOrder_ValidOrder_ReturnsOk()
        {
            // Arrange
            var orderId = ObjectId.GenerateNewId().ToString();

            // Simuler at DeleteOrder bliver kaldt på DBManager
            _mockDbManager.Setup(db => db.DeleteOrder(It.IsAny<ObjectId>())).Verifiable();

            // Act
            var result = _controller.DeleteOrder(orderId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Order deleted successfully.", okResult.Value);
        }

        [Fact]
        public void UpdateOrderInfo_ValidOrder_ReturnsOk()
        {
            // Arrange
            var orderId = ObjectId.GenerateNewId().ToString();
            var updatedOrder = new Order
            {
                Status = "Completed",
                OrderDate = DateTime.UtcNow
            };

            // Simuler at UpdateOrderInfo bliver kaldt på DBManager
            _mockDbManager.Setup(db => db.UpdateOrderInfo(It.IsAny<ObjectId>(), It.IsAny<Order>())).Verifiable();

            // Act
            var result = _controller.UpdateOrderInfo(orderId, updatedOrder);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Order information updated successfully.", okResult.Value);
        }
    }


}
