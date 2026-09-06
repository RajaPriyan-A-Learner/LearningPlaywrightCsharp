# 12 - Generic API Envelope Pattern: Type-Safe HTTP Assertions in C# vs TypeScript

## Overview
In web and microservice automated testing, tests must constantly interact with HTTP APIs. In loosely typed JavaScript or unconstrained TypeScript, API responses are often treated as `any` or cast unsafely (`response.data as UserProfile`). When endpoints return error payloads (e.g. 404 Not Found or 500 Internal Error), accessing fields on `data` leads to unhandled runtime crashes.

The **Generic API Envelope Pattern** (first introduced in Chapter 27) wraps the HTTP status code, success state, error logs, and typed payload `<T>` into a single container. In C#, this pattern is implemented using a generic class with static factory methods and `System.Text.Json` deserialization.

---

## Main Concept

### TypeScript vs C# Envelope Comparison

```typescript
// TypeScript (Chapter 27)
interface ApiResponse<T> {
  statusCode: number;
  isSuccess: boolean;
  data?: T;
  errorMessage?: string;
}
```

```csharp
// C# Generic API Envelope
public class ApiResponse<T>
{
    public int StatusCode { get; }
    public bool IsSuccess => StatusCode is >= 200 and < 300;
    public T? Data { get; }
    public string? ErrorMessage { get; }

    private ApiResponse(int statusCode, T? data, string? errorMessage)
    {
        StatusCode = statusCode;
        Data = data;
        ErrorMessage = errorMessage;
    }

    // Static factory methods enforce valid envelope state
    public static ApiResponse<T> Success(int statusCode, T data) 
        => new(statusCode, data, null);

    public static ApiResponse<T> Failure(int statusCode, string error) 
        => new(statusCode, default, error);
}
```

### Type-Safe Consumption in Test Assertions

```csharp
public async Task Test_GetUserProfile_ReturnsExpectedUser()
{
    ApiResponse<UserProfileDto> response = await apiClient.GetAsync<UserProfileDto>("/users/42");

    // 1. Assert HTTP level
    Assert.True(response.IsSuccess, $"API failed with: {response.ErrorMessage}");

    // 2. Strongly typed access with null safety
    Assert.NotNull(response.Data);
    Assert.Equal("Alex Mercer", response.Data.FullName);
}
```

---

## Common Mistakes

1. **Attempting to Deserialize `Data` on Non-2xx Responses**:
   When an API returns 400 Bad Request or 500 Internal Error, the JSON payload shape usually matches an error schema (`ProblemDetails`), not `T`. Attempting to unconditionally deserialize into `T` throws a deserialization exception. Check `IsSuccess` before parsing `Data`.

2. **Allowing `Data` to be Non-Nullable**:
   In C#, marking `public T Data { get; }` implies it will never be null. However, on failed requests or 204 No Content, `Data` is `null`. Always declare it as `T? Data`.

3. **Exposing Public Modifiable Setters on the Envelope**:
   Envelope instances represent an immutable snapshot of an HTTP response. All properties should be get-only (`{ get; }`) with instantiation routed through private constructors and static factories.

---

## Summary
- The Generic API Envelope encapsulates HTTP metadata and strongly-typed payloads `<T>` into a unified model.
- Static factory methods (`Success` / `Failure`) ensure consistent envelope state.
- Combines seamlessly with `System.Text.Json` to provide compile-time verified API testing without raw JSON inspection or `any` casts.

**Key Takeaway:** Wrap all API test client calls in `ApiResponse<T>` to produce resilient, self-documenting, and type-safe API automation suites.
