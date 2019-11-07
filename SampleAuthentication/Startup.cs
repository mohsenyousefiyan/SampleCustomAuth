using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SampleAuthentication.DAL;
using SampleAuthentication.InfraStructures.ActionFilters;
using SampleAuthentication.InfraStructures.Extentions;

namespace SampleAuthentication
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
            services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlServer("server=.;database=SampleAuthentication;trusted_connection=true;");
            });

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie();

            //services.AddScoped<IClaimsTransformation,UserClaimsProvider>();
            services.AddScoped<CustomAuthorizeFilter>();

            services.AddAuthorization(opts => {
                opts.AddPolicy("mobile", policy =>
                {
                    policy.RequireClaim("Mobile");
                });
                opts.AddPolicy("custommobile", policy =>
                {
                    policy.RequireClaim("Mobile","09124441325");
                });
                opts.AddPolicy("role", policy =>
                {
                    policy.RequireClaim("Role","Admin");                    
                });
                opts.AddPolicy("mobilerole", policy =>
                {
                    policy.RequireClaim("Mobile","09132754661");
                    policy.RequireClaim("Role", "Admin");
                });
            });

        services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
    }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseStaticFiles();            
            app.UseMvcWithDefaultRoute();
        }
    }
}
