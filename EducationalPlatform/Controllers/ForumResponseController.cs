﻿using EducationalPlatform.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EducationalPlatform.Controllers
{
    public class ForumResponseController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();

        // GET: Response
        public ActionResult Index(int TopicId)
        {
            var x = db.ForumResponses.Include("User").Include("Topic").Where(c => c.TopicId == TopicId);
            ViewBag.ForumResponses = x;


            ForumResponse y = new ForumResponse();
            y.UserId = User.Identity.GetUserId();
            y.TopicId = TopicId;
            y.Topic = db.ForumTopics.FirstOrDefault(c => c.Id == TopicId);
            return View(y);
        }

        // GET: Response
        public ActionResult New(int TopicId, int FatherId = -1)
        {
            var x = db.ForumResponses.Include("User").Include("Topic").Where(c => c.TopicId == TopicId);
            ViewBag.ForumResponses = x;


            ForumResponse y = new ForumResponse();
            y.UserId = User.Identity.GetUserId();
            y.TopicId = TopicId;
            y.Topic = db.ForumTopics.FirstOrDefault(c => c.Id == TopicId);
            y.FatherId = FatherId;
          
            return View(y);
        }

        [HttpPost]
        public ActionResult New(ForumResponse x)
        {
            x.User = db.Users.FirstOrDefault(u => u.Id == x.UserId);
            x.Topic = db.ForumTopics.FirstOrDefault(c => c.Id == x.TopicId);
            x.CreationDate = DateTime.Now.Date;

            if (x.FatherId != -1)
            {
                x.FatherResponse = db.ForumResponses.FirstOrDefault(c => c.Id == x.FatherId);
            }
            try
            {
                db.ForumResponses.Add(x);
                db.SaveChanges();
                TempData["message"] = "Categoria a fost adaugata!";
                return RedirectToAction("Index", "ForumResponse", new { x.TopicId });
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "ForumResponse", new { x.TopicId });
            }
        }

    }
}