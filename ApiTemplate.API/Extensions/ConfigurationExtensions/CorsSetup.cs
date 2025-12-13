namespace ApiTemplate.API.Extensions.ConfigurationExtensions
{
    public static class CorsSetup
    {
        public static string CORS_POLICY_NAME => "CorsPolicy";
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy(CORS_POLICY_NAME, builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
                //.SetIsOriginAllowed((host) => true)
                //.AllowCredentials();
            }));
            return services;
        }
    }
}
