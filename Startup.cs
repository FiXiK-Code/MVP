using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVP.Date;
using MVP.Date.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVP.Date.Models;
using Task = MVP.Date.Models.Tasks;
using TaskStatus = MVP.Date.Models.TaskStatus;
using MVP.Date.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Pomelo.EntityFrameworkCore.MySql.Storage;


namespace MVP
{
    public class Startup
    {
        public IConfigurationRoot _config;
         public Startup(Microsoft.Extensions.Hosting.IHostingEnvironment hostEvn)
        {
            _config = new ConfigurationBuilder().SetBasePath(hostEvn.ContentRootPath).AddJsonFile("appsettings.json").Build();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = "MyAuthServer",
                    ValidAudience = "MyAuthClient",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysupersecret_secretkey!123")),
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
                };
            });


           

            var config = _config.GetSection("EmailConfiguration").Get<EmailConfig>();
            services.AddSingleton(config);

            services.AddScoped<IEmailService, EmailServiceRep>();


            services.AddTransient<ICompanyStructure, CompanyStuctureRep>();
            services.AddTransient<IDivision, DivisionRep>();
            services.AddTransient<IPost, PostRep>();
            services.AddTransient<IProject, ProjectRep>();
            services.AddTransient<IRole, RoleRep>();
            services.AddTransient<IStaff, StaffRep>();
            services.AddTransient<IStage, StageRep>();
            services.AddTransient<ITask, TaskRep>();
            services.AddTransient<ITaskStatus, TaskStatusRep>();
            services.AddTransient<ILogistickTask, LogistickTaskRep>();
            services.AddTransient<ILogisticProject, LogistickProjectRep>();

            services.AddDbContext<AppDB>(
                   options =>
                   {
                       options.UseMySql($"server=localhost;userid=root;pwd=root;port=3306;database=mvp");
                   });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc(option => option.EnableEndpointRouting = false);

            services.AddMemoryCache();
            services.AddSession(p => {
                p.IdleTimeout = TimeSpan.FromHours(9);
            });

            services.AddControllersWithViews();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppDB context)
        {
            

            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();


            app.UseAuthentication();

            context.Database.Migrate();
            app.UseRouting();

            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();

            app.UseMvc(routs =>
            {
                routs.MapRoute(name: "default", template: "{controller=Login}/{action=Index}/{id?}");

            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });

            using (var scope = app.ApplicationServices.CreateScope())
            {
                AppDB content = scope.ServiceProvider.GetRequiredService<AppDB>();
                DBObjects.Initial(content);
            }
        }
    }

}
