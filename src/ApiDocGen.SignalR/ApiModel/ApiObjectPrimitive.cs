namespace ApiDocGen.SignalR.ApiModel;

public record ApiObjectPrimitive(
        string Name,
        string TypeName)
    : ApiObject(Name, TypeName, ApiObjectType.Primitive);