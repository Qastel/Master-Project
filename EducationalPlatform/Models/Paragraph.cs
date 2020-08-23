using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EducationalPlatform.Models
{
    public class Paragraph
    {
        [Key]
        public int Id { get; set; }

        public int CodebaseId { get; set; }

        public string Description { get; set; }
    }
}