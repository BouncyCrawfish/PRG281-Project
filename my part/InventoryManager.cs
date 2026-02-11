using PRG_281_Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG_281_Project
{
    public class InventoryManager
    {
        public Inventory Inventory { get; set; }
        public List<Customer> Customers { get; set; }
        public List<Supplier> Suppliers { get; set; }
        public List<Order> Orders { get; set; }

        // EVENTS
        public event ProductEventHandler ProductAdded;
        public event ProductEventHandler ProductRemoved;
        public event OrderEventHandler OrderProcessed;

        public InventoryManager()
        {
            Inventory = new Inventory();
            Customers = new List<Customer>();
            Suppliers = new List<Supplier>();
            Orders = new List<Order>();
        }

        public void ProcessCustomerOrder(Customer customer, Order order)
        {
            foreach (var product in order.ProductList)
            {
                if (Inventory.GetProductQuantity(product.Key.ProductID) >= product.Value)
                {
                    int newQuantity = Inventory.GetProductQuantity(product.Key.ProductID) - product.Value;
                    Inventory.UpdateProductQuantity(product.Key.ProductID, newQuantity);
                }
                else
                {
                    order.Status = "Backorder";
                    return;
                }
            }
            order.Status = "Fulfilled";
            Orders.Add(order);
            OrderProcessed?.Invoke(order.OrderID, order.TotalCost);
        }

        public string CheckInventoryStatus()
        {
            string lowStockItems = "";
            foreach (var product in Inventory.ProductList)
            {
                if (product.Value < 10)
                {
                    lowStockItems = lowStockItems + product.Key.Name + " ";
                }
            }
            return lowStockItems;
        }

        public void AddCustomer(Customer customer)
        {
            Customers.Add(customer);
        }

        public void AddSupplier(Supplier supplier)
        {
            Suppliers.Add(supplier);
        }

        // Additional methods for product management
        public void AddProductToInventory(Product product, int quantity)
        {
            Inventory.AddProduct(product, quantity);
            ProductAdded?.Invoke(product.Name, "Added");
        }

        public void RemoveProductFromInventory(int productID)
        {
            var product = Inventory.ProductList.Keys.FirstOrDefault(p => p.ProductID == productID);
            if (product != null)
            {
                Inventory.RemoveProduct(productID);
                ProductRemoved?.Invoke(product.Name, "Removed");
            }
        }
    }
}
