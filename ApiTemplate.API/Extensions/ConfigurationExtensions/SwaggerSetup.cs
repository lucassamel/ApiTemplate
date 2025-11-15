using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace ApiTemplate.API.Extensions.ConfigurationExtensions
{
    public static class SwaggerSetup
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Api Template",
                    Version = "v1",
                    Description = "Documentação da API",
                    Contact = new OpenApiContact
                    {
                        Name = "Lucas Samel",
                        Email = "lucas.samel@outlook.com"
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header utilizando o esquema Bearer. 
                                Insira 'Bearer' [espaço] e entre com o token no input abaixo. 
                                Exemplo: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,

                    },
                    new List<string>()
                }
            });

                //options.OperationFilter<OptionalParameterOperationFilter>();
                //options.OperationFilter<EnumSchemaFilter>();
            });
            return services;
        }

        public static WebApplication? UseSwaggerDocumentation(this WebApplication app)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var apiVersionProvider = app.Services.GetService<IApiVersionDescriptionProvider>();
                ArgumentNullException.ThrowIfNull(apiVersionProvider);

                foreach (var apiVersion in apiVersionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{apiVersion.GroupName}/swagger.json"
                        , apiVersion.GroupName);
                }
                options.RoutePrefix = string.Empty;
            });
            return app;
        }
    }
}
