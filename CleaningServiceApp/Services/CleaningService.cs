using CleaningServiceApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System. IO;

namespace CleaningServiceApp.Services
{
    public class CleaningService
    {
        public List<ClothingItem> ClothingItems { get; set; } = new List<ClothingItem>();
        public List<Customer> Customers { get; set; } = new List<Customer>();

        public CleaningService()
        {

        }
        public void AddItem(ClothingItem item)
        {
            ClothingItems.Add(item);
        }
        public void RemoveItem(string itemId)
        {
            var item = ClothingItems.FirstOrDefault(i => i.ItemID == itemId);
            if (item != null)
            {
                ClothingItems.Remove(item);
            }
        }
        public ClothingItem FindItem(string itemId)
        {
            return ClothingItems.FirstOrDefault(i => i.ItemID == itemId);
        }
        public void AddCustomer(Customer customer)
        {
            Customers.Add(customer);
        }
        public Customer FindCustomer(string name)
        {
            return Customers.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        public void DropOffItem(string customerName, ClothingItem item)
        {
            var customer = FindCustomer(customerName);
            if (customer != null)
            {
                customer.DropOff(item);
            }
            else
            {
                Console.WriteLine("Az ügyfél nem található.");
            }
        }
        public bool PickupItem(string customerName, string itemId)
        {
            var customer = FindCustomer(customerName);
            var item = FindItem(itemId);

            if (customer != null && item != null && item.IsCleaned && customer.Orders.Contains(item))
            {
                customer.PickUp(item);
                ClothingItems.Remove(item);
                return true;
            }
            else
            {
                Console.WriteLine("Az ügyfél vagy a ruhadarab nem található, vagy a ruhadarab nincs tisztítva.");
                return false;
            }
        }
        public List<ClothingItem> ListUncleanedItems()
        {
            return ClothingItems.Where(item => !item.IsCleaned).ToList();
        }
        public List<Customer> ListCustomers()
        {
            return Customers;
        }
        public void SaveToFile(string itemsPath, string customerPath)
        {
            using (StreamWriter writer = new StreamWriter(itemsPath))
            {
                foreach (var item in ClothingItems)
                {
                    writer.WriteLine($"{item.ItemID},{item.Type},{item.Material},{item.IsCleaned},{item.Price}");
                }
            }
            using (StreamWriter writer = new StreamWriter(customerPath))
            {
                foreach (var customer in Customers)
                {
                    string orderIDs = string.Join(",", customer.Orders.Select(o => o.ItemID));
                    writer.WriteLine($"{customer.CustomerID},{customer.Name},{orderIDs}");
                }
            }
        }
        public void LoadFromFile(string itemsPath, string customersPath)
        {
            ClothingItems.Clear();
            Customers.Clear();

            if (File.Exists(itemsPath))
            {
                foreach (var line in File.ReadAllLines(itemsPath))
                {
                    var parts = line.Split(',');
                    if (parts.Length == 5)
                    {
                        ClothingItems.Add(new ClothingItem(
                            parts[0], // ItemID
                            parts[1], // Type
                            parts[2], // Material
                            bool.Parse(parts[3]), // IsCleaned
                            decimal.Parse(parts[4]) // Price
                        ));
                    }
                }
            }
            if (File.Exists(customersPath))
            {
                foreach (var line in File.ReadAllLines(customersPath))
                {
                    var parts = line.Split(',');
                    if (parts.Length >= 2)
                    {
                        var customer = new Customer
                        {
                            CustomerID = int.Parse(parts[0]),
                            Name = parts[1]
                        };
                        if (parts.Length == 3 && !string.IsNullOrEmpty(parts[2]))
                        {
                            var orderIDs = parts[2].Split(';');
                            foreach (var id in orderIDs)
                            {
                                var item = FindItem(id);
                                if (item != null)
                                    customer.Orders.Add(item);
                            }
                        }

                        Customers.Add(customer);
                    }
                }
            }
        }
    }
}
