using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG_281_Project
{
    public interface IProduct
    {
        int ProductID { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        double Price { get; set; }
        string Category { get; set; }
        void UpdatePrice(double newPrice);
        string GetProductDetails();
    }
}
