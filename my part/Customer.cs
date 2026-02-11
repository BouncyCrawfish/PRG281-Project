using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG_281_Project
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<Order> OrderHistory { get; set; }

        public Customer()
        {
            OrderHistory = new List<Order>();
        }

        public void PlaceOrder(Order order)
        {
            OrderHistory.Add(order);
            ProcessOrder(order);
        }

        public List<Order> GetOrderHistory()
        {
            return OrderHistory;
        }

        private void ProcessOrder(Order order)
        {
            Console.WriteLine($"Processing order {order.OrderID} for customer {Name}");
        }
    }
}
