using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEduSolutions.Databases.SEDB
{
    public class Questions
    {
        [Key]
        public int IdQuestions { get; set; }
        [Required]
        public string QuestionPart { get; set; }
        [Required]
        public int Marks { get; set; }
        public string Attachment { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }
        [Required]
        public int Classrooms_IdClassrooms { get; set; }
        [Required]
        public int Assignments_IdAssignments { get; set; }
    }
}
