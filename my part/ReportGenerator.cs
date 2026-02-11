using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG_281_Project
{
    public class ReportGenerator
    {
        public Inventory Inventory { get; set; }
        public List<Order> Orders { get; set; }

        public ReportGenerator()
        {
            Orders = new List<Order>();
        }

        public string GenerateSalesReport(DateTime startDate, DateTime endDate)
        {
            double totalSales = 0.0;
            string salesData = "";
            foreach (var order in Orders)
            {
                if (order.OrderDate >= startDate && order.OrderDate <= endDate)
                {
                    totalSales = totalSales + order.TotalCost;
                    salesData = salesData + order.GetOrderDetails() + "\n";
                }
            }
            return "Sales Report: Total = " + totalSales + "\n" + salesData;
        }

        public string GenerateLowStockReport(int threshold)
        {
            string report = "Low Stock Items:\n";
            foreach (var product in Inventory.ProductList)
            {
                if (product.Value < threshold)
                {
                    report = report + product.Key.Name + ": " + product.Value + " units\n";
                }
            }
            return report;
        }


        public string GenerateInventorySummary()
        {
            int totalItems = Inventory.ProductList.Count;
            double totalValue = Inventory.CalculateInventoryValue();
            return "Inventory Summary: " + totalItems + " products, Value: " + totalValue;
        }
    }
}
