using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace The_Watcher.Models
{
    public class WatchJewellery
    {
        public List<Watch> Watches{ get; set; }
        public List<Jewellery> Jewelleries{ get; set; }

        public WatchJewellery()
        {
            Watches = new List<Watch>();
            Jewelleries = new List<Jewellery>();
        }
    }
}