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

        //[Required(ErrorMessage = "The codebase Link is Required!")]
        public string CodebaseLink { get; set; }

        [DisplayName("Upload Codebase")]
        public string CodebasePath { get; set; }

        [NotMapped]
        public HttpPostedFileBase CodebaseFile { get; set; }

        //[Required(ErrorMessage = "The codebase Description is Required!")]
        public string Role { get; set; }

        //[Required(ErrorMessage = "The codebase Description is Required!")]
        public int Sprints { get; set; }

        //[Required(ErrorMessage = "The codebase Description is Required!")]
        public int Members { get; set; }

        //[Required(ErrorMessage = "The codebase Description is Required!")]
        public string Environment { get; set; }

        //[Required(ErrorMessage = "The codebase Description is Required!")]
        public Technology SelectedTechnology { get; set; }

        //[Required(ErrorMessage = "The codebase Description is Required!")]
        //public string SelectedProgrammingLanguage { get; set; }
        //public IEnumerable<SelectListItem> ProgrammingLanguages { get; set; }
        public Language SelectedProgrammingLanguage { get; set; }

        //[Required(ErrorMessage = "The codebase Description is Required!")]
        public string TimeLimit { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public Difficulty SelectedDifficulty{ get; set; }

        public List <string> Tags { get; set; }

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