using SmartEduSolutions.Databases.SEDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEduSolutions.Databases.Seeder
{
    public class AssignmentSeed
    {
        public AssignmentSeed()
        {

        }

        public List<Assignments> GetAssignment()
        {
            var assignments = new List<Assignments>();
            for (int i = 1; i <= 10; i++)
            {
                var data = new Assignments();
                data.Title = "Assignment "+i;
                data.TotalMarks = "5"+i;
                data.StartedAt = DateTime.Now.AddHours(i);
                data.EndedAt = (DateTime.Now).AddDays(i);
                data.Classrooms_IdClassrooms = i;
                data.Users_IdUsers = i;
                data.CreatedAt = DateTime.Now;

                assignments.Add(data);
            }
            return assignments;
        }
    }
}
