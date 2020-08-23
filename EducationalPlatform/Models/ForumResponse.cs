using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EducationalPlatform.Models
{
    public class ForumResponse
    {
        [Key]
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime CreationDate { get; set; }

        public int TopicId { get; set; }

        public int FatherId { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ForumTopic Topic { get; set; }

        public virtual ForumResponse FatherResponse { get; set; }
    }
}