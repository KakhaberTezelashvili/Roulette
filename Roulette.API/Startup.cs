using System;
using System.IO;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Roulette.API.CQRSServices;
using Roulette.API.Filters;
using Roulette.Infrastructure.DBProvider;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Roulette.Shared.Rows;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Roulette.Application.SignInManager;

namespace Roulette.API
{
    public class Startup
    {
        public const string v1 = "version";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            DataAccessLayerBase.Configuration = Configuration;

            services.AddHttpContextAccessor();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.Configure<TokenSettings>(Configuration.GetSection("tokenSettings"));

            var token = Configuration.GetSection("tokenSettings").Get<TokenSettings>();
            var secret = Encoding.ASCII.GetBytes(token.Secret);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Secret)),
                        ValidIssuer = token.Issuer,
                        ValidAudience = token.Audience,
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddMvc(options =>
            {
                options.Filters.Add<ApiExceptionFilter>();

            }).AddJsonOptions(x =>
            {
                x.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                x.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                x.SerializerSettings.Formatting = Formatting.Indented;
                x.SerializerSettings.ContractResolver = new DefaultContractResolver();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddCors(c => c.AddPolicy("AllowAllHeaders", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(nameof(v1), new Info()
                {
                    Title = $"Roulette Api {nameof(v1)}",
                    Description = "Service for managing virtual roulette game",
                    Version = nameof(v1)
                });

                var currentDir = new DirectoryInfo(AppContext.BaseDirectory);
                foreach (var xmlCommentFile in currentDir.EnumerateFiles("Roulette.*.xml"))
                    c.IncludeXmlComments(xmlCommentFile.FullName);
                c.DescribeAllEnumsAsStrings();

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme { In = "header", Description = "Please enter JWT with Bearer into field", Name = "Authorization", Type = "apiKey" });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> { { "Bearer", Enumerable.Empty<string>() }, });
            });

            services.AddTransient<IDBProvider, DBProvider>();
            services.AddTransient<ISignInManager, SignInManager>();

            var container = new ContainerBuilder();

            CommandServices.RegisterServices(container, services);
            QueryServices.RegisterServices(container, services);

            container.Populate(services);

            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvc();

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "swagger";
                c.SwaggerEndpoint($"{nameof(v1)}/swagger.json", $"Roulette Api {nameof(v1)} Proxy");
            });

            app.UseCors("AllowAllHeaders");
        }
    }
}