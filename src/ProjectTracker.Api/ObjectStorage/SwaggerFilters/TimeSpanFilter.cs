using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ProjectTracker.Api.ObjectStorage.SwaggerFilters;

public class TimeSpanFilter : ISchemaFilter
{
	public void Apply(OpenApiSchema schema, SchemaFilterContext context)
	{
		if (context.Type == typeof(TimeSpan) || context.Type == typeof(TimeSpan?))
		{
			schema.Type = "string";
			schema.Format = "duration";
			schema.Example = new Microsoft.OpenApi.Any.OpenApiString("00:00:00");
		}
	}
}
