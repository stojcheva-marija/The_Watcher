using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace The_Watcher.Models
{
    public class ShoppingCart
    {
            public int Id { get; set; }
            public String Username { get; set; }
            public String Address { get; set; }
            public String Name { get; set; }
            public String Surname { get; set; }
            public String Phone { get; set; }
            public virtual List<Jewellery> ListJewelleries { get; set; }
            public virtual List<Watch> ListWatches { get; set; }
            public ShoppingCart()
            {
            ListJewelleries = new List<Jewellery>();
            ListWatches = new List<Watch>();
            }
        
    }
}