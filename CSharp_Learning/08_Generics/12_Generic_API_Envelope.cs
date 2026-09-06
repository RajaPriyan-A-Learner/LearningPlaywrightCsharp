// ============================================================================
// C# Learning Framework - Chapter 08: Generic API Envelope Pattern
// Equivalent to TypeScript: Chapter 27 - Generic API Response Wrapping
// ============================================================================

using System.Text.Json;

namespace CSharpLearning.Generics;

/// <summary>
/// Domain model for an API user profile.
/// </summary>
public record UserProfileDto(int Id, string Email, string FullName, bool IsActive);

/// <summary>
/// Domain model for an Auth Token response.
/// </summary>
public record AuthTokenDto(string AccessToken, string TokenType, int ExpiresInSeconds);

/// <summary>
/// Universal Generic API Response Envelope.
/// Wraps any API payload <T> alongside HTTP metadata, status code, success state, and error logs.
/// In TypeScript:
///   interface ApiResponse<T> {
///     statusCode: number;
///     isSuccess: boolean;
///     data?: T;
///     errorMessage?: string;
///   }
/// </summary>
/// <typeparam name="T">Type of payload returned on success.</typeparam>
public class ApiResponse<T>
{
    public int StatusCode { get; }
    public bool IsSuccess => StatusCode is >= 200 and < 300;
    public T? Data { get; }
    public string? ErrorMessage { get; }
    public Dictionary<string, string> Headers { get; }

    private ApiResponse(int statusCode, T? data, string? errorMessage, Dictionary<string, string>? headers = null)
    {
        StatusCode = statusCode;
        Data = data;
        ErrorMessage = errorMessage;
        Headers = headers ?? new Dictionary<string, string>();
    }

    /// <summary>
    /// Static factory for successful API responses.
    /// </summary>
    public static ApiResponse<T> Success(int statusCode, T data, Dictionary<string, string>? headers = null)
    {
        return new ApiResponse<T>(statusCode, data, null, headers);
    }

    /// <summary>
    /// Static factory for failed API responses.
    /// </summary>
    public static ApiResponse<T> Failure(int statusCode, string errorMessage, Dictionary<string, string>? headers = null)
    {
        return new ApiResponse<T>(statusCode, default, errorMessage, headers);
    }

    public override string ToString()
    {
        return IsSuccess
            ? $"[HTTP {StatusCode} OK] Payload Type: {typeof(T).Name}"
            : $"[HTTP {StatusCode} ERROR] Message: {ErrorMessage}";
    }
}

/// <summary>
/// Simulated generic API client illustrating how C# automated test frameworks
/// deserialize strongly-typed JSON payloads using System.Text.Json into generic envelopes.
/// </summary>
public class GenericApiClient
{
    private readonly string _baseUrl;

    public GenericApiClient(string baseUrl)
    {
        _baseUrl = baseUrl;
    }

    /// <summary>
    /// Generic request simulation that deserializes response JSON into <T>.
    /// </summary>
    public async Task<ApiResponse<T>> RequestAsync<T>(string endpoint, string mockJsonPayload, int simulatedStatus)
    {
        Console.WriteLine($"[ApiClient] Sending GET -> {_baseUrl}{endpoint}");
        await Task.Delay(50); // Simulate network I/O

        if (simulatedStatus is >= 200 and < 300)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var data = JsonSerializer.Deserialize<T>(mockJsonPayload, options);
            return ApiResponse<T>.Success(simulatedStatus, data!);
        }

        return ApiResponse<T>.Failure(simulatedStatus, $"Request to {endpoint} returned status {simulatedStatus}");
    }
}

public class Program
{
    public static async Task Main()
    {
        Console.WriteLine("=== C# Generic API Envelope Pattern Demonstration ===");

        var client = new GenericApiClient("https://api.automation.test/v1");

        // 1. Successful request returning UserProfileDto
        string userJson = """
        {
            "id": 42,
            "email": "tester@framework.com",
            "fullName": "Alex Mercer",
            "isActive": true
        }
        """;

        var userResponse = await client.RequestAsync<UserProfileDto>("/users/42", userJson, 200);
        Console.WriteLine($"Result: {userResponse}");
        if (userResponse.IsSuccess && userResponse.Data is not null)
        {
            Console.WriteLine($" -> User ID: {userResponse.Data.Id}");
            Console.WriteLine($" -> Name: {userResponse.Data.FullName}");
            Console.WriteLine($" -> Active: {userResponse.Data.IsActive}");
        }

        // 2. Successful request returning AuthTokenDto
        string tokenJson = """
        {
            "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
            "tokenType": "Bearer",
            "expiresInSeconds": 3600
        }
        """;

        var tokenResponse = await client.RequestAsync<AuthTokenDto>("/auth/token", tokenJson, 200);
        Console.WriteLine($"\nResult: {tokenResponse}");
        if (tokenResponse.IsSuccess && tokenResponse.Data is not null)
        {
            Console.WriteLine($" -> Token Type: {tokenResponse.Data.TokenType}");
            Console.WriteLine($" -> Expires In: {tokenResponse.Data.ExpiresInSeconds}s");
        }

        // 3. Failed request returning error envelope
        var errorResponse = await client.RequestAsync<UserProfileDto>("/users/999", "{}", 404);
        Console.WriteLine($"\nResult: {errorResponse}");
        Console.WriteLine($" -> IsSuccess: {errorResponse.IsSuccess}");
        Console.WriteLine($" -> ErrorMessage: {errorResponse.ErrorMessage}");
    }
}
