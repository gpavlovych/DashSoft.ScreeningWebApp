using System;
using System.IO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Screening.WebAPI.Core.Authorization;
using Screening.WebAPI.Core.Data;
using Swashbuckle.AspNetCore.Swagger;

namespace Screening.WebAPI.Core
{
    public class Startup
    {
        public Startup(IHostingEnvironment environment)
        {
            if (environment == null)
                throw new ArgumentNullException(nameof(environment));

            var builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            if (environment.IsDevelopment())
                builder.AddUserSecrets<Startup>();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddDbContext<ScreeningWebApiCoreContext>(options =>
                options.UseInMemoryDatabase("ScreeningWebAPICoreDatabase"));

            services.AddIdentity<User, Role>()
                .AddUserManager<CombinedUserManager>()
                .AddEntityFrameworkStores<ScreeningWebApiCoreContext>()
                .AddDefaultTokenProviders();

            services.Configure<TokenAuthenticationOptions>(Configuration);

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var configurationOptions = Configuration.Get<TokenAuthenticationOptions>();
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configurationOptions.TokenIssuer,
                        ValidateAudience = true,
                        ValidAudience = configurationOptions.TokenAudience,
                        ValidateLifetime = true,
                        IssuerSigningKey = configurationOptions.TokenSigningKey.ToSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true
                    };
                });

            services.AddMvcCore()
                .AddJsonFormatters(options =>
                {
                    options.ContractResolver = new CamelCasePropertyNamesContractResolver();
                })
                .AddCors()
                .AddApiExplorer()
                .AddAuthorization(options =>
                {
                    options.DefaultPolicy =
                        new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                            .RequireAuthenticatedUser()
                            .Build();

                    options.AddPolicy("teacher",
                        builder => { builder.AddRequirements(new TeacherAuthorizationRequirement()); });
                });

            services.AddScoped<IAuthorizationHandler, TeacherAuthorizationHandler>();

            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(
                    "oauth2",
                    new OAuth2Scheme
                    {
                        Flow = "password",
                        TokenUrl = "/api/users/token",
                        Extensions = {{"tokenName", "token"}}
                    });
                options.SwaggerDoc(
                    "v1",
                    new Info
                    {
                        Title = "Stakeholders API",
                        Version = "v1"
                    });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            if (env == null)
                throw new ArgumentNullException(nameof(env));

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(
                o => { o.SwaggerEndpoint("/swagger/v1/swagger.json", "Stakeholders API V1"); });

            // the middleware below supports corrent ng2 routing
            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value) &&
                    !(context.Request.Path.Value.StartsWith("api") || context.Request.Path.Value.StartsWith("swagger")))
                {
                    context.Request.Path = "/index.html";
                    context.Response.StatusCode = 200;
                    await next();
                }
            });
        }
    }
}
