# 05 - Asynchronous Programming: Task vs Promise & ThreadPool vs Event Loop

## Overview
Asynchronous execution is central to modern web test automation with Playwright.
In JavaScript and Node.js, asynchronous tasks are represented by `Promise<T>` and orchestrated on a **single-threaded Event Loop** with a microtask queue.

In C# and .NET, asynchronous operations are represented by `Task` and `Task<T>`. Unlike Node.js, .NET executes asynchronous continuations on a **multi-threaded ThreadPool**. This means different parts of an `await` chain can resume on different operating system threads, providing true hardware parallelism alongside asynchronous non-blocking I/O.

---

## Main Concept

### The Async/Await Mapping Table

| Concept | JavaScript / TypeScript | C# (.NET) |
| :--- | :--- | :--- |
| Asynchronous Handle (Void) | `Promise<void>` | `Task` |
| Asynchronous Handle (Value) | `Promise<T>` | `Task<T>` |
| Await Keyword | `await promise` | `await task` |
| Parallel Execution (All) | `Promise.all([p1, p2])` | `await Task.WhenAll(t1, t2)` |
| Racing Execution (First) | `Promise.race([p1, p2])` | `await Task.WhenAny(t1, t2)` |
| Execution Runtime | Single-threaded Event Loop | Multi-threaded ThreadPool |
| Cancellation Mechanism | `AbortController` / `AbortSignal` | `CancellationTokenSource` / `CancellationToken` |

### Code Comparison

```typescript
// TypeScript: Promise.all on Event Loop
async function setupTestEnvironment(): Promise<string[]> {
  const p1 = fetchConfig();
  const p2 = launchBrowser();
  const results = await Promise.all([p1, p2]);
  return results;
}
```

```csharp
// C#: Task.WhenAll on ThreadPool
public async Task<string[]> SetupTestEnvironmentAsync(CancellationToken ct = default)
{
    Task<string> t1 = FetchConfigAsync(ct);
    Task<string> t2 = LaunchBrowserAsync(ct);
    
    // Await all tasks concurrently
    string[] results = await Task.WhenAll(t1, t2);
    return results;
}
```

---

## Common Mistakes

1. **Using `async void` (Except in Event Handlers)**:
   In C#, returning `void` from an `async` method causes any thrown exception to escape the caller's `try/catch` block and crash the process. Always return `Task` (or `Task<T>`) instead of `void`.

2. **Sync-over-Async Deadlocks (`.Result` or `.Wait()`)**:
   Calling `.Result` or `.Wait()` on an incomplete `Task` synchronously blocks the calling thread while waiting for the task to complete on another thread. In UI or synchronization contexts, this causes fatal deadlocks. Always use `await`.

3. **Ignoring `CancellationToken`**:
   In long-running test suites or cloud CI runners, ignoring cancellation tokens prevents tests from terminating gracefully when CI timeouts occur. Pass `CancellationToken` through your async call stack.

---

## Summary
- `Task` and `Task<T>` represent asynchronous operations in C#, directly mapping to `Promise<void>` and `Promise<T>`.
- .NET dispatches async continuations across a multi-threaded ThreadPool, offering true multicore parallelism.
- Never write `async void`; always return `Task` and use `await` instead of `.Result`.

**Key Takeaway:** In Playwright for .NET, every browser navigation and interaction returns a `Task`; always mark your Page Object methods with `async Task` and suffix method names with `Async`.
