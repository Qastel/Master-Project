using EducationalPlatform.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EducationalPlatform.Controllers
{
    public class ReviewController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();

        // GET: Review
        public ActionResult Index(int CodebaseId)
        {
            var x = db.Reviews.Include("User").Include("Codebase").Where(c=> c.CodebaseId == CodebaseId && c.Description != null && c.Description != "").ToList();
            if (x != null) 
            {
                ViewBag.Reviews = x;
            }
            else {
                ViewBag.Reviews = new List<Review> ();
            }
            return View();
        }


        public ActionResult New(int CodebaseId)
        {
            var x = db.Reviews.Include("User").Include("Codebase").Where(c => c.CodebaseId == CodebaseId).ToList();
            ViewBag.Reviews = x;

            Review y = new Review();
            y.UserId = User.Identity.GetUserId();
            y.CodebaseId = CodebaseId;
            return View(y);
        }


        [HttpPost]
        [Authorize(Roles = "Instructor,Learner")]
        public ActionResult New(Review x)
        {
            x.User = db.Users.FirstOrDefault(u => u.Id == x.UserId);
            x.Codebase = db.Codebases.FirstOrDefault(c => c.Id == x.CodebaseId);


            if (ModelState.IsValid)
            {
                try
                {
                    db.Reviews.Add(x);
                    db.SaveChanges();
                    TempData["message"] = "The review has been added!";
                    return RedirectToAction("Show", "Codebases", new { Id = x.CodebaseId });
                }
                catch (Exception e)
                {
                    return RedirectToAction("Show", "Codebases", new { Id = x.CodebaseId });
                }
            }
           
            TempData["ErrorMessage"] = "Description is reuquired!";
            return RedirectToAction("Show", "Codebases", new { Id = x.CodebaseId });

        }

    }
}