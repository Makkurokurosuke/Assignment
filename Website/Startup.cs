using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Website.Filter;
using Website.Services;

namespace Website
{
    public class Startup
    {
        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            Logger = logger;
        }

        public IConfiguration Configuration { get; }
        public ILogger Logger { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Logger.LogInformation("Website is Starting up.");



            services.AddLocalization(o => o.ResourcesPath = "Resources");

            services.AddMvc(options =>
            {
                options.Filters.Add(new GlobalActionFilter());
                options.Filters.Add(new WebsiteAuthorizationFilter());
                options.Filters.Add(new Microsoft.AspNetCore.Mvc.MiddlewareFilterAttribute(typeof(LocalizationPipeline)));

            })
            .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
            .AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                    factory.Create(typeof(Resources.SharedResource));
            });

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Dependency Injection
            services.AddHttpClient<IUserReviewApiService, UserReviewApiService>(
                c =>
                {
                    c.BaseAddress = new Uri(Configuration.GetValue<string>("WebAPI:BaseURL"));
                }
            );

            services.AddHttpClient<IReviewRatingApiService, ReviewRatingApiService>(
                c =>
                {
                    c.BaseAddress = new Uri(Configuration.GetValue<string>("WebAPI:BaseURL"));
                }
            );

            services.AddHttpClient<IAuthorizationApiService, AuthorizationApiService>(
                c =>
                {
                    c.BaseAddress = new Uri(Configuration.GetValue<string>("WebAPI:BaseURL"));
                }
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStatusCodePagesWithReExecute("/Error/Error", "?sc={0}");

            // app.UseDeveloperExceptionPage();

            app.UseExceptionHandler("/Error/Error");


            AppHttpContext.Services = app.ApplicationServices;

            app.UseCookiePolicy();
            app.UseSession();



            app.UseAuthentication();


            app.UseDefaultFiles(new DefaultFilesOptions
            {
                DefaultFileNames = new
                List<string> { "default.html" }
            });
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Default}/{action=LogIn}/{id?}");
            });
        }
    }



    public class LocalizationPipeline
    {
        public void Configure(IApplicationBuilder app)
        {
            var supportedCultures = new[]
             {
                new System.Globalization.CultureInfo("en-CA"),
                new System.Globalization.CultureInfo("fr-CA"),
            };

            var requestLocalizationOptions = new RequestLocalizationOptions()
            {
                DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(culture: "en-CA", uiCulture: "en-CA"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };


            requestLocalizationOptions.RequestCultureProviders = new[] {
                new Microsoft.AspNetCore.Localization.Routing.RouteDataRequestCultureProvider()
                {
                    RouteDataStringKey = "lang",
                    Options = requestLocalizationOptions
                }
            };

            app.UseRequestLocalization(requestLocalizationOptions);

        }
    }
}
