using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEduSolutions.Databases.Dto
{
    public class ClassroomDto
    {
        public int IdClassrooms { get; set; }
        [Required]
        public string Title { get; set; }
        public string CoverPic { get; set; }
    }
}
