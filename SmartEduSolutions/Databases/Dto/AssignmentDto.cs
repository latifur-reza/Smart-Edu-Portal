using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEduSolutions.Databases.Dto
{
    public class AssignmentDto
    {
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
        public int IdClassrooms { get; set; }
        [Required]
        public int IdUsers { get; set; }
    }
}
