namespace ApiDocGen.SignalR.ApiModel;

public abstract record ApiObject(
    string Name,
    string TypeName,
    ApiObjectType ObjectType);