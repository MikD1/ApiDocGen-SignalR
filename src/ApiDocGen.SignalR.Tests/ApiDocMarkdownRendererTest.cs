using ApiDocGen.SignalR.ApiModel;

namespace ApiDocGen.SignalR.Tests;

[TestClass]
public class ApiDocMarkdownRendererTest
{
    [TestMethod]
    public void RenderMarkdownApiDocumentation()
    {
        ApiDocGenOptions options = new("Some SignalR API", new[]
        {
            new ApiDocGenHubInfo(typeof(ApiDocGen.SignalR.Tests.TestApiHub), "hubs/api")
        });

        ApiDocGenerator generator = new(options);
        ApiInfo apiInfo = generator.Generate();

        ApiDocMarkdownRenderer renderer = new();
        string markdown = renderer.Render(apiInfo);

        Assert.IsFalse(string.IsNullOrWhiteSpace(markdown));
    }
}