using EducationalPlatform.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EducationalPlatform.Controllers
{
    public class ForumTopicController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();

        // GET: ForumTopic
        public ActionResult Index(int CodebaseId)
        {
            var x = db.ForumTopics.Include("User").Include("Codebase").Where(c => c.CodebaseId == CodebaseId);
            ViewBag.ForumTopics = x;
            ViewBag.CurrentUser = User.Identity.GetUserId();

            ForumTopic y = new ForumTopic();
            y.UserId = User.Identity.GetUserId();
            y.CodebaseId = CodebaseId;
            y.Codebase = db.Codebases.FirstOrDefault(c => c.Id == CodebaseId);
            return View(y);
        }

        /*
        public ActionResult New(int CodebaseId)
        {
            var x = db.ForumTopics.Include("User").Include("Codebase").Where(c => c.CodebaseId == CodebaseId);
            ViewBag.ForumTopics = x;

            return RedirectToAction("Index", "ForumTopic", new { x.Co});
        } 
        */

        [HttpPost]
        [Authorize(Roles = "Instructor,Learner")]
        public ActionResult New(ForumTopic x)
        {
            x.User = db.Users.FirstOrDefault(u => u.Id == x.UserId);
            x.Codebase = db.Codebases.FirstOrDefault(c => c.Id == x.CodebaseId);
            x.CreationDate = DateTime.Now;
            x.UserId = User.Identity.GetUserId();
 
            try
            {
                db.ForumTopics.Add(x);
                db.SaveChanges();
                TempData["message"] = "Categoria a fost adaugata!";
                return RedirectToAction("Index", "ForumTopic", new { x.CodebaseId });
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "ForumTopic", new { x.CodebaseId });
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Instructor, Learner")]
        public ActionResult Delete(int id, int CodebaseId)
        {
            ForumTopic t = db.ForumTopics.Find(id);

            if (t != null && (t.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator")) )
            {
                db.ForumTopics.Remove(t);
                db.SaveChanges();
                return RedirectToAction("Index", "ForumTopic", new { CodebaseId});
            }
            else
            {
                return RedirectToAction("Index", "ForumTopic", new { CodebaseId });
            }
        }

    }
}