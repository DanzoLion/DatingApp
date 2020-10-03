using System.Text;      // Encoding
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;   // ITokenService
using API.Services;     // TokenService
using Microsoft.AspNetCore.Authentication.JwtBearer;                // installed from mocrosoft.aspnetcore.authentication.jwtbearer // JwtBearerDefaults
using Microsoft.IdentityModel.Tokens;                                          // TokenValidationParameters
using API.Extensions;                                                       // AddAppicationServices class we created earlier to manage our static extension method

namespace API
{
    public class Startup                                                                                                                                                              // configuration is being injected into our startup class
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)                                                                                                                   // injecting configurations into startup class here // constructor for the class
        {
            _config = config;
            // we access the configuration via _config property
        }

     //    public IConfiguration Configuration { get; }                                                                                                 // removed to adopt an alternative convension
                                                                                                                                                                                                  // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)                                                                              // add classes here to make them available to our app // aka services to be added
        {
            services.AddApplicationServices(_config);                                                                                                   // after refactor, our services are now all contained inside this class
            services.AddControllers();
            services.AddCors();                                                                                                                                      // we add this method to allow CORS communication between different origin ports
           services.AddIdentityServices(_config);                                                                                                        // after refactor, our services are now all contained inside this class

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });
        }
                                                                                                                                                                                 // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }
                                                                                                //NB Sequential Ordering of methods is important here!!
            app.UseHttpsRedirection();                                                                                                                              // from http to https endpoint if entering on http
            app.UseRouting();                                                                                                                                              // our router to route from url to controller
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));            // acts as middleware to allow CORS routing of differing originating ports // x is our implented CORS policy
           app.UseAuthentication();                                                                                                                                 // after we implement our configuration for middleware authentication
            app.UseAuthorization();                                                                                                                                      // authorisation implementation
            app.UseEndpoints(endpoints =>                                                                                                                    // middleware to use endpoints
            {
                endpoints.MapControllers();                                                                                                                         // endpoint to map controllers // looks inside controllers to see what controllers are available // weatherforecast controller
            });
        }
    }
}
