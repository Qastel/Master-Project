using EducationalPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EducationalPlatform.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();

        // GET: Users
        public ActionResult Index()
        {

            var users = from user in db.Users
                            //where user.EmailConfirmed == true
                        orderby user.UserName
                        select user;

            ViewBag.UsersList = users;
            return View();
        }

        [HttpDelete]
        public ActionResult Delete(string id)
        {
            ApplicationUser user = db.Users.Find(id);

            var codebases = db.Codebases.Where(d => d.UserId == id).ToList(); // se sterg toate programarile ce apartin userului curent

            foreach (Codebases a in codebases)
            {
                db.Codebases.Remove(a);
            }

            db.Users.Remove(user);
            db.SaveChanges();


            TempData["message"] = "The user has been removed!";
            return RedirectToAction("Index", "Users");

        }
    }
}