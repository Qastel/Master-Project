﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EducationalPlatform.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        public string Description { get; set; }

        public int Rating { get; set; }

        public string UserId { get; set; }

        public int CodebaseId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Codebases Codebase { get; set; }
    }
}