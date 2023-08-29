
namespace Microsoft.OpenApi.ApiManifest.OpenAI;

public class OpenApiPluginFactory
{

    public static OpenAIPluginManifest CreateOpenAIPluginManifest()
    {
        var manifest = new OpenAIPluginManifest
        {
            SchemaVersion = "v1"
        };
        return manifest;
    }
}