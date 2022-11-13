using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace The_Watcher.Models
{
    public class Jewellery
    {   public int Id { get; set; }
        [Display(Name = "Бренд")]
        public string Brand { get; set; }
        [Display(Name ="Пол")]
        public string Gender { get; set; }
        [Display(Name = "Шифра")]
        public string ProductCode { get; set; }
        [Display(Name = "Цена")]
        public int Price { get; set; }
        [Display(Name = "Попуст")]
        public int Discount { get; set; }
        [Display(Name = "Изглед")]
        public string ImageURL { get; set; }
        [Display(Name = "Категорија")]
        public string Category { get; set; }
        [Display(Name = "Боја на накит")]
        public string Color { get; set; }
        [Display(Name = "Должина на накит (mm)")]
        public int Length { get; set; }
        [Display(Name = "Материјал на накит")]
        public string Material { get; set; }
        [Display(Name = "Достапност")]
        public string Avaliability { get; set; }
        public double UserGrade{ get; set; }
        public int Graders { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public List<ShoppingCart> ShoppingCarts { get; set; }
        public List<WishList> WishLists { get; set; }
    }
}