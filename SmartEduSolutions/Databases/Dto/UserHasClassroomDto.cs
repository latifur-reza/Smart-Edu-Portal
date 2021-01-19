using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEduSolutions.Databases.Dto
{
    public class UserHasClassroomDto
    {
        public int IdUserHasClassroom { get; set; }
        public string Role { get; set; }
        public int IdUsers { get; set; }
        public int IdClassrooms { get; set; }

    }

    public class UserForClassroomDto : UserHasClassroomDto
    {
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        public string ProfilePic { get; set; }
    }

    public class ClassroomForUserDto : UserHasClassroomDto
    {
        [Required]
        public string Title { get; set; }
        public string CoverPic { get; set; }
    }
}
