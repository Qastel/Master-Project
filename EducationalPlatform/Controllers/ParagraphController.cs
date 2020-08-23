using EducationalPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EducationalPlatform.Controllers
{
    public class ParagraphController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();

        // GET: Paragraph
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult New()
        {
            Paragraph p = new Paragraph();
            return View(p);
        }

        [HttpPost]
        public ActionResult New(Paragraph p)
        {
            try
            {
                db.Paragraphs.Add(p);
                db.SaveChanges();
                TempData["message"] = "";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }

    }
}