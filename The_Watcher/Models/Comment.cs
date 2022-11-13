using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace The_Watcher.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public ApplicationUser user { get; set; }
        public string comment { get; set; }
    }
}