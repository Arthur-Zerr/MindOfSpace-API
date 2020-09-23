using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MindOfSpace_Api.BusinessLogic;
using MindOfSpace_Api.DAL;
using MindOfSpace_Api.Helpers;

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

            services.AddControllers();
            services.AddSwaggerGen();

            services.AddScoped<LogUserActivity>();
            services.AddScoped<MindOfSpaceRepository>();
            services.AddScoped<HighscoreRepository>();
            services.AddScoped<GameLogic>();
            services.AddScoped<PlayerLogic>();
            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MindOfSpaceContext mindOfSpaceContext)
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
            }
    
            app.UseHttpsRedirection();
            app.UseRouting();
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
