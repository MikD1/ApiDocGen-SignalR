using ApiDocGen.SignalR.ApiModel;

namespace ApiDocGen.SignalR.Tests;

[TestClass]
public class ApiDocGeneratorTest
{
    [TestMethod]
    public void ProcessApiMethodParametersIsCorrect()
    {
        ApiDocGenOptions options = new("Some SignalR API", new[]
        {
            new ApiDocGenHubInfo(typeof(ApiDocGen.SignalR.Tests.TestApiHub), "hubs/api")
        });

        ApiDocGenerator generator = new(options);
        ApiInfo apiInfo = generator.Generate();

        ApiMethod method2 = apiInfo.Hubs[0].Methods.First(x => x.Name == "Method2");
        Assert.AreEqual(1, method2.Parameters.Count);

        ApiObjectComplex parameter = (ApiObjectComplex)method2.Parameters[0];
        Assert.AreEqual(2, parameter.Fields.Count);

        ApiObjectPrimitive field1 = (ApiObjectPrimitive)parameter.Fields[0];
        Assert.AreEqual("Field1", field1.Name);
        Assert.AreEqual("String", field1.TypeName);
        Assert.AreEqual(ApiObjectType.Primitive, field1.ObjectType);

        ApiObjectPrimitive field2 = (ApiObjectPrimitive)parameter.Fields[1];
        Assert.AreEqual("Field2", field2.Name);
        Assert.AreEqual("Int32", field2.TypeName);
        Assert.AreEqual(ApiObjectType.Primitive, field2.ObjectType);
    }

    [TestMethod]
    public void ProcessApiMethodResponseIsCorrect()
    {
        ApiDocGenOptions options = new("Some SignalR API", new[]
        {
            new ApiDocGenHubInfo(typeof(ApiDocGen.SignalR.Tests.TestApiHub), "hubs/api")
        });
    
        ApiDocGenerator generator = new(options);
        ApiInfo apiInfo = generator.Generate();
        ApiMethod method1 = apiInfo.Hubs[0].Methods.First(x => x.Name == "Method1");
        ApiObjectComplex response = (ApiObjectComplex)method1.Response!;
    
        Assert.AreEqual(string.Empty, response.Name);
        Assert.AreEqual("ApiResult<TestApiObject>", response.TypeName);
        Assert.AreEqual(3, response.Fields.Count);
    
        ApiObjectEnum statusField = (ApiObjectEnum)response.Fields[0];
        Assert.AreEqual("Status", statusField.Name);
        Assert.AreEqual("ApiResultStatus", statusField.TypeName);
        Assert.AreEqual(ApiObjectType.Enum, statusField.ObjectType);
        Assert.AreEqual(2, statusField.Values.Count);
        Assert.AreEqual("Ok", statusField.Values[0]);
        Assert.AreEqual("Error", statusField.Values[1]);
    
        ApiObjectComplex dataField = (ApiObjectComplex)response.Fields[1];
        Assert.AreEqual("Data", dataField.Name);
        Assert.AreEqual("TestApiObject", dataField.TypeName);
        Assert.AreEqual(ApiObjectType.Complex, dataField.ObjectType);
        Assert.AreEqual(2, dataField.Fields.Count);
        Assert.AreEqual("Field1", dataField.Fields[0].Name);
        Assert.AreEqual("String", dataField.Fields[0].TypeName);
        Assert.AreEqual(ApiObjectType.Primitive, dataField.Fields[0].ObjectType);
        Assert.AreEqual("Field2", dataField.Fields[1].Name);
        Assert.AreEqual("Int32", dataField.Fields[1].TypeName);
        Assert.AreEqual(ApiObjectType.Primitive, dataField.Fields[1].ObjectType);

        ApiObjectPrimitive errorMessageField = (ApiObjectPrimitive)response.Fields[2];
        Assert.AreEqual("ErrorMessage", errorMessageField.Name);
        Assert.AreEqual("String", errorMessageField.TypeName);
        Assert.AreEqual(ApiObjectType.Primitive, errorMessageField.ObjectType);
    }
    
    [TestMethod]
    public void ProcessApiMethodObjectsWithListIsCorrect()
    {
        ApiDocGenOptions options = new("Some SignalR API", new[]
        {
            new ApiDocGenHubInfo(typeof(ApiDocGen.SignalR.Tests.TestApiHub), "hubs/api")
        });
    
        ApiDocGenerator generator = new(options);
        ApiInfo apiInfo = generator.Generate();
        ApiMethod method3 = apiInfo.Hubs[0].Methods.First(x => x.Name == "Method3");
        ApiObjectComplex response = (ApiObjectComplex)method3.Response!;
        ApiObjectComplex dataField = (ApiObjectComplex)response.Fields[1];
    
        Assert.AreEqual("Data", dataField.Name);
        Assert.AreEqual("List<String>", dataField.TypeName);
        Assert.AreEqual(ApiObjectType.Complex, dataField.ObjectType);
        Assert.AreEqual(2, dataField.Fields.Count);
    }

    [TestMethod]
    public void FindClientApiMethods()
    {
        ApiDocGenOptions options = new("Some SignalR API", new[]
        {
            new ApiDocGenHubInfo(typeof(ApiDocGen.SignalR.Tests.TestApiHub), "hubs/api")
        });

        ApiDocGenerator generator = new(options);
        ApiInfo apiInfo = generator.Generate();

        ApiMethod clientMethod1 = apiInfo.Hubs[0].ClientMethods.First(x => x.Name == "ClientMethod1");
        Assert.AreEqual(0, clientMethod1.Parameters.Count);
        Assert.IsNull(clientMethod1.Response);


        ApiMethod clientMethod2 = apiInfo.Hubs[0].ClientMethods.First(x => x.Name == "ClientMethod2");
        Assert.AreEqual(1, clientMethod2.Parameters.Count);
        Assert.IsNull(clientMethod2.Response);

        ApiObjectPrimitive clientMethod2Parameter = (ApiObjectPrimitive)clientMethod2.Parameters[0];
        Assert.AreEqual(string.Empty, clientMethod2Parameter.Name);
        Assert.AreEqual("String", clientMethod2Parameter.TypeName);
        Assert.AreEqual(ApiObjectType.Primitive, clientMethod2Parameter.ObjectType);
    }
}