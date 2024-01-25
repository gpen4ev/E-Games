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

            if (context.Type == typeof(UpdateUserModel))
            {
                schema.Example = new OpenApiObject()
                {
                    ["username"] = new OpenApiString("John Doe"),
                    ["phoneNumber"] = new OpenApiString("123-123-123"),
                    ["addressDelivery"] = new OpenApiString("123 Street")
                };
            }

            if (context.Type == typeof(UpdatePasswordModel))
            {
                schema.Example = new OpenApiObject
                {
                    ["currentPassword"] = new OpenApiString("Password1!"),
                    ["newPassword"] = new OpenApiString("Password12!")
                };
            }
        }
    }
}
