using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using SmartEduSolutions.Databases;
using SmartEduSolutions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartEduSolutions
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // All origin CROS
        readonly string AllowAllOriginCors = "_AllowAllOrigin";

        // Specific origins CROS

        // readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var claimConfigurationSection = Configuration.GetSection(nameof(ClaimConfiguration));
            services.Configure<ClaimConfiguration>(claimConfigurationSection);
            var claimConfig = claimConfigurationSection.Get<ClaimConfiguration>();

            #region JWT Authentication

            var key = Encoding.ASCII.GetBytes(claimConfig.AppKey);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };
            });

            #endregion

            #region Allow all origin CROS request

            services.AddCors(options =>
            {
                options.AddPolicy(AllowAllOriginCors, policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            #endregion

            #region  Allow specific origin CROS request


            //services.AddCors(options =>
            //{
            //    options.AddPolicy(name: MyAllowSpecificOrigins,
            //                      builder =>
            //                      {
            //                          builder.WithOrigins("http://example.com",
            //                                              "http://www.contoso.com");
            //                      });
            //});

            #endregion

            services.AddControllers();
            services.AddControllers().AddNewtonsoftJson();

            #region Api versioning 

            services.AddApiVersioning(
                options =>
                {
                    // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                    options.ReportApiVersions = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                });


            #endregion

            #region Mysql Connection string from appSettings

            var dbConnetionStringSection = Configuration.GetSection("ConnectionString");
            services.Configure<DbConnectionSettings>(dbConnetionStringSection);
            var dbConnectionSettings = dbConnetionStringSection.Get<DbConnectionSettings>();
            services.AddDbContext<SEDBContext>(options => options.UseMySql(dbConnectionSettings.SEDBConnection));

            #endregion

            // All Register Services from extension 

            services.RegisterServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(AllowAllOriginCors);

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "UploadedContent")),
                RequestPath = "/UploadedContent"

            });

            app.UseSerilogRequestLogging();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
