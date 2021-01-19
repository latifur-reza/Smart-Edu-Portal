using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEduSolutions.Databases.SEDB
{
    public class User_has_Classroom
    {
        [Key]
        public int IdUserHasClassroom { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public int Users_IdUsers { get; set; }
        public int Classrooms_IdClassrooms { get; set; }
    }
}
