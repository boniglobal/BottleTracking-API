using BottleTracking_API.Helpers;
using Core.Constants;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace BottleTracking_API.Dependencies
{
    public static class Swagger
    {
        public static void AddSwaggerConfigurations(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = DocumentTexts.AuthDesc
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

                c.SchemaFilter<CustomSchemeFilters>();
                c.UseInlineDefinitionsForEnums();
                c.EnableAnnotations();

                var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.XML";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, true);
                c.IncludeXmlComments($@"{AppDomain.CurrentDomain.BaseDirectory}\Core.xml");
            });
        }
    }
}
