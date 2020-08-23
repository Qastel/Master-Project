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
        public ActionResult Index(Codebases codebase, int pagination = 1, string search = null)
        {
            var allCodebases = db.Codebases;

            ViewBag.ActivePage = pagination;

            // figure out how many pages do we need
            if (allCodebases.Count() % 6 == 0) // check if the number of codebases is a multiple of 6
            {
                ViewBag.CodebasesPages = allCodebases.Count() / 6; //  we have a complete number of pages 
            }
            else
            {
                ViewBag.CodebasesPages = allCodebases.Count() / 6 + 1; // we need one more page for the rest
            }

            ViewBag.LeftArrow = true; // set the pagination boundaries
            ViewBag.RightArrow = true;
            if (pagination == 1) // check the pagination boundaries
            {
                ViewBag.LeftArrow = false;
            }
            if (pagination == ViewBag.CodebasesPages || allCodebases.Count() == 0)
            {
                ViewBag.RightArrow = false;
            }

            var x = db.Codebases.Include("User").ToList(); // save the codebases including the user

            if (!String.IsNullOrEmpty(search)) // verify if search was run
            {
                x = x.Where(c => c.CodebaseName.Contains(search) || c.User.FullName.Contains(search)
                ).OrderBy(c => c.Id).Skip((pagination - 1) * 6).Take(6).ToList();
            }
            else {
                if (codebase.SelectedProgrammingLanguage != null) // verify if a language was selected
                {
                    x = x.Where(d => d.SelectedProgrammingLanguage == codebase.SelectedProgrammingLanguage).ToList();
                }
                if (codebase.SelectedDifficulty != null) // verify if a codebase was selected
                {  
                    x = x.Where(d => d.SelectedDifficulty == codebase.SelectedDifficulty).ToList();
                }
                if (codebase.SelectedTechnology != null) // verify if a technology was selected
                {
                    x = x.Where(d => d.SelectedTechnology == codebase.SelectedTechnology).ToList();
                }

                x = x.OrderBy(d => d.Id).Skip((pagination - 1) * 6).Take(6).ToList(); // if any of the selection has been applied take only the 6 codebases of the current paage
            }

            foreach(var c in x) // set the rating for each codebase
            {
                var CodebaseReviews = db.Reviews.Where(r => r.CodebaseId == c.Id).ToList();
                
                var total = 0;
                foreach (var r in CodebaseReviews)
                {
                    total = total + r.Rating;
                }
                var rating = 0;
                if (CodebaseReviews.Count() != 0)
                {
                    rating = total / CodebaseReviews.Count(); // for one codebase
                }

                c.MeanRating = rating;
            }

            ViewBag.Codebases = x;

            return View();
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


        public ActionResult Show(int Id)
        {
            var x = db.Codebases.Find(Id);
            return View(x);
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
        [ValidateInput(false)]
        public ActionResult New(Codebases codebase, FormCollection form)
        {
            //codebase.ProgrammingLanguages = GetAllProgrammingLanguages(); // save the obj with the list of programming languages in case we need further along to change it
            codebase.User = db.Users.FirstOrDefault(x => x.Id == codebase.UserId);
                        
            //form["topics"].ToString() 
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

        [HttpDelete]
        [Authorize(Roles = "Instructor")]
        public ActionResult Delete(int id)
        {
            Codebases c = db.Codebases.Find(id);

            if (c.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator") && c!= null)
            {
                db.Codebases.Remove(c);
                db.SaveChanges();
                TempData["message"] = "Locatia a fost stearsa!";
                return RedirectToAction("Index", "Manage");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti o locatie care nu va apartine!";
                return RedirectToAction("Index", "Manage");
            }
        }

    }
}