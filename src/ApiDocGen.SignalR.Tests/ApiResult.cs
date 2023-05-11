namespace ApiDocGen.SignalR.Tests;

public record ApiResult(
    ApiResultStatus Status,
    string? ErrorMessage)
{
    public static ApiResult Success()
    {
        return new ApiResult(ApiResultStatus.Ok, null);
    }

    public static ApiResult Error(string message)
    {
        return new ApiResult(ApiResultStatus.Error, message);
    }
}

public record ApiResult<T>(
    ApiResultStatus Status,
    T? Data,
    string? ErrorMessage)
{
    public static ApiResult<T> Success(T data)
    {
        return new ApiResult<T>(ApiResultStatus.Ok, data, null);
    }

    public static ApiResult<T> Error(string message)
    {
        return new ApiResult<T>(ApiResultStatus.Error, default, message);
    }
}