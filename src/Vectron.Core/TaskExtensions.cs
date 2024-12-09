using Refit;

namespace Vectron.Core;

public static class TaskExtensions
{
    public static async Task<Result<T>> ToResult<T>(this Task<ApiResponse<T>> task)
        where T : class
    {
        try
        {
            var result = await task;
            return (Result<T>)result;
        }
        catch (Exception e)
        {
            return Result.Failure<T>(new Error("Exception", e.Message));
        }
    }
}