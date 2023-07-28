
namespace Microsoft.OpenApi.ApiManifest.OpenAI;

public class OpenApiPluginFactory {

    public static OpenAIPluginManifest CreateOpenAIPluginManifest() {
        var manifest = new OpenAIPluginManifest();
        manifest.SchemaVersion = "v1";
        return manifest;
    }
}