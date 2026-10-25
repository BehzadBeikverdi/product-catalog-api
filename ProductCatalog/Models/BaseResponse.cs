namespace ProductCatalog.Models.Responses;

public class BaseResponse<T>
{
    public bool Success { get; init; }

    public string Message { get; init; } = string.Empty;

    public T? Payload { get; init; }

    public IReadOnlyList<object>? Errors { get; init; }

    public DateTime Timestamp { get; init; } = DateTime.UtcNow;

    public static BaseResponse<T> SuccessResponse(
        string message,
        T? payload)
    {
        return new BaseResponse<T>
        {
            Success = true,
            Message = message,
            Payload = payload,
            Errors = null
        };
    }

    public static BaseResponse<T> ErrorResponse(
        string message,
        IReadOnlyList<object>? errors = null)
    {
        return new BaseResponse<T>
        {
            Success = false,
            Message = message,
            Payload = default,
            Errors = errors
        };
    }
}