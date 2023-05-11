using System.Text;
using ApiDocGen.SignalR.ApiModel;

namespace ApiDocGen.SignalR;

public class ApiDocMarkdownRenderer
{
    public string Render(ApiInfo apiInfo)
    {
        StringBuilder builder = new();
        builder.AppendLine($"# {apiInfo.Name}");
        builder.AppendLine();

        foreach (ApiHub hub in apiInfo.Hubs)
        {
            RenderHub(builder, hub);
        }

        return builder.ToString();
    }

    private void RenderHub(StringBuilder builder, ApiHub hub)
    {
        builder.AppendLine($"## {hub.Route}");
        builder.AppendLine();
        foreach (ApiMethod method in hub.Methods)
        {
            RenderMethod(builder, method);
        }

        builder.AppendLine($"## {hub.Route} - client methods");
        builder.AppendLine();
        foreach (ApiMethod method in hub.ClientMethods)
        {
            RenderMethod(builder, method);
        }
    }

    private void RenderMethod(StringBuilder builder, ApiMethod method)
    {
        builder.AppendLine(@"<details style=""border: 0.1px solid; padding: 5px 15px; margin: 15px 0"">");
        builder.AppendLine($@"<summary style=""font-size: 16px;"">{method.Name}</summary>");

        if (method.Parameters.Any())
        {
            builder.AppendLine("Parameters:");
            foreach (ApiObject parameter in method.Parameters)
            {
                RenderObject(builder, parameter, 0);
            }
        }

        if (method.Response is not null)
        {
            builder.AppendLine("Response:");
            RenderObject(builder, method.Response, 0);
        }

        builder.AppendLine("</details>");
        builder.AppendLine();
    }

    private void RenderObject(StringBuilder builder, ApiObject apiObject, int indent)
    {
        if (indent == 0)
        {
            builder.AppendLine("<code><pre>");
        }

        switch (apiObject.ObjectType)
        {
            case ApiObjectType.Primitive:
                RenderObjectPrimitive(builder, (ApiObjectPrimitive)apiObject, indent);
                break;
            case ApiObjectType.Enum:
                RenderObjectEnum(builder, (ApiObjectEnum)apiObject, indent);
                break;
            case ApiObjectType.Complex:
                RenderObjectComplex(builder, (ApiObjectComplex)apiObject, indent);
                break;
            default:
                throw new NotSupportedException("Not supported API object field type.");
        }

        if (indent == 0)
        {
            builder.AppendLine("</pre></code>");
        }
    }

    private void RenderObjectPrimitive(StringBuilder builder, ApiObjectPrimitive apiObject, int indent)
    {
        string indentString = MakeIndentString(indent);
        string typeName = EscapeCharacters(apiObject.TypeName);
        builder.AppendLine(string.IsNullOrEmpty(apiObject.Name)
            ? $"{indentString}{typeName}"
            : $"{indentString}{apiObject.Name}: {typeName}");
    }

    private void RenderObjectEnum(StringBuilder builder, ApiObjectEnum apiObject, int indent)
    {
        string indentString = MakeIndentString(indent);
        string typeName = EscapeCharacters(apiObject.TypeName);
        string values = string.Join(", ", apiObject.Values);
        builder.AppendLine(string.IsNullOrEmpty(apiObject.Name)
            ? $"{indentString}{typeName} [{values}]"
            : $"{indentString}{apiObject.Name}: {typeName} [{values}]");
    }

    private void RenderObjectComplex(StringBuilder builder, ApiObjectComplex apiObject, int indent)
    {
        string indentString = MakeIndentString(indent);
        string typeName = EscapeCharacters(apiObject.TypeName);

        builder.AppendLine(string.IsNullOrEmpty(apiObject.Name)
            ? $"{indentString}{typeName}"
            : $"{indentString}{apiObject.Name}: {typeName}");

        foreach (ApiObject field in apiObject.Fields)
        {
            RenderObject(builder, field, indent + 1);
        }
    }

    private string EscapeCharacters(string line)
    {
        return line
            .Replace("<", "&lt;")
            .Replace(">", "&gt;");
    }

    private string MakeIndentString(int indent)
    {
        int indentCharactersNumber = (indent + 1) * 2;
        return new string(' ', indentCharactersNumber);
    }
}