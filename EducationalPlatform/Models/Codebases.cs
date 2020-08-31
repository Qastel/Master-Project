using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EducationalPlatform.Models
{
    public class Codebases
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The codebase Name is Required!")]
        public string CodebaseName { get; set; }

        [Required(ErrorMessage = "The codebase Description is Required!")]
        public string Description { get; set; }

        public string CodebaseLink { get; set; }

        [DisplayName("Upload Codebase")]
        public string CodebasePath { get; set; }

        [NotMapped]
        public HttpPostedFileBase CodebaseFile { get; set; }

        [DisplayName("Upload Codebase")]
        public string ModelAnswersPath { get; set; }

        [NotMapped]
        public HttpPostedFileBase ModelAnswersFile { get; set; }

        public string GoogleFormsLink { get; set; }

        [AllowHtml]
        public string GoogleFormsEmbedded { get; set;}

        [Required(ErrorMessage = "The Learner's Role is Required!")]
        public string Role { get; set; }

        [Required(ErrorMessage = "The codebase Number of Sprints is Required!")]
        public int Sprints { get; set; }

        [Required(ErrorMessage = "The codebase's Contributors number is Required!")]
        public int Members { get; set; }

       [Required(ErrorMessage = "The codebase Environment is Required!")]
        public string Environment { get; set; }

        [Required(ErrorMessage = "The codebase Technology is Required!")]
        public Technology? SelectedTechnology { get; set; }

        [Required(ErrorMessage = "The codebase Programming Language is Required!")]
        public Language? SelectedProgrammingLanguage { get; set; }

        [Required(ErrorMessage = "The codebase Difficulty is Required!")]
        public Difficulty? SelectedDifficulty { get; set; }

        [Required(ErrorMessage = "The codebase Time Limit is Required!")]
        public int TimeLimit { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string Tags { get; set; }
        
        public int MeanRating { get; set; }
    }

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    public enum Technology
    {
        Desktop,
        Web,
        Android,
        IOS
    }

    public enum Language
    {
        Python, CSS, C, Java, JavaScript, NodeJs, Go, Swift
    }
}