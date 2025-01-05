using MongoDB.Bson;
using System.Collections.Generic;

namespace MTOGO_API_Service.Data
{
    public interface IDBManager
    {
        // Add Methods
        void AddRestaurant(Restaurant restaurant);
        void AddMenuToRestaurant(ObjectId restaurantId, Menu menu);
        void AddMenuItemToRestaurantMenu(string restaurantId, ObjectId menuId, MenuItem menuItem);
        void AddOrder(Order order);
        void AddMenuItemsToOrder(ObjectId orderId, List<MenuItem> menuItems);
        void AddCustomer(Customer customer);
        void AddCourier(Courier courier);
        void AddDeliveriesToCourier(ObjectId courierId, List<ObjectId> deliveryIds);

        // Get Methods
        List<Customer> GetAllCustomers();
        Customer GetCustomerById(ObjectId customerId);
        List<Restaurant> GetAllRestaurants();
        Restaurant GetRestaurantById(ObjectId restaurantId);
        List<Order> GetAllOrders();
        Order GetOrderById(ObjectId orderId);
        Courier GetCourierByEmail(string email);

        // Update Methods
        void UpdateCustomer(ObjectId customerId, Customer updatedCustomer);
        void UpdateRestaurant(ObjectId restaurantId, Restaurant updatedRestaurant);
        void UpdateOrderInfo(ObjectId orderId, Order updatedOrder);
        void UpdateCourier(ObjectId courierId, Courier updatedCourier);
        void UpdateCourierStatus(ObjectId courierId, Courier updatedStatus);

        // Delete Methods
        void DeleteCustomer(ObjectId customerId);
        void DeleteRestaurant(ObjectId restaurantId);
        void DeleteOrder(ObjectId orderId);
        void DeleteCourier(ObjectId courierId);

        // Helper Methods
        bool IsCourierUnique(Courier courier);
    }
}
