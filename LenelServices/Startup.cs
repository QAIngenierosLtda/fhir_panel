using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DataConduitManager.Repositories.Logic;
using DataConduitManager.Repositories.Interfaces;
using LenelServices.Repositories.Logic;
using LenelServices.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LenelServices
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddTransient<IDataConduITMgr, DataConduITMgr>();
            services.AddTransient<IBadge, Badge>();
            services.AddTransient<ICardHolder, CardHolder>();
            services.AddTransient<IBadge_REP_LOCAL, Badge_REP_LOCAL>();
            services.AddTransient<ICardHolder_REP_LOCAL, CardHolder_REP_LOCAL>();
            services.AddTransient<IReader, Reader>();
            services.AddTransient<IReader_REP_LOCAL, Reader_REP_LOCAL>();
            services.AddTransient<ILists, Lists>();
            services.AddTransient<ILists_REP_LOCAL, Lists_REP_LOCAL>();
            // Habilitar CORS
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin()
                    .AllowCredentials();
            }));

            //services.AddAuthentication(
            //    CookieAuthenticationDefaults.AuthenticationScheme
            //    ).AddJwtBearer("Bearer", options =>
            //    {
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateAudience = false,
            //            //ValidAudience = "the audience you want to validate",
            //            ValidateIssuer = false,
            //            //ValidIssuer = "the isser you want to validate",
            //            ValidateIssuerSigningKey = true,
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtConfig:Key"])),
            //            ValidateLifetime = true, //validate the expiration and not before values in the token
            //            ClockSkew = TimeSpan.FromMinutes(5) //5 minute tolerance for the expiration date
            //        };
            //    });
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
