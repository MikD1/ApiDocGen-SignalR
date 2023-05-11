namespace ApiDocGen.SignalR.ApiModel;

public record ApiMethod(
    string Name,
    List<ApiObject> Parameters,
    ApiObject? Response);