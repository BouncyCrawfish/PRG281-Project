using PRG_281_Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG_281_Project
{
    public class Order
    {
        public int OrderID { get; set; }
        public Dictionary<Product, int> ProductList { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public double TotalCost { get; set; }

        public Order()
        {
            ProductList = new Dictionary<Product, int>();
            OrderDate = DateTime.Now;
            Status = "Pending";
            TotalCost = 0.0;
        }

        public void AddProduct(Product product, int quantity)
        {
            ProductList[product] = quantity;
            CalculateTotalCost();
        }

        public double CalculateTotalCost()
        {
            double total = 0.0;
            foreach (var product in ProductList)
            {
                total = total + (product.Key.Price * product.Value);
            }
            TotalCost = total;
            return TotalCost;
        }

        public void UpdateOrderStatus(string newStatus)
        {
            Status = newStatus;
            LogStatusChange(OrderID, newStatus);
        }

        public string GetOrderDetails()
        {
            return "Order " + OrderID + ": " + Status + ", Total: " + TotalCost;
        }

        private void LogStatusChange(int orderID, string newStatus)
        {
            Console.WriteLine($"Order {orderID} status changed to {newStatus}");
        }
    }
}
