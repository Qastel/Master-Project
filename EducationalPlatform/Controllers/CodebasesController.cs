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
        private ApplicationDbContext db = ApplicationDbContext.Create(); // Create database
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

        // useful for modifying user details
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

        // GET: All Codebases
        public ActionResult Index(Codebases codebase, int pagination = 1, string search = null)
        {
            ViewBag.Page = "Codebases";

            var allCodebases = db.Codebases; // get all codebases

            ViewBag.ActivePage = pagination; // chech the active page

            // figure out how many pages do we need for the number of codebases (we accept maximum 6 codebases per page)
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

            if (!String.IsNullOrEmpty(search)) // verify if search was pressed
            {
                x = x.Where(c => c.CodebaseName.ToLower().Contains(search.ToLower()) || c.User.FullName.ToLower().Contains(search.ToLower())
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

            foreach (var c in x) // set the rating for each codebase
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

        // Show the details of the selected codebase
        public ActionResult Show(int Id)
        {
            ViewBag.Page = "Codebases";  // for the navbar, to know which one will be highlighted
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

            /*
            if (user.LearnedCodebases != null)
            {
                var l = user.LearnedCodebases.Split(',');
                if (l.Contains(Convert.ToString(Id)))
                {
                    ViewBag.StartLearning = true;
                }
            }
            */
            return View(x);
        }

        // save the Codebase to the LearnedCodebases variable
        public async Task<ActionResult> StartLearning(int Id)
        {
            var x = db.Codebases.Find(Id);
            var user = UserManager.FindById(User.Identity.GetUserId());

            if (user != null) // check if the user is logged in
            {
                // Update the learned Codebases, it will appear in My Profile section
                if (user.LearnedCodebases != null) // if is not the first learned codebase
                {
                    var l = user.LearnedCodebases.Split(',');
                    if (!l.Contains(Convert.ToString(Id))) // check if the codebase was already added to the variable
                    {
                        user.LearnedCodebases = user.LearnedCodebases + x.Id + ",";
                        var updateResult = await UserManager.UpdateAsync(user);

                        if (updateResult.Succeeded)
                        {
                            return RedirectToAction("Index", "Manage");
                        }
                    }
                }
                else
                {
                    user.LearnedCodebases = x.Id + ",";
                    var updateResult = await UserManager.UpdateAsync(user);
                    if (updateResult.Succeeded)
                    {
                        return RedirectToAction("Index", "Manage");
                    }
                }
            }

            return RedirectToAction("Show", "Codebases", new { Id = Id });
        }

        // show the edit codebase details
        public ActionResult Edit(int Id)
        {
            Codebases c = db.Codebases.Find(Id);
            return View(c);
        }

        // edit the codebase details
        [HttpPut]
        [Authorize(Roles = "Instructor")]
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
                            if (updatedCodebase.CodebaseFile != null) // edit the codebase upload
                            {
                                var path = Server.MapPath("~/App_Data/UploadedCodebases");

                                if (!System.IO.Directory.Exists(path)) // check if the directory exists
                                {
                                    System.IO.Directory.CreateDirectory(path); // create directory
                                }

                                string fileName = Path.GetFileNameWithoutExtension(updatedCodebase.CodebaseFile.FileName); // get the file
                                string extension = Path.GetExtension(updatedCodebase.CodebaseFile.FileName);
                                fileName = fileName + DateTime.Now.ToString("yy_mm_ss_fff") + extension; // set unique name
                                codebase.CodebasePath = fileName; // set the path
                                fileName = Path.Combine(Server.MapPath("~/App_Data/UploadedCodebases/"), fileName);
                                codebase.CodebaseFile.SaveAs(fileName); // save the file into the path mentioned
                            }

                            if (updatedCodebase.ModelAnswersFile != null) // edit the uploaded model answers
                            {
                                var path = Server.MapPath("~/App_Data/UploadedModelAnswers");

                                if (!System.IO.Directory.Exists(path))
                                {
                                    System.IO.Directory.CreateDirectory(path);
                                }

                                string fileName = Path.GetFileNameWithoutExtension(updatedCodebase.ModelAnswersFile.FileName);
                                string extension = Path.GetExtension(updatedCodebase.ModelAnswersFile.FileName);
                                fileName = fileName + DateTime.Now.ToString("yy_mm_ss_fff") + extension;
                                codebase.ModelAnswersPath = fileName;
                                fileName = Path.Combine(Server.MapPath("~/App_Data/UploadedModelAnswers/"), fileName);
                                codebase.ModelAnswersFile.SaveAs(fileName);
                            }
                            // update each changed variable 
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
                            TempData["message"] = "The Codebase was modified";
                        }
                        return RedirectToAction("Index", "Manage");
                    }
                    else
                    {
                        TempData["message"] = "You do not have the permission to modify it!";
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

        // show the instructor training page
        public ActionResult Instructions()
        {
            ViewBag.Page = "Instructions";
            return View("Instructions");
        }

        // show new codebase details
        [Authorize(Roles = "Instructor")]
        public ActionResult New()
        {
            ViewBag.Page = "NewCodebase";  // save the name of the page for navbar highlighting
            Codebases codebase = new Codebases();
            codebase.UserId = User.Identity.GetUserId();
            return View(codebase);
        }

        // save new codebase 
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult New(Codebases codebase, FormCollection form)
        {
            codebase.User = db.Users.FirstOrDefault(x => x.Id == codebase.UserId); // save the User of the codebase

            try
            {
                if (codebase.CodebaseFile != null) // upload the codebase file
                {
                    var path = Server.MapPath("~/App_Data/UploadedCodebases");

                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }

                    string fileName = Path.GetFileNameWithoutExtension(codebase.CodebaseFile.FileName);
                    string extension = Path.GetExtension(codebase.CodebaseFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yy_mm_ss_fff") + extension;
                    codebase.CodebasePath = fileName;
                    fileName = Path.Combine(Server.MapPath("~/App_Data/UploadedCodebases/"), fileName);
                    codebase.CodebaseFile.SaveAs(fileName);
                }
                if (codebase.ModelAnswersFile != null) { // upload the model answer
                    var path = Server.MapPath("~/App_Data/UploadedModelAnswers");

                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }

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

        // download the codebase on click 
        public FileResult DownloadCodebase(string fileName)
        {
            string fullPath = Path.Combine(Server.MapPath("~/App_Data/UploadedCodebases"), fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        // download the model answers on click
        public FileResult DownloadModelAnswers(string fileName)
        {
            string fullPath = Path.Combine(Server.MapPath("~/App_Data/UploadedModelAnswers"), fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        // delete the your uploaded codebases
        [HttpDelete]
        [Authorize(Roles = "Instructor")]
        public ActionResult Delete(int id)
        {
            Codebases c = db.Codebases.Find(id);

            if (c.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator") && c!= null)
            {
                db.Codebases.Remove(c);
                db.SaveChanges();
                TempData["message"] = "The codebase has been deleted";
                return RedirectToAction("Index", "Manage");
            }
            else
            {
                TempData["message"] = "You do not have the right to delete the codebase!";
                return RedirectToAction("Index", "Manage");
            }
        }

    }
}