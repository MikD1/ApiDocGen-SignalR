using System.Reflection;
using ApiDocGen.SignalR.ApiModel;

namespace ApiDocGen.SignalR;

public class ApiDocGenerator
{
    public ApiDocGenerator(ApiDocGenOptions options)
    {
        _options = options;
    }

    public ApiInfo Generate()
    {
        List<ApiHub> hubs = _options.Hubs
            .Select(x => ProcessHub(x.Route, x.Hub))
            .ToList();

        ApiInfo apiInfo = new(_options.ApiName, hubs);
        return apiInfo;
    }

    private ApiHub ProcessHub(string route, Type hubType)
    {
        List<ApiMethod> methods = FindApiMethods(hubType);
        List<ApiMethod> clientMethods = FindClientApiMethods(hubType);
        ApiHub hub = new(route, methods, clientMethods);
        return hub;
    }

    private List<ApiMethod> FindApiMethods(Type type)
    {
        return type
            .GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
            .Select(ProcessMethod)
            .ToList();
    }

    private List<ApiMethod> FindClientApiMethods(Type type)
    {
        Type? baseType = type.BaseType;
        if (baseType is null)
        {
            return new List<ApiMethod>();
        }

        if (!baseType.IsGenericType ||
            baseType.GetGenericTypeDefinition().FullName != "Microsoft.AspNetCore.SignalR.Hub`1")
        {
            return new List<ApiMethod>();
        }

        Type clientHubType = baseType.GenericTypeArguments[0];
        return FindApiMethods(clientHubType);
    }

    private ApiMethod ProcessMethod(MethodInfo methodInfo)
    {
        List<ApiObject> parameters = methodInfo
            .GetParameters()
            .Select(x => ProcessApiObject(x.ParameterType, string.Empty))
            .Where(x => x is not null)
            .ToList()!;

        ApiObject? response = ProcessApiObject(methodInfo.ReturnParameter.ParameterType, string.Empty);

        ApiMethod apiMethod = new(methodInfo.Name, parameters, response);
        return apiMethod;
    }

    private ApiObject? ProcessApiObject(Type apiObjectType, string propertyName)
    {
        if (apiObjectType == typeof(Task))
        {
            return null;
        }

        Type type = ExtractTaskParameter(apiObjectType);

        if (IsPrimitive(type))
        {
            return ProcessApiObjectPrimitive(type, propertyName);
        }

        if (IsEnum(type))
        {
            return ProcessApiObjectEnum(type, propertyName);
        }

        return ProcessApiObjectComplex(type, propertyName);
    }

    private ApiObjectPrimitive ProcessApiObjectPrimitive(Type type, string name)
    {
        return new ApiObjectPrimitive(name, type.Name);
    }

    private ApiObjectEnum ProcessApiObjectEnum(Type type, string name)
    {
        List<string> values = type
            .GetFields()
            .Where(x => !x.IsSpecialName)
            .Select(x => x.Name)
            .ToList();

        return new ApiObjectEnum(name, type.Name, values);
    }

    private ApiObjectComplex ProcessApiObjectComplex(Type type, string name)
    {
        string typeName = GetParameterTypeName(type);
        type = ExtractListParameter(type);
        List<ApiObject> fields = type
            .GetMembers(BindingFlags.Instance | BindingFlags.Public)
            .Where(x => x.MemberType == MemberTypes.Property)
            .Select(x =>
            {
                PropertyInfo p = (PropertyInfo)x;
                return ProcessApiObject(p.PropertyType, p.Name);
            })
            .Where(x => x is not null)
            .ToList()!;

        return new ApiObjectComplex(name, typeName, fields);
    }

    private Type ExtractTaskParameter(Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>))
        {
            return type.GenericTypeArguments[0];
        }

        return type;
    }

    private Type ExtractListParameter(Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
        {
            return type.GenericTypeArguments[0];
        }

        return type;
    }

    private string GetParameterTypeName(Type type)
    {
        if (type.IsGenericType)
        {
            string parameterTypeName = GetParameterTypeName(type.GenericTypeArguments[0]);
            string[] parts = type.Name.Split('`');
            return $"{parts[0]}<{parameterTypeName}>";
        }

        return type.Name;
    }

    private bool IsPrimitive(Type type)
    {
        return type.IsPrimitive || type == typeof(decimal) || type == typeof(string);
    }

    private bool IsEnum(Type type)
    {
        return type.IsEnum;
    }

    private readonly ApiDocGenOptions _options;
}