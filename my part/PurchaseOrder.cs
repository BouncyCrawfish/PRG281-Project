using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG_281_Project
{
    public class PurchaseOrder : Order
    {
        public Supplier Supplier { get; set; }

        public void ReceiveOrder(Inventory inventory)
        {
            foreach (var product in ProductList)
            {
                inventory.AddProduct(product.Key, product.Value);
            }
            Status = "Received";
        }
    }
}
