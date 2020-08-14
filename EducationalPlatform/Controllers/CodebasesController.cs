using EducationalPlatform.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EducationalPlatform.Controllers
{
    public class CodebasesController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();

        // GET: Codebases
        public ActionResult Index()
        {
            try
            {
                var x = db.Codebases.Include("User");
                ViewBag.Codebases = x;
            }
            catch
            {
                ViewBag.Codebases = null;
            }
            return View();
        }

        [HttpPost]
        public ActionResult ApplyFilter(Codebases codebase)
        {
            try
            {
                var x = from d in db.Codebases
                        where (d.SelectedDifficulty == codebase.SelectedDifficulty 
                        && d.SelectedTechnology == codebase.SelectedTechnology 
                        && d.SelectedProgrammingLanguage == codebase.SelectedProgrammingLanguage)
                        select d;
                ViewBag.Codebases = x;
                return View("Index");
            }
            catch (Exception e)
            {
                return View("Index");
            }
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllProgrammingLanguages()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();
            // Extragem toate categoriile din baza de date
            var categories = from cat in db.Codebases select cat;
            // iteram prin categorii
           
                // Adaugam in lista elementele necesare pentru dropdown
                selectList.Add(new SelectListItem
                {
                    Value = "Python",
                    Text = "Python"
                });
            return selectList;
        }


        public ActionResult Show()
        {
            return View();
        }


        [Authorize(Roles = "Instructor")]
        public ActionResult New()
        {
            
            Codebases codebase = new Codebases();

            //codebase.ProgrammingLanguages = GetAllProgrammingLanguages(); // get the list of programming languages for dropdown

            codebase.UserId = User.Identity.GetUserId();

            return View(codebase);
        }

        [HttpPost]
        public ActionResult New(Codebases codebase)
        {
            //codebase.ProgrammingLanguages = GetAllProgrammingLanguages(); // save the obj with the list of programming languages in case we need further along to change it
            codebase.User = db.Users.FirstOrDefault(x => x.Id == codebase.UserId);

            try
            {
                db.Codebases.Add(codebase);
                db.SaveChanges();
                TempData["message"] = "Categoria a fost adaugata!";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }
   
    }
}