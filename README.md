# ApiDocGen.SignalR
SignalR API documentation generator

## How to use

```
ApiDocGenOptions options = new("Some SignalR API", new[]
{
    new ApiDocGenHubInfo(typeof(ApiHub), "hubs/api")
});

ApiDocGenerator generator = new(options);
ApiInfo apiInfo = generator.Generate();

ApiDocMarkdownRenderer renderer = new();
string markdown = renderer.Render(apiInfo);
```