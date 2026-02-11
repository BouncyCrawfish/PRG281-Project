using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG_281_Project
{
    public class Inventory
    {
        public Dictionary<Product, int> ProductList { get; set; }
        public double TotalInventoryValue { get; set; }

        public Inventory()
        {
            ProductList = new Dictionary<Product, int>();
            TotalInventoryValue = 0.0;
        }

        public void AddProduct(Product product, int quantity)
        {
            if (ProductList.ContainsKey(product))
            {
                ProductList[product] = ProductList[product] + quantity;
            }
            else
            {
                ProductList[product] = quantity;
            }
            UpdateInventoryValue();
        }

        public void RemoveProduct(int productID)
        {
            Product product = FindProductByID(productID);
            if (product != null)
            {
                ProductList.Remove(product);
                UpdateInventoryValue();
            }
        }

        public void UpdateProductQuantity(int productID, int newQuantity)
        {
            Product product = FindProductByID(productID);
            if (product != null)
            {
                ProductList[product] = newQuantity;
                UpdateInventoryValue();
            }
        }

        public int GetProductQuantity(int productID)
        {
            Product product = FindProductByID(productID);
            if (product != null && ProductList.ContainsKey(product))
            {
                return ProductList[product];
            }
            return 0;
        }

        public double CalculateInventoryValue()
        {
            double totalValue = 0.0;
            foreach (var product in ProductList)
            {
                totalValue = totalValue + (product.Key.Price * product.Value);
            }
            return totalValue;
        }

        public void UpdateInventoryValue()
        {
            TotalInventoryValue = CalculateInventoryValue();
        }

        private Product FindProductByID(int productID)
        {
            return ProductList.Keys.FirstOrDefault(p => p.ProductID == productID);
        }
    }
}
