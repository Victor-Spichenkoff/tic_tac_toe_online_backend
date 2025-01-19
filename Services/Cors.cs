namespace asp_rest_model.Services;

public static class CorsService
{
    public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration, string environment)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("DevelopmentCorsPolicy", builder =>
            {
                
                builder.WithOrigins("http://localhost:3000")  
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });

            options.AddPolicy("ProductionCorsPolicy", builder =>
            {
                string[] allowedOrigins = ["https://tic-tac-toe-online-six.vercel.app"];
                builder.WithOrigins(allowedOrigins) // Adiciona os sites de produção
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }
}
