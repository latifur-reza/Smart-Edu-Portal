using SmartEduSolutions.Databases.SEDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEduSolutions.Databases.Seeder
{
    public class ClassroomsSeed
    {
        public ClassroomsSeed()
        {
            
        }

        private List<Classrooms> Classrooms
        {
            get
            {
                return new List<Classrooms>
                {
                    new Classrooms
                    {
                        IdClassrooms = 1,
                        Title = "Class 1",

                        CreatedAt = DateTime.Now
                    },
                     new Classrooms
                    {
                        IdClassrooms = 2,
                        Title = "Class 2",

                        CreatedAt = DateTime.Now
                    },
                      new Classrooms
                    {
                        IdClassrooms = 3,
                        Title = "Class 3",

                        CreatedAt = DateTime.Now
                    },
                    new Classrooms
                    {
                        IdClassrooms = 4,
                        Title = "Class 4",

                        CreatedAt = DateTime.Now
                    },
                     new Classrooms
                    {
                        IdClassrooms = 5,
                        Title = "Class 5",

                        CreatedAt = DateTime.Now
                    },
                    new Classrooms
                    {
                        IdClassrooms = 6,
                        Title = "Class 6",

                        CreatedAt = DateTime.Now
                    },
                      new Classrooms
                    {
                        IdClassrooms = 7,
                        Title = "Class 7",

                        CreatedAt = DateTime.Now
                    },
                      new Classrooms
                    {
                        IdClassrooms = 8,
                        Title = "Class 8",

                        CreatedAt = DateTime.Now
                    },
                      new Classrooms
                    {
                        IdClassrooms = 9,
                        Title = "Class 9",

                        CreatedAt = DateTime.Now
                    },
                      new Classrooms
                    {
                        IdClassrooms = 10,
                        Title = "Class 10",

                        CreatedAt = DateTime.Now
                    }
                };
            }
        }

        public List<Classrooms> GetClassrooms()
        {
            return Classrooms;
        }

    }
}
