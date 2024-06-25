using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Car_Mender.API.Features.Swagger;

public class JsonPatchDocumentFilter : IOperationFilter
{
	public void Apply(OpenApiOperation operation, OperationFilterContext context)
	{
		var isJsonPatchOperation = context.MethodInfo.GetCustomAttributes(true)
			.OfType<HttpPatchAttribute>().Any();

		if (!isJsonPatchOperation)
		{
			return;
		}

		if (operation.RequestBody == null)
		{
			return;
		}

		var patchDocumentType = context.MethodInfo.GetParameters()
			.FirstOrDefault(p =>
				p.ParameterType.IsGenericType &&
				p.ParameterType.GetGenericTypeDefinition() == typeof(JsonPatchDocument<>))
			?.ParameterType;

		if (patchDocumentType == null)
		{
			return;
		}

		operation.RequestBody.Content["application/json-patch+json"] = new OpenApiMediaType
		{
			Schema = new OpenApiSchema
			{
				Type = "array",
				Items = new OpenApiSchema
				{
					Type = "object",
					Properties = new Dictionary<string, OpenApiSchema>
					{
						["op"] = new() { Type = "string" },
						["path"] = new() { Type = "string" },
						["value"] = new()
						{
							AnyOf = new List<OpenApiSchema>
							{
								new() { Type = "object" },
								new() { Type = "array", Items = new OpenApiSchema { Type = "object" } }
							}
						}
					}
				}
			}
		};
	}
}