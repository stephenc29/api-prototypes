using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

// TODO SCHURCH: Consider if there's a better way to do this
namespace ObservableVersionedApi.Swagger
{
    public class ReplaceVersionWithExactValueInPath : IDocumentFilter
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ReplaceVersionWithExactValueInPath(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var path in swaggerDoc.Paths.Keys.ToList())
            {
                if (path.Contains("{version}"))
                {
                    var newPath = path.Replace("{version}", swaggerDoc.Info.Version);
                    var item = swaggerDoc.Paths[path];
                    swaggerDoc.Paths.Remove(path);
                    swaggerDoc.Paths.Add(newPath, item);
                }
            }
        }
    }

}
