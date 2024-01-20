using E_Games.Web.ViewModels;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace E_Games.Web.Infrastructure
{
    public class CustomSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(SignModel))
            {
                schema.Example = new OpenApiObject
                {
                    ["email"] = new OpenApiString("john.doe@example.com"),
                    ["password"] = new OpenApiString("Password1!")
                };
            }
        }
    }
}
