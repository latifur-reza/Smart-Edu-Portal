using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEduSolutions.Databases.SEDB
{
    public class Classrooms
    {
        [Key]
        public int IdClassrooms { get; set; }
        [Required]
        public string Title { get; set; }
        public string CoverPic { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
