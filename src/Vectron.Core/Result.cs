using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Refit;

namespace Vectron.Core;

public record Result()
{
    protected Result(bool isSuccess, Error error) : this()
    {
        if (isSuccess && error != Error.None) throw new InvalidOperationException();
        if (!isSuccess && error == Error.None) throw new InvalidOperationException();

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; init; }
    public bool IsFailure => !IsSuccess;
    public Error? Error { get; init; }

    public static Result Success() => new(true, Error.None);

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

    public static Result<TValue> Create<TValue>(TValue? value, Error error) =>
        value is null ? Failure<TValue>(error) : Success(value);

    public static Result Failure(Error error) => new(false, error);
    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

    public static Result FirstFailureOrSuccess(params Result[] results)
    {
        foreach (var result in results)
        {
            if (result.IsFailure) return result;
        }

        return Success();
    }
}

[JsonConverter(typeof(ResultJsonConverter))]
public record Result<TValue> : Result
{
    public TValue? Value
    {
        get
        {
            if (!IsSuccess) throw new InvalidOperationException();
            return field;
        }
        init;
    }

    public Result()
    {
    }

    protected internal Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        Value = value;
    }

    public static implicit operator Result<TValue>(TValue value) => Success(value);

    public static explicit operator Result<TValue>(ApiResponse<TValue> response)
    {
        if (!response.IsSuccessful)
            return Result.Failure<TValue>(response.Error.StatusCode switch
            {
                HttpStatusCode.NotFound => Errors.Common.NotFound,
                HttpStatusCode.BadRequest when response.Error!.Content!.Contains(Errors.Common.BadJson.Code) => Errors.Common.BadJson,
                HttpStatusCode.BadRequest when response.Error.Content.Contains(Errors.Common.NotJson.Code) => Errors.Common.NotJson,
                _ => JsonSerializer.Deserialize<Error>(response.Error.Content!) ?? Errors.Common.Unknown,
            });
        return response.Content;
    }
}