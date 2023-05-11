using Microsoft.AspNetCore.SignalR;

namespace ApiDocGen.SignalR.Tests;

public class TestApiHub : Hub<ITestApiHubClient>
{
    public Task<ApiResult<TestApiObject>> Method1()
    {
        throw new NotImplementedException();
    }

    public Task<ApiResult<int>> Method2(TestApiObject data)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResult<List<string>>> Method3()
    {
        throw new NotImplementedException();
    }
}