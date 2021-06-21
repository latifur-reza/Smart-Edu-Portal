using SmartEduSolutions.Databases.SEDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEduSolutions.Databases.Seeder
{
    public class QuestionSeed
    {
        public QuestionSeed()
        {

        }

        public List<Questions> GetQuestion()
        {
            var questions = new List<Questions>();
            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 5 ; j++)
                {
                    var data = new Questions();
                    data.QuestionPart = "Assignment " + i + " Question " + j;
                    data.Marks = (i*10) + j;
                    data.CreatedAt = DateTime.Now;
                    data.UpdatedAt = DateTime.Now;
                    data.Classrooms_IdClassrooms = i;
                    data.Assignments_IdAssignments = i;

                    questions.Add(data);
                }
                
            }
            return questions;
        }
    }
}
