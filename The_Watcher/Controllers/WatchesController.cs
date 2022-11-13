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
    public class WatchesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Watches
        public ActionResult Index()
        {
            return View(db.Watches.ToList());
        }

        // GET: Watches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Watch watch = db.Watches.Find(id);
            if (watch == null)
            {
                return HttpNotFound();
            }
            return View(watch);
        }

        // GET: Watches/Create
        [Authorize(Roles = "Administrator,Moderator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Watches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Moderator")]
        public ActionResult Create([Bind(Include = "Id,Gender,Brand,ProductCode,Price,Discount,ImageURL,Category,Color,WatchWidth,WatchMaterial,StrapMaterial,WatchShape,Avaliability")] Watch watch)
        {
            if (ModelState.IsValid)
            {
                db.Watches.Add(watch);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(watch);
        }

        // GET: Watches/Edit/5
        [Authorize(Roles = "Administrator,Moderator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Watch watch = db.Watches.Find(id);
            if (watch == null)
            {
                return HttpNotFound();
            }
            return View(watch);
        }

        // POST: Watches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Moderator")]
        public ActionResult Edit([Bind(Include = "Id,Gender,Brand,ProductCode,Price,Discount,ImageURL,Category,Color,WatchWidth,WatchMaterial,StrapMaterial,WatchShape,Avaliability")] Watch watch)
        {
            if (ModelState.IsValid)
            {
                db.Entry(watch).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(watch);
        }

        // GET: Watches/Delete/5
        [Authorize(Roles = "Administrator,Moderator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Watch watch = db.Watches.Find(id);
            if (watch == null)
            {
                return HttpNotFound();
            }
            return View(watch);
        }

        // POST: Watches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Moderator")]
        public ActionResult DeleteConfirmed(int id)
        {
            Watch watch = db.Watches.Find(id);
            db.Watches.Remove(watch);
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
        Watch watch = db.Watches.Find(id);
        watch.UserGrade = (watch.UserGrade * watch.Graders + grade) / (watch.Graders+1);
            watch.Graders++;
            db.SaveChanges();
        return RedirectToAction("Details", new { id = id });
    }
        [Authorize]
    public ActionResult Comment(int id, string comment)
    {
        string username = User.Identity.Name;
        ApplicationUser user = db.Users.Single(x => x.UserName.Equals(username));
        Watch watch = db.Watches.Find(id);
        if (watch.Comments == null)
            watch.Comments = new List<Comment>();
        watch.Comments.Add(new Comment() { comment = comment, user = user });
        db.SaveChanges();
        return RedirectToAction("Details", new { id = id });
    }
    [HttpGet]
    public async Task<ActionResult> Index(string sortOrder, string SearchString, int? page, string gender, string strapMaterial)
    {
            ViewData["SearchParameter"] = SearchString;
            if (sortOrder != null)
                ViewData["Filtering"] = sortOrder;
            else
                ViewData["Filtering"] = "Најпопуларни";

            List<Watch> watches = new List<Watch>();
            if (strapMaterial == null && ViewData["strapMaterial"] != null)
            {
                gender = ViewData["gender"].ToString();
                strapMaterial = ViewData["strap"].ToString();
            }
            if (gender == null && strapMaterial == null)
            {
                watches = (from w in db.Watches
                               select w).ToList();
            }
            else
            {
                if (strapMaterial == "Сите")
                {
                    watches = db.Watches.Where(w => w.Gender.Equals(gender)).ToList();

                }
                else
                {
                    watches = db.Watches.Where(w => w.Gender.Equals(gender)).Where(j => j.StrapMaterial.Equals(strapMaterial)).ToList();
                }
            }


        if (!String.IsNullOrEmpty(SearchString))
        {
            watches = watches.Where(w => w.ProductCode.Contains(SearchString.ToUpper()) || w.Brand.Contains(SearchString.ToUpper())).ToList();
        }
        switch (sortOrder)
        {
            case "Цена ( растечки редослед )":
                watches = watches.OrderBy(w => ((100 - w.Discount) * w.Price) / 100).ToList();
                break;
            case "Цена ( опаѓачки редослед )":
                watches = watches.OrderByDescending(j => ((100 - j.Discount) * j.Price) / 100).ToList();
                break;
            case "Име":
                watches = watches.OrderBy(j => j.ProductCode).ToList();
                break;
            case "Попуст ( растечки редослед )":
                watches = watches.OrderBy(j => j.Discount).ToList();
                break;
            case "Попуст ( опаѓачки редослед )":
                watches = watches.OrderByDescending(j => j.Discount).ToList();
                break;
            case "Бренд ( A - Z )":
                watches = watches.OrderBy(j => j.Brand).ToList();
                break;
            case "Бренд ( Z - A )":
                watches = watches.OrderByDescending(j => j.Brand).ToList();
                break;
            default:
               watches = watches.OrderByDescending(j => j.UserGrade).ToList();
                break;

        }

        int pageSize = 9;
        int pageNumber = (page ?? 1);

            ViewData["gender"] = gender;
            ViewData["strap"] = strapMaterial;


            return View(watches.ToPagedList(pageNumber, pageSize));
    }

    public ActionResult AddToCart(string id)
    {
        ShoppingCart cart = (ShoppingCart)Session["cart"];
        if (cart == null)
        {
            cart = new ShoppingCart();
            Session["cart"] = cart;
        }

        foreach (Watch w in db.Watches.ToList())
        {
            if (w.Id.Equals(int.Parse(id)))
            {
                cart.ListWatches.Add(w);
                Session["cart"] = cart;
                return View("Index", db.Watches.OrderBy(m => m.UserGrade).ToPagedList(1, 9));
            }
        }

        return HttpNotFound();
    }

    public ActionResult AddToWishList(string id)
    {
        WishList wishList = (WishList)Session["WishList"];
        if (wishList == null)
        {
            wishList = new WishList();
            Session["WishList"] = wishList;
        }

        foreach (Watch w in db.Watches.ToList())
        {
            if (w.Id.Equals(int.Parse(id)))
            {
                wishList.ListWatches.Add(w);
                Session["WishList"] = wishList;
                return View("Index", db.Watches.OrderBy(m => m.UserGrade).ToPagedList(1, 9));
            }
        }

        return HttpNotFound();
        }
    }
    }

