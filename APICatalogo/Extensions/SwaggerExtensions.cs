using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace APICatalogo.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSettingsSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "APICatalogo",
                    Description = "Catálogo de Produtos e Categorias",
                    TermsOfService = new Uri("https://rodrigo.net/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Rodrigu",
                        Email = "rodrigu.dev@outlook.com",
                        Url = new Uri("https://rodrigo.net"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "About License",
                        Url = new Uri("https://rodrigo.net/license")
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                     {
                         {
                           new OpenApiSecurityScheme
                           {
                              Reference = new OpenApiReference
                              {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                               }
                            },
                           new string[] {}
                          }
                     });

            });
            return services;
        }
    }
}
