namespace ApiDocGen.SignalR.ApiModel;

public record ApiObjectEnum(
        string Name,
        string TypeName,
        List<string> Values)
    : ApiObject(Name, TypeName, ApiObjectType.Enum);