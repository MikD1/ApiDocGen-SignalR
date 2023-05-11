namespace ApiDocGen.SignalR.ApiModel;

public record ApiInfo(
    string Name,
    List<ApiHub> Hubs);