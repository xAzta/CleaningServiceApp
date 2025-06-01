using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleaningServiceApp.Models
{
    public class Customer
    {
        public int CustomerID { get; set; } // Egyedi azonosító
        public string Name { get; set; } // Ügyfél neve
        public List<ClothingItem> Orders { get; set; } = new List<ClothingItem>(); // Leadott ruhák listája

        public void DropOff(ClothingItem item)
        {
            Orders.Add(item);
        }
        public void PickUp(ClothingItem item)
        {
            if (item.IsCleaned && Orders.Contains(item))
            {
                Orders.Remove(item);
            }
            else
            {
                Console.WriteLine("Ruhadarab nem tisztított vagy nincs a megrendelések között.");
            }
        }
        public override string ToString()
        {
            return $"Név: {Name},Leadott ruhák: {Orders.Count} db";
        }
    }
}
