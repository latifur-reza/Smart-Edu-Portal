using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEduSolutions.Databases.SEDB
{
    public class Assignments
    {
        [Key]
        public int IdAssignments { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string TotalMarks { get; set; }
        [Required]
        public DateTime StartedAt { get; set; }
        [Required]
        public DateTime EndedAt { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public int Classrooms_IdClassrooms { get; set; }
        [Required]
        public int Users_IdUsers { get; set; }
    }
}
