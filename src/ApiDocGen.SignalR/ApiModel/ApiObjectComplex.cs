namespace ApiDocGen.SignalR.ApiModel;

public record ApiObjectComplex(
        string Name,
        string TypeName,
        List<ApiObject> Fields)
    : ApiObject(Name, TypeName, ApiObjectType.Complex);