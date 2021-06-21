using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEduSolutions.Databases.Dto
{
    public class QuestionDto
    {
        public int IdQuestions { get; set; }
        [Required]
        public string QuestionPart { get; set; }
        [Required]
        public int Marks { get; set; }
        [Required]
        public int IdClassrooms { get; set; }
        [Required]
        public int IdAssignments { get; set; }
    }
}
