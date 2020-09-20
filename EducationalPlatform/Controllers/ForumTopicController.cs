using EducationalPlatform.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EducationalPlatform.Controllers
{
    public class ForumTopicController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();

        // GET: ForumTopic
        public ActionResult Index(int CodebaseId)
        {
            var x = db.ForumTopics.Include("User").Include("Codebase").Where(c => c.CodebaseId == CodebaseId);
            var responses = db.ForumResponses.Include("User").Include("Topic").GroupBy(id => id.TopicId).Select(g => new {TopicId = g.Key, Count = g.Count()}).ToList();
            Dictionary<int, int> d = new Dictionary<int, int>();
            if (responses != null)
            {
                foreach (var r in responses)
                {
                    d.Add(r.TopicId, r.Count);
                }
            }

            ViewBag.ForumTopics = x;
            ViewBag.CurrentUser = User.Identity.GetUserId();
            ViewBag.ResponsesNumber = d;

            ForumTopic y = new ForumTopic();
            y.UserId = User.Identity.GetUserId();
            y.CodebaseId = CodebaseId;
            y.Codebase = db.Codebases.FirstOrDefault(c => c.Id == CodebaseId);
            return View(y);
        }


        [HttpPost]
        [Authorize(Roles = "Instructor,Learner")]
        public ActionResult New(ForumTopic x)
        {
            x.User = db.Users.FirstOrDefault(u => u.Id == x.UserId);
            x.Codebase = db.Codebases.FirstOrDefault(c => c.Id == x.CodebaseId);
            x.CreationDate = DateTime.Now;
            x.UserId = User.Identity.GetUserId();

            if (ModelState.IsValid && x.Description != null && x.Description != "") // check if all required fields are filled and if a description exists
            {
                try
                {
                    db.ForumTopics.Add(x);
                    db.SaveChanges();
                    TempData["message"] = "The topic has been added!";
                    return RedirectToAction("Index", "ForumTopic", new { x.CodebaseId });
                }
                catch (Exception e)
                {
                    return RedirectToAction("Index", "ForumTopic", new { x.CodebaseId });
                }
            }
            TempData["ErrorMessage"] = "A topic description is required!";
            return RedirectToAction("Index", "ForumTopic", new { x.CodebaseId });
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