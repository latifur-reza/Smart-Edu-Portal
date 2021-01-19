using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEduSolutions.Databases.SEDB
{
    public class Users
    {
        [Key]
        public int IdUsers { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string ProfilePic { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
