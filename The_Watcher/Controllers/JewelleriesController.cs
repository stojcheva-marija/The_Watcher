using PagedList;
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
    public class JewelleriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Jewelleries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jewellery jewellery = db.Jewelleries.Find(id);
            if (jewellery == null)
            {
                return HttpNotFound();
            }
            return View(jewellery);
        }

        // GET: Jewelleries/Create
        [Authorize(Roles = "Administrator,Moderator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Jewelleries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Moderator")]
        public ActionResult Create([Bind(Include = "Id,Brand,Gender,ProductCode,Price,Discount,ImageURL,Category,Color,Length,Material,Avaliability")] Jewellery jewellery)
        {
            if (ModelState.IsValid)
            {
                db.Jewelleries.Add(jewellery);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jewellery);
        }

        // GET: Jewelleries/Edit/5
        [Authorize(Roles = "Administrator,Moderator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jewellery jewellery = db.Jewelleries.Find(id);
            if (jewellery == null)
            {
                return HttpNotFound();
            }
            return View(jewellery);
        }

        // POST: Jewelleries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Moderator")]
        public ActionResult Edit([Bind(Include = "Id,Brand,Gender,ProductCode,Price,Discount,ImageURL,Category,Color,Length,Material,Avaliability")] Jewellery jewellery)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jewellery).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jewellery);
        }

        // GET: Jewelleries/Delete/5
        [Authorize(Roles = "Administrator,Moderator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jewellery jewellery = db.Jewelleries.Find(id);
            if (jewellery == null)
            {
                return HttpNotFound();
            }
            return View(jewellery);
        }

        // POST: Jewelleries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Moderator")]
        public ActionResult DeleteConfirmed(int id)
        {
            Jewellery jewellery = db.Jewelleries.Find(id);
            db.Jewelleries.Remove(jewellery);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize]
        public ActionResult Grade(int id, int grade)
        {
            Jewellery jewellery = db.Jewelleries.Find(id);
            jewellery.UserGrade = (jewellery.UserGrade * jewellery.Graders + grade) / (jewellery.Graders+1);
            jewellery.Graders++;
            db.SaveChanges();
            return RedirectToAction("Details", new { id = id });
        }
        [Authorize]
        public ActionResult Comment(int id, string comment)
        {
            string username = User.Identity.Name;
            ApplicationUser user = db.Users.Single(x => x.UserName.Equals(username));
            Jewellery jewellery = db.Jewelleries.Find(id);
            if (jewellery.Comments == null)
                jewellery.Comments = new List<Comment>();
            jewellery.Comments.Add(new Comment() { comment = comment, user = user });
            db.SaveChanges();
            return RedirectToAction("Details", new { id = id });
        }

        [HttpGet]
        public ActionResult Index(string sortOrder, string SearchString, int? page, string gender, string category)
        {
            ViewData["SearchParameter"] = SearchString;
            if (sortOrder != null)
                ViewData["Filtering"] = sortOrder;
            else
                ViewData["Filtering"] = "Најпопуларни";


            List<Jewellery> jewelleries = new List<Jewellery>();
            if (category == null && ViewData["category"]!=null)
            {
                gender = ViewData["gender"].ToString();
                category = ViewData["category"].ToString();
            }
            if (gender == null && category == null)
            {
                    jewelleries = (from j in db.Jewelleries
                                               select j).ToList();
            }
            else
            {
                if (category == "Сите")
                {
                    jewelleries = db.Jewelleries.Where(j => j.Gender.Equals(gender)).ToList();
       
                }
                else
                {
                    jewelleries = db.Jewelleries.Where(j => j.Gender.Equals(gender)).Where(j => j.Category.Equals(category)).ToList();
                }
            }

            if (!String.IsNullOrEmpty(SearchString))
            {
                jewelleries = jewelleries.Where(j => j.ProductCode.Contains(SearchString.ToUpper()) || j.Brand.Contains(SearchString.ToUpper())).ToList();
            }
            switch (sortOrder)
            {
                case "Цена ( растечки редослед )":
                    jewelleries = jewelleries.OrderBy(j => ((100 - j.Discount) * j.Price) / 100).ToList();
                    break;
                case "Цена ( опаѓачки редослед )":
                    jewelleries = jewelleries.OrderByDescending(j => ((100 - j.Discount) * j.Price) / 100).ToList();
                    break;
                case "Име":
                    jewelleries = jewelleries.OrderBy(j => j.ProductCode).ToList();
                    break;
                case "Попуст ( растечки редослед )":
                    jewelleries = jewelleries.OrderBy(j => j.Discount).ToList();
                    break;
                case "Попуст ( опаѓачки редослед )":
                    jewelleries = jewelleries.OrderByDescending(j => j.Discount).ToList();
                    break;
                case "Бренд ( A - Z )":
                    jewelleries = jewelleries.OrderBy(j => j.Brand).ToList();
                    break;
                case "Бренд ( Z - A )":
                    jewelleries = jewelleries.OrderByDescending(j => j.Brand).ToList();
                    break;
                default:
                    jewelleries = jewelleries.OrderByDescending(j => j.UserGrade).ToList();
                    break;

            }

            int pageSize = 9;
            int pageNumber = (page ?? 1);

            ViewData["gender"] = gender;
            ViewData["category"] = category;
            
            return View(jewelleries.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult AddToCart(string id)
        {
            ShoppingCart cart = (ShoppingCart)Session["cart"];
            if (cart == null)
            {
                cart = new ShoppingCart();
                Session["cart"] = cart;
            }

            foreach (Jewellery j in db.Jewelleries.ToList())
            {
                if (j.Id.Equals(int.Parse(id)))
                {
                    cart.ListJewelleries.Add(j);
                    Session["cart"] = cart;
                   // return View("Index", db.Jewelleries.ToList());
                    return View("Index", db.Jewelleries.OrderBy(m => m.UserGrade).ToPagedList(1, 9));
                }
            }

            return HttpNotFound();
        }

        public ActionResult AddToWishList(string id)
        {
            WishList wishList = (WishList)Session["WishList"];
            if (wishList == null)
            {
                wishList= new WishList();
                Session["WishList"] = wishList;
            }

            foreach (Jewellery j in db.Jewelleries.ToList())
            {
                if (j.Id.Equals(int.Parse(id)))
                {
                    wishList.ListJewelleries.Add(j);
                    Session["WishList"] = wishList;
                    return View("Index", db.Jewelleries.OrderBy(m => m.UserGrade).ToPagedList(1, 9));
                }
            }

            return HttpNotFound();
        }
    }
}

    

