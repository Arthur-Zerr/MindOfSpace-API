using System;
using System.Text;
using medsurv_diary_apigateway.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MindOfSpace_Api.BusinessLogic;
using MindOfSpace_Api.DAL;
using MindOfSpace_Api.Helpers;
using MindOfSpace_Api.Models;

namespace MindOfSpace_Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MindOfSpaceContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            IdentityBuilder builder = services.AddIdentityCore<Player>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;

                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = new TimeSpan(99, 99, 99);
                options.Lockout.MaxFailedAccessAttempts = 99999;
            })
            .AddRoles<IdentityRole>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddRoleValidator<RoleValidator<IdentityRole>>()
            .AddSignInManager<SignInManager<Player>>()
            .AddUserManager<UserManager<Player>>()
            .AddEntityFrameworkStores<MindOfSpaceContext>();

            // transient service for password generation
            services.Configure<PasswordHasherOptions>(options => options.IterationCount = 100000);

            // switches email confirmation off
            services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
            });
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("AppSettings:JwtKey").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminRole",
                    policy => policy.RequireRole("Admin")); // Admin login
                options.AddPolicy("UserRole",
                    policy => policy.RequireRole("User")); // Normal User
            });

            services.AddCors(optionsCors =>
            {
                optionsCors.AddPolicy("AllowSetOrigins", options =>
                {
                    options.WithOrigins(Configuration.GetSection("AppSettings:Cors").Get<string[]>());
                    options.AllowAnyHeader();
                    options.AllowAnyMethod();
                    options.AllowCredentials();
                });
            });
            services.AddControllers();
            services.AddSwaggerGen();

            services.AddScoped<LogUserActivity>();
            services.AddScoped<MindOfSpaceRepository>();
            services.AddScoped<PlayerRepository>();
            services.AddScoped<HighscoreRepository>();
            services.AddScoped<PlayerLogic>();
            services.AddScoped<JWTTokenFactory>();
            services.AddScoped<LobbyHelper>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MindOfSpaceContext mindOfSpaceContext, UserManager<Player> userManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (mindOfSpaceContext.Database.CanConnect())
            {
                mindOfSpaceContext.Database.Migrate();
            }
            else
            {
                mindOfSpaceContext.Database.Migrate();
                ApplicationDbInitializer.SeedRoles(mindOfSpaceContext);
                ApplicationDbInitializer.SeedUsers(userManager);
            }
    
            app.UseCors("AllowSetOrigins");


            app.UseHttpsRedirection();
            app.UseHsts();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MindOfSpace API");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
