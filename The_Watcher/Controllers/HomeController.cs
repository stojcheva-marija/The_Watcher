
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using The_Watcher.Models;

namespace The_Watcher.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ShoppingCart()
        {
            ShoppingCart cart = (ShoppingCart)Session["cart"];

            if (cart == null)
            {
                cart = new ShoppingCart();
                Session["cart"] = cart;
            }

            WatchJewellery product = new WatchJewellery();
            product.Jewelleries = cart.ListJewelleries.ToList();
            product.Watches = cart.ListWatches.ToList();

            return View(product);

        }

        public ActionResult DeleteCart(int id, string type)
        {
            ShoppingCart cart = (ShoppingCart)Session["cart"];
            if (cart == null)
            {
                cart = new ShoppingCart();
                Session["cart"] = cart;
            }
            if (type == "Jewellery")
            {
                foreach (Jewellery j in cart.ListJewelleries.ToList())
                {
                    if (j.Id.Equals(id))
                    {
                        cart.ListJewelleries.Remove(j);
                        Session["cart"] = cart;
                    }
                }
            }
            else if (type == "Watch")
            {
                foreach (Watch w in cart.ListWatches.ToList())
                {
                    if (w.Id.Equals(id))
                    {
                        cart.ListWatches.Remove(w);
                        Session["cart"] = cart;
                    }
                }
            }
            WatchJewellery product = new WatchJewellery();
            product.Jewelleries = cart.ListJewelleries;
            product.Watches = cart.ListWatches;

            return View("ShoppingCart", product);
        }

        [Authorize]
        public ActionResult Buy()
        {
            string username = User.Identity.Name;
            ApplicationUser user = db.Users.Single(x => x.UserName.Equals(username));

            ShoppingCart cart = (ShoppingCart)Session["cart"];
            if (cart == null)
            {
                cart = new ShoppingCart();
                Session["cart"] = cart;
            }

            ShoppingCart databaseCart = new ShoppingCart();
            databaseCart.Address = user.Address;
            databaseCart.Username = user.UserName;
            databaseCart.Name = user.Name;
            databaseCart.Surname = user.Surname;
            databaseCart.Phone = user.Phone;
            foreach (Jewellery j in cart.ListJewelleries)
            {
                databaseCart.ListJewelleries.Add(db.Jewelleries.Single(x => x.Id.Equals(j.Id)));
            }
            foreach (Watch w in cart.ListWatches)
            {
                databaseCart.ListWatches.Add(db.Watches.Single(x => x.Id.Equals(w.Id)));
            }
            db.ShoppingCarts.Add(databaseCart);
            db.SaveChanges();
            Session["cart"] = null;
            return View();
        }

       [Authorize(Roles = "Administrator,Moderator")]
        public ActionResult BoughtProducts()
        {
            List<ShoppingCart> carts = db.ShoppingCarts.Include(x => x.ListJewelleries).Include(x=>x.ListWatches).ToList();
            return View(carts);
        }

        [Authorize(Roles = "Administrator,Moderator")]
        public ActionResult BoughtProductsDetailed(int id)
        {
            ShoppingCart cart = db.ShoppingCarts.Include(x => x.ListWatches).Include(x => x.ListJewelleries).Single(x => x.Id == id);
            return View(cart);

        }

        public ActionResult WishList()
        {
            WishList wishList = (WishList)Session["WishList"];

            if (wishList== null)
            {
                wishList = new WishList();
                Session["WishList"] = wishList;
            }

            WatchJewellery product = new WatchJewellery();
            product.Jewelleries = wishList.ListJewelleries.ToList();
            product.Watches = wishList.ListWatches.ToList();

            return View(product);

        }
        public ActionResult DeleteWishList(int id, string type)
        {
            WishList wishList = (WishList)Session["WishList"];

            if (wishList == null)
            {
                wishList = new WishList();
                Session["WishList"] = wishList;
            }
            if (type == "Jewellery")
            foreach (Jewellery j in wishList.ListJewelleries.ToList())
            {
                if (j.Id.Equals(id))
                {
                    wishList.ListJewelleries.Remove(j);
                    Session["WishList"] = wishList;
                }
            }
            if (type == "Watch")
            foreach (Watch w in wishList.ListWatches.ToList())
            {
                if (w.Id.Equals(id))
                {
                    wishList.ListWatches.Remove(w);
                    Session["WishList"] = wishList;
                }
            }

            WatchJewellery product = new WatchJewellery();
            product.Jewelleries = wishList.ListJewelleries;
            product.Watches = wishList.ListWatches;

            return View("WishList", product);
        }

    }
}