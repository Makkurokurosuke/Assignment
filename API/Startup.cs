using API.Middleware;
using Common;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace API
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
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            #region services.AddAuthentication

            services.AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme
            )
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = AuthConfig.ApiJwtSigningKey,
                        ValidateIssuer = true,
                        ValidIssuer = AuthConfig.ApiJwtIssuer,
                        ValidateAudience = true,
                        ValidAudience = AuthConfig.ApiJwtAudience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            return Task.FromResult(0);
                        },
                        OnChallenge = context =>
                        {
                            string errorMessage = context.AuthenticateFailure != null ?
                                "The access token is expired or invalid." :
                                "The access token is required.";

                            context.Response.OnStarting(async state =>
                            {

                                await new CustomJsonResult(HttpStatusCode.Unauthorized,
                                    new
                                    {
                                        Type = this.GetType().FullName,
                                        Title = errorMessage,
                                        Instance = context.Request?.Path.Value
                                    }
                                    ).SerializeJsonAsync(((JwtBearerChallengeContext)state).Response);
                                return;
                            }, context);
                            return Task.FromResult(0);
                        },
                    };
                });

            #endregion

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "User Review API V1", Version = "v1" });

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };


                options.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey",
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"  Token can be generated from POST: /api/v1/Account/Login with a valid username and password"
                });

                options.AddSecurityRequirement(security);
            });


            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true;
            }).AddXmlDataContractSerializerFormatters();

            services.AddTransient<IUserReviewRepository, UserReviewRepository>();
            services.AddTransient<IReviewRatingRepository, ReviewRatingRepository>();
            services.AddTransient<IUserRepository, UserRepository>();

        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Review API V1");
            });


            app.UseAuthentication();
            app.UseMiddleware(typeof(MiddlewareErrorHandler));


            app.UseMvc();


        }
    }
}
