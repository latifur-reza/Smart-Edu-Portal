using Microsoft.AspNetCore.Identity;
using SmartEduSolutions.Databases.SEDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEduSolutions.Databases.Seeder
{
    public static class SeedRegister
    {
        public static void IntializeSeedData(string connectionString)
        {
            using (var context = new SEDBContext(connectionString))
            {
                var userSeedData = new UsersSeed(new PasswordHasher<Users>());
                context.Database.EnsureCreated();
                var user = context.Users.FirstOrDefault();
                if (user == null)
                {
                    context.Users.AddRange(userSeedData.GetUsers());
                }
                context.SaveChanges();

                var classroomSeedData = new ClassroomsSeed();
                context.Database.EnsureCreated();
                var classroom = context.Classrooms.FirstOrDefault();
                if (classroom == null)
                {
                    context.Classrooms.AddRange(classroomSeedData.GetClassrooms());
                }
                context.SaveChanges();

                var userHasClassroomSeed = new UserHasClassroomSeed();
                context.Database.EnsureCreated();
                var userHasClassroom = context.User_has_Classroom.FirstOrDefault();
                if (userHasClassroom == null)
                {
                    context.User_has_Classroom.AddRange(userHasClassroomSeed.GetUserHasClassrooms());
                }
                context.SaveChanges();

                var assignmentSeed = new AssignmentSeed();
                context.Database.EnsureCreated();
                var assignments = context.Assignments.FirstOrDefault();
                if (assignments == null)
                {
                    context.Assignments.AddRange(assignmentSeed.GetAssignment());
                }
                context.SaveChanges();

            }
        }
    }
}
