using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SmartEduSolutions.DataControl.Interfaces;
using SmartEduSolutions.DataControl.Services;
using SmartEduSolutions.Databases.SEDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEduSolutions.Configuration
{
    public static class ServiceRegistry
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IPasswordHasher<Users>, PasswordHasher<Users>>();
            services.AddTransient<IAuth, AuthService>();
            services.AddTransient<IUser, UserService>();
            services.AddTransient<IClassroom, ClassroomService>();
            services.AddTransient<IAssignment, AssignmentService>();
            services.AddTransient<IQuestion, QuestionService>();
        }
    }
}
