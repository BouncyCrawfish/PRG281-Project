using PRG_281_Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG_281_Project
{
    public class Product : IProduct
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }

        public void UpdatePrice(double newPrice)
        {
            Price = newPrice;
            LogPriceChange(ProductID, newPrice);
        }

        public string GetProductDetails()
        {
            return "ID: " + ProductID + ", Name: " + Name + ", Price: " + Price;
        }

        private void LogPriceChange(int productID, double newPrice)
        {
            Console.WriteLine($"Price changed for Product {productID} to {newPrice}");
        }
    }
}
