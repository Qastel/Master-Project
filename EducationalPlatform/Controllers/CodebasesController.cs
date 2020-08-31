using EducationalPlatform.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EducationalPlatform.Controllers
{
    public class CodebasesController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;


        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Codebases
        public ActionResult Index(Codebases codebase, int pagination = 1, string search = null)
        {
            ViewBag.Page = "Codebases";

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
            ViewBag.Page = "Codebases";
            var x = db.Codebases.Find(Id);

            // update the rating
            var CodebaseReviews = db.Reviews.Where(r => r.CodebaseId == x.Id).ToList();
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
            x.MeanRating = rating;
           

            ViewBag.DescriptionError = TempData["ErrorMessage"] as string;

            // check if learning of the current codebase has started
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user.LearnedCodebases != null)
            {
                var l = user.LearnedCodebases.Split(',');
                if (l.Contains(Convert.ToString(Id)))
                {
                    ViewBag.StartLearning = true;
                }
            }

            return View(x);
        }

        public async Task<ActionResult> StartLearning(int Id)
        {
            var x = db.Codebases.Find(Id);
            var user = UserManager.FindById(User.Identity.GetUserId());
            
            // Update the learned Codebases, it will appear in My Profile section
            if (user.LearnedCodebases != null)
            {
                var l = user.LearnedCodebases.Split(',');
                if (!l.Contains(Convert.ToString(Id)))
                {
                    user.LearnedCodebases = user.LearnedCodebases + x.Id + ",";
                    var updateResult = await UserManager.UpdateAsync(user);

                    if (updateResult.Succeeded)
                    {
                        return RedirectToAction("Show", "Codebases", new { Id = Id });
                    }
                }
            }
            else {
                user.LearnedCodebases = x.Id + ",";
                var updateResult = await UserManager.UpdateAsync(user);

                if (updateResult.Succeeded)
                {
                    return RedirectToAction("Show", "Codebases", new { Id = Id });
                }
            }

            return RedirectToAction("Show", "Codebases", new { Id = Id });
        }

        public ActionResult Edit(int Id)
        {
            Codebases c = db.Codebases.Find(Id);
            return View(c);
        }

        [HttpPut]
        public ActionResult Edit(int Id, Codebases updatedCodebase)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Codebases codebase = db.Codebases.Find(Id);

                    if (codebase.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
                    {
                        if (TryUpdateModel(codebase))
                        {
                            codebase.Members = updatedCodebase.Members;
                            codebase.Role = updatedCodebase.Role;
                            codebase.SelectedDifficulty = updatedCodebase.SelectedDifficulty;
                            codebase.SelectedProgrammingLanguage = updatedCodebase.SelectedProgrammingLanguage;
                            codebase.SelectedTechnology = updatedCodebase.SelectedTechnology;
                            codebase.Sprints =  updatedCodebase.Sprints;
                            codebase.Tags = updatedCodebase.Tags;
                            codebase.TimeLimit = updatedCodebase.TimeLimit;
                            codebase.CodebaseLink = updatedCodebase.CodebaseLink;
                            codebase.GoogleFormsLink = updatedCodebase.GoogleFormsLink;
                            codebase.GoogleFormsEmbedded = updatedCodebase.GoogleFormsEmbedded;
                            codebase.CodebaseName = updatedCodebase.CodebaseName;
                            codebase.Description = updatedCodebase.Description;
                            codebase.Environment = updatedCodebase.Environment;

                            db.SaveChanges();
                            TempData["message"] = "Locatia a fost modificata!";
                        }
                        return RedirectToAction("Index", "Manage");
                    }
                    else
                    {
                        TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui articol care nu va apartine!";
                        return View();
                    }
                }
                else
                {
                    return View();
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }


        public ActionResult Instructions()
        {
            ViewBag.Page = "Instructions";
            return View();
        }


        [Authorize(Roles = "Instructor")]
        public ActionResult New()
        {
            ViewBag.Page = "NewCodebase";
            Codebases codebase = new Codebases();
            codebase.UserId = User.Identity.GetUserId();
            return View(codebase);
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult New(Codebases codebase, FormCollection form)
        {
            codebase.User = db.Users.FirstOrDefault(x => x.Id == codebase.UserId);

            try
            {
                if (codebase.CodebaseFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(codebase.CodebaseFile.FileName);
                    string extension = Path.GetExtension(codebase.CodebaseFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yy_mm_ss_fff") + extension;
                    codebase.CodebasePath = fileName;
                    fileName = Path.Combine(Server.MapPath("~/App_Data/UploadedCodebases/"), fileName);
                    codebase.CodebaseFile.SaveAs(fileName);
                }
                if (codebase.ModelAnswersFile != null) {
                    string fileName = Path.GetFileNameWithoutExtension(codebase.ModelAnswersFile.FileName);
                    string extension = Path.GetExtension(codebase.ModelAnswersFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yy_mm_ss_fff") + extension;
                    codebase.ModelAnswersPath = fileName;
                    fileName = Path.Combine(Server.MapPath("~/App_Data/UploadedModelAnswers/"), fileName);
                    codebase.ModelAnswersFile.SaveAs(fileName);
                }

                db.Codebases.Add(codebase);
                db.SaveChanges();
                TempData["message"] = "The codebase has been added";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }

        public FileResult DownloadCodebase(string fileName)
        {
            string fullPath = Path.Combine(Server.MapPath("~/App_Data/UploadedCodebases"), fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }


        public FileResult DownloadModelAnswers(string fileName)
        {
            string fullPath = Path.Combine(Server.MapPath("~/App_Data/UploadedModelAnswers"), fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
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