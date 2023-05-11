namespace ApiDocGen.SignalR;

public record ApiDocGenOptions(
    string ApiName,
    ApiDocGenHubInfo[] Hubs);