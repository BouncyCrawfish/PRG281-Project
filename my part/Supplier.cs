using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG_281_Project
{
    public class Supplier
    {
        public int SupplierID { get; set; }
        public string Name { get; set; }
        public string ContactInfo { get; set; }
        public List<Product> ProductList { get; set; }

        public Supplier()
        {
            ProductList = new List<Product>();
        }

        public void AddProduct(Product product)
        {
            ProductList.Add(product);
        }

        public string GetSupplierDetails()
        {
            return "Supplier: " + Name + ", Contact: " + ContactInfo;
        }
    }
}
