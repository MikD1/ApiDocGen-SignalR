namespace ApiDocGen.SignalR.Tests;

public interface ITestApiHubClient
{
    Task ClientMethod1();
    
    Task ClientMethod2(string strArg);
}