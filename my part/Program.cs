using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PRG_281_Project
{
    // CUSTOM EXCEPTIONS
    public class InvalidLoginException : Exception
    {
        public InvalidLoginException(string message) : base(message) { }
    }

    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException(string message) : base(message) { }
    }

    public class InsufficientStockException : Exception
    {
        public InsufficientStockException(string message) : base(message) { }
    }

    // DELEGATES AND EVENTS
    public delegate void ProductEventHandler(string productName, string action);
    public delegate void OrderEventHandler(int orderID, double totalCost);
    public delegate void LoginEventHandler(string username, bool successful);

    // MAIN PROGRAM CLASS
    internal class Program
    {
        enum Menu_Page
        {
            Login = 1,
            Add_Product,
            Process_Order,
            Generate_Report,
            Logout
        }

        // STATIC FIELDS
        private static InventoryManager system = new InventoryManager();
        private static ReportGenerator reportGen = new ReportGenerator();
        private static User currentUser = new User();
        private static bool applicationRunning = true;

        // EVENT HANDLERS
        static void OnProductAdded(string productName, string action)
        {
            Console.WriteLine($"EVENT: Product '{productName}' was {action}");
        }

        static void OnProductRemoved(string productName, string action)
        {
            Console.WriteLine($"EVENT: Product '{productName}' was {action}");
        }

        static void OnOrderProcessed(int orderID, double totalCost)
        {
            Console.WriteLine($"EVENT: Order {orderID} processed with total cost {totalCost}");
        }

        static void OnLoginAttempt(string username, bool successful)
        {
            string status = successful ? "SUCCESSFUL" : "FAILED";
            Console.WriteLine($"EVENT: Login attempt for '{username}' was {status}");
        }

        static Program()
        {
            // Subscribe to events
            system.ProductAdded += OnProductAdded;
            system.ProductRemoved += OnProductRemoved;
            system.OrderProcessed += OnOrderProcessed;

            // Initialize system data
            reportGen.Inventory = system.Inventory;
            reportGen.Orders = system.Orders;

            // Initialize test users
            system.Customers.Add(new Customer { CustomerID = 1, Name = "Test Customer", Email = "test@example.com" });

            // Set up default user
            currentUser.UserID = 1;
            currentUser.Username = "Admin";
            currentUser.Password = "1234";
        }

        static void Add_Product()
        {
            try
            {
                if (!currentUser.HasPermission("inventory_management"))
                {
                    Console.WriteLine("Access denied: Insufficient permissions");
                    return;
                }

                Console.WriteLine("=== ADD PRODUCT ===");

                Console.Write("Enter Product ID: ");
                int productID = int.Parse(Console.ReadLine());

                Console.Write("Enter Product Name: ");
                string productName = Console.ReadLine();

                Console.Write("Enter Description: ");
                string description = Console.ReadLine();

                Console.Write("Enter Price: ");
                double price = double.Parse(Console.ReadLine());

                Console.Write("Enter Category: ");
                string category = Console.ReadLine();

                Console.Write("Enter Quantity: ");
                int quantity = int.Parse(Console.ReadLine());

                Product newProduct = new Product
                {
                    ProductID = productID,
                    Name = productName,
                    Description = description,
                    Price = price,
                    Category = category
                };

                system.AddProductToInventory(newProduct, quantity);
                Console.WriteLine("Product added successfully!");
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: Invalid input format");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            GenerateMenu();
        }

        static void Process_Order()
        {
            try
            {
                if (system.Customers.Count == 0)
                {
                    Console.WriteLine("No customers available");
                    return;
                   
                }

                Console.WriteLine("=== PROCESS ORDER ===");

                // Find customer
                Customer customer = system.Customers[0];

                // Create order from input
                Order order = CreateOrderFromInput();

                if (order != null)
                {
                    system.ProcessCustomerOrder(customer, order);
                    Console.WriteLine($"Order processed: {order.GetOrderDetails()}");
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing order: {ex.Message}");
                
            }
            GenerateMenu(); 
        }

        static void Generate_Report()
        {
            try
            {
                Console.WriteLine("=== GENERATE REPORT ===");
                Console.WriteLine("1. Sales Report");
                Console.WriteLine("2. Low Stock Report");
                Console.WriteLine("3. Inventory Summary");
                Console.Write("Select report type: ");

                int reportType = int.Parse(Console.ReadLine());
                string report = "";

                switch (reportType)
                {
                    case 1:
                        Console.Write("Enter start date (yyyy-mm-dd): ");
                        DateTime startDate = DateTime.Parse(Console.ReadLine());
                        Console.Write("Enter end date (yyyy-mm-dd): ");
                        DateTime endDate = DateTime.Parse(Console.ReadLine());
                        report = reportGen.GenerateSalesReport(startDate, endDate);
                        GenerateMenu();
                        break;
                    case 2:
                        Console.Write("Enter threshold quantity: ");
                        int threshold = int.Parse(Console.ReadLine());
                        report = reportGen.GenerateLowStockReport(threshold);
                        GenerateMenu(); 
                        break;
                    case 3:
                        report = reportGen.GenerateInventorySummary();
                        GenerateMenu();
                        break;
                    default:
                        Console.WriteLine("Invalid report type");
                        GenerateMenu() ;
                        return;
                }

                Console.WriteLine(report);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating report: {ex.Message}");
                GenerateMenu();
            }
        }

        static void Login_System()
        {
            Console.WriteLine("Please Enter Username:");
            string Username = Console.ReadLine();
            Console.WriteLine("Please Enter Password:");
            string Password = Console.ReadLine();

            if (currentUser.Login(Username, Password))
            {
                Console.WriteLine("Successfully Logged in");
                OnLoginAttempt(Username, true);
            }
            else
            {
                Console.WriteLine("Login failed");
                OnLoginAttempt(Username, false);
            }

            GenerateMenu();
        }

        // Helper method to create order from user input
        static Order CreateOrderFromInput()
        {
            try
            {
                if (system.Inventory.ProductList.Count == 0)
                {
                    Console.WriteLine("No products available");
                    return null;
                }

                Order order = new Order();
                order.OrderID = system.Orders.Count + 1;

                Console.WriteLine("Available Products:");
                var productList = system.Inventory.ProductList.Keys.ToList();
                for (int i = 0; i < productList.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {productList[i].GetProductDetails()}");
                }

                Console.Write("Select product number: ");
                int productChoice = int.Parse(Console.ReadLine()) - 1;

                if (productChoice < 0 || productChoice >= productList.Count)
                {
                    throw new ProductNotFoundException("Invalid product selection");
                }

                Console.Write("Enter quantity: ");
                int quantity = int.Parse(Console.ReadLine());

                order.AddProduct(productList[productChoice], quantity);
                return order;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating order: {ex.Message}");
                return null;
            }
        }

        static void GenerateMenu()
        {
            Console.ReadLine();
            Console.Clear();
           
            Console.WriteLine("Welcome, please select an option");
            Console.WriteLine("--------------------------------");
            foreach (Menu_Page item in Enum.GetValues(typeof(Menu_Page)))
            {
                Console.WriteLine($"For {item.ToString()} enter: {(int)item} ");
            }
            int choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    {
                        Console.WriteLine("Loading...");
                        Thread.Sleep(2000);
                        Console.Clear();
                        Login_System();
                    }
                    break;
                case 2:
                    {
                        Console.WriteLine("Loading...");
                        Thread.Sleep(2000);
                        Console.Clear();
                        Add_Product();
                    }
                    break;
                case 3:
                    {
                        Console.WriteLine("Loading...");
                        Thread.Sleep(2000);
                        Console.Clear();
                        Process_Order();
                    }
                    break;
                case 4:
                    {
                        Console.WriteLine("Loading...");
                        Thread.Sleep(2000);
                        Console.Clear();
                        Generate_Report();
                    }
                    break;
                case 5:
                    {
                        Console.WriteLine("Loading...");
                        Thread.Sleep(2000);
                        Console.Clear();
                        currentUser.Logout();
                        Environment.Exit(0);
                    }
                    break;
            }
        }
        static void Main(string[] args)
        {
            GenerateMenu();
        }
    }
}