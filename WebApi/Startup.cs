using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ReservationSystem.Controllers;
using WebApi.Services;

namespace ReservationSystem
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
            services.AddCors(corsOptions =>
            {
                corsOptions.AddDefaultPolicy(corsPolicyBuilder =>
                {
                    corsPolicyBuilder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });


            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(jwtBearerOptions =>
                {
                    jwtBearerOptions.SaveToken = true;

                    var issuerSigningKey = AuthenticationController.TokenEncryptionKey;

                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = AuthenticationController.ValidIssuer,
                        ValidAudience = AuthenticationController.ValidAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(issuerSigningKey),
                        ClockSkew = TimeSpan.FromMinutes(2)
                    };
                });

            services.AddTransient<PlacesDetailService>();
            services.AddTransient<PlacesTextsearchService>();
            services.AddSingleton<HumanReadableKeyGeneratorService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(swaggerGenOptions =>
            {
                swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ReservationSystemAPI"
                });

                swaggerGenOptions.IncludeXmlComments(Path.Combine(
                    Configuration.GetValue<string>(WebHostDefaults.ContentRootKey), "ReservationSystem.xml"));


                swaggerGenOptions.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description =
                            @"JWT Authorization header using the Bearer schema. Example ""Bearer {token}""",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Name = "Authorization",
                        Scheme = "bearer", //This is were it was not working for me. I was using uppercase B
                    }
                );

                swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            //Scheme = "oauth2",
                            //Name = "Bearer",
                            //In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();


            app.UseCors(
                // ToDo for more Security
                // x => x.WithOrigins("http://example.com").AllowAnyMethod()
                );

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseMvc();

            app.UseSwagger(x => { x.RouteTemplate = "api-docs/{documentName}/oas3.json"; });
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/api-docs/v1/oas3.json", "ReservationSystemAPI");
                x.RoutePrefix = "swagger/ui";
            });
        }
    }
}