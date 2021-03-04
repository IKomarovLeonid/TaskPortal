using Microsoft.Extensions.DependencyInjection;

namespace Core.API.Startup
{
    static class SwaggerRegistration
    {
        public static void RegisterSwagger(this IServiceCollection services)
        {
            services.AddSwaggerDocument(settings =>
            {
                settings.SchemaType = NJsonSchema.SchemaType.OpenApi3;
                settings.AllowReferencesWithProperties = true;
                settings.Title = "Task Portal";

            });
        }
    }
}
