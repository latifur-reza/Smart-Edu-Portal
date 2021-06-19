using SmartEduSolutions.Databases.SEDB;
using SmartEduSolutions.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEduSolutions.Databases.Seeder
{
    public class UserHasClassroomSeed
    {
        public UserHasClassroomSeed()
        {

        }

        public List<User_has_Classroom> GetUserHasClassrooms()
        {
            var UserHasClassroom = new List<User_has_Classroom>();
            for (int i = 1; i <= 10; i++)
            {
                var creator = new User_has_Classroom();
                creator.Role = Roles.Creator;
                creator.CreatedAt = DateTime.Now;
                creator.Users_IdUsers = i;
                creator.Classrooms_IdClassrooms = i;

                UserHasClassroom.Add(creator);
                for (int j=1; j<=10; j++)
                {
                    if(i != j)
                    {
                        var student = new User_has_Classroom();
                        student.Role = Roles.Student;
                        student.CreatedAt = DateTime.Now;
                        student.Users_IdUsers = j;
                        student.Classrooms_IdClassrooms = i;

                        UserHasClassroom.Add(student);
                    }
                }
            }
            return UserHasClassroom;
        }

    }
}
