using System;
using System.Collections.Generic;
using CleaningServiceApp.Services;
using CleaningServiceApp.Models;

namespace CleaningServiceApp
{
    class Program
    {
        static void Main()
        {
            var cleaningService = new CleaningService();
            string choice = string.Empty;
            while (choice != "0")
            {
                Console.WriteLine("\n🧺 Ruhatisztító Ügyfél- és Megrendeléskezelő Rendszer\n");
                Console.WriteLine("1. Ruhák listázása");
                Console.WriteLine("2. Elérhető (nem tisztított) ruhák listázása");
                Console.WriteLine("3. Új ruhadarab felvétele");
                Console.WriteLine("4. Ruhadarab eltávolítása");
                Console.WriteLine("5. Ügyfelek listázása");
                Console.WriteLine("6. Új ügyfél hozzáadása");
                Console.WriteLine("7. Ruhadarab leadása tisztításra");
                Console.WriteLine("8. Ruhadarab átvétele");
                Console.WriteLine("9. Adatok mentése fájlba");
                Console.WriteLine("0. Kilépés");
                Console.Write("\nVálassz egy opciót: ");
                choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Összes ruhadarab: ");
                        foreach (var item in cleaningService.ClothingItems)
                        {
                            Console.WriteLine(item);
                        }
                        break;
                    case "2":
                        Console.WriteLine("Elérhető ruhadarabok (nem tisztított): ");
                        foreach (var item in cleaningService.ClothingItems)
                        {
                            if (!item.IsCleaned)
                            {
                                Console.WriteLine(item);
                            }
                        }
                        break;
                    case "3":
                        Console.WriteLine("Add meg a ruhadarab azonosítóját: ");
                        string itemId = Console.ReadLine();
                        Console.WriteLine("Add meg a ruhadarab típusát: ");
                        string type = Console.ReadLine();
                        Console.WriteLine("Add meg a ruhadarab anyagát: ");
                        string material = Console.ReadLine();
                        Console.WriteLine("Add meg a tisztítás díját: ");
                        decimal price = decimal.Parse(Console.ReadLine());

                        var newItem = new ClothingItem(itemId, type, material, false, price);
                        cleaningService.AddItem(newItem);
                        Console.WriteLine("Ruhadarab hozzáadva.");
                        break;
                    case "4":
                        Console.WriteLine("Add meg a ruhadarab azonosítóját, amit el szeretnél távolítani: ");
                        string removeItemId = Console.ReadLine();
                        cleaningService.RemoveItem(removeItemId);
                        Console.WriteLine("Ruhadarab eltávolítva.");
                        break;
                    case "5":
                        Console.WriteLine("Ügyfelek listája: ");
                        foreach (var customer in cleaningService.Customers)
                        {
                            Console.WriteLine(customer);
                        }
                        break;
                    case "6":
                        Console.WriteLine("Add meg az ügyfél nevét: ");
                        string name = Console.ReadLine();
                        int newId = cleaningService.Customers.Count + 1;
                        var newCustomer = new Customer { CustomerID = newId, Name = name };
                        cleaningService.AddCustomer(newCustomer);
                        Console.WriteLine("Ügyfél hozzáadva.");
                        break;
                    case "7":
                        Console.WriteLine("Add meg az ügyfél nevét: ");
                        string customerName = Console.ReadLine();
                        Console.WriteLine("Add meg a ruhadarab azonosítóját: ");
                        string itemToDropId = Console.ReadLine();
                        Console.WriteLine("Ruha típusa: ");
                        string itemType = Console.ReadLine();
                        Console.WriteLine("Ruha anyaga: ");
                        string itemMaterial = Console.ReadLine();
                        Console.WriteLine("Tisztítás díja: ");
                        decimal itemPrice = decimal.Parse(Console.ReadLine());

                        var itemToDrop = new ClothingItem(itemToDropId, itemType, itemMaterial, false, itemPrice);
                        if (cleaningService.FindCustomer(customerName) != null)
                        {
                            cleaningService.DropOffItem(customerName, itemToDrop);
                            Console.WriteLine("Ruhadarab leadva tisztításra.");
                        }
                        else
                        {
                            Console.WriteLine("Ügyfél nem található.");
                        }
                        break;
                    case "8":
                        Console.WriteLine("Add meg az ügyfél nevét: ");
                        string pickupCustomerName = Console.ReadLine();
                        Console.WriteLine("Add meg a ruhadarab azonosítóját: ");
                        string pickupItemId = Console.ReadLine();

                        var itemToClean = cleaningService.FindItem(pickupItemId);
                        if (itemToClean != null)
                        {
                            itemToClean.IsCleaned = true;
                        }

                        if (cleaningService.PickupItem(pickupCustomerName, pickupItemId))
                        {
                            Console.WriteLine("Ruhadarab átvéve.");
                        }
                        break;
                    case "9":
                        cleaningService.SaveToFile("clothingItems.csv", "customers.csv");
                        Console.WriteLine("Adatok sikeresen mentve.");
                        break;
                    case "0":
                        Console.WriteLine("Kilépés a programból...");
                        break;
                    default:
                        Console.WriteLine("Érvénytelen választás, próbáld újra!");
                        break;
                }
            }
        }
    }



 

  
}
