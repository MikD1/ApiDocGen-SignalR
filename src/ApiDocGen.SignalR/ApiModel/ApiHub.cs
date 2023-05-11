namespace ApiDocGen.SignalR.ApiModel;

public record ApiHub(
    string Route,
    List<ApiMethod> Methods,
    List<ApiMethod> ClientMethods);