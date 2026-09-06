# 04 - Delegates & Lambdas in C# vs TypeScript Arrow Functions

## Overview
In JavaScript and TypeScript, functions are first-class objects. You can pass arrow functions `() => void` or `(x: T) => R` directly into functions, with type signatures defined via callable types `(arg: T) => R`.

In C#, a method cannot be passed directly as a raw pointer without a strongly-typed delegate wrapper. C# solves this using **Delegates**—type-safe, object-oriented function pointers. The .NET BCL provides standard delegates: `Action` (for methods returning `void`) and `Func` (for methods returning a value).

---

## Main Concept

### Standard Delegate Types in C#

1. **`Action<T1, T2, ...>`**: Represents a delegate that takes 0 to 16 parameters and returns `void`.
2. **`Func<T1, T2, ..., TResult>`**: Represents a delegate that takes 0 to 16 parameters and returns a value of type `TResult` (the last generic parameter is always the return type).
3. **`Predicate<T>`**: Special delegate taking `T` and returning `bool` (equivalent to `Func<T, bool>`).

### TypeScript vs C# Comparison

```typescript
// TypeScript: Arrow functions & callable types
type Logger = (message: string) => void;
type Transformer = (value: number) => string;

const logInfo: Logger = (msg) => console.log(`[INFO] ${msg}`);
const doubleToString: Transformer = (n) => `Result: ${n * 2}`;

function retryUntil(condition: () => boolean): void {
  while (!condition()) {
    // wait
  }
}
```

```csharp
// C#: Action and Func delegates with lambdas
Action<string> logInfo = msg => Console.WriteLine($"[INFO] {msg}");
Func<int, string> doubleToString = n => $"Result: {n * 2}";

// Custom polling helper used in automated test frameworks
public static void RetryUntil(Func<bool> condition, int maxAttempts = 5)
{
    int attempts = 0;
    while (!condition() && attempts++ < maxAttempts)
    {
        Thread.Sleep(100);
    }
}
```

---

## Common Mistakes

1. **Confusing `Action` with `Func`**:
   Trying to assign a method that returns a value to an `Action` or vice versa will cause a compile error. Remember: `Action` is for `void`; `Func` always returns a value (the last type parameter is the return type).

2. **Accidental Variable Capture in Loops (Closures)**:
   Capturing a loop index variable in a lambda can lead to unexpected behavior if execution is deferred, as all lambdas might reference the final value of the loop variable. Always create a local copy inside the loop if necessary.

3. **Unhandled Exceptions in Multicast Delegates**:
   When combining delegates using `+=` (multicast), if the first delegate throws an exception, subsequent delegates in the invocation chain will not execute.

---

## Summary
- In C#, delegates are strongly-typed, memory-managed function pointers.
- `Action<T>` replaces void callbacks, while `Func<T, TResult>` replaces value-returning callbacks.
- C# lambdas (`=>`) provide the same concise syntax as JavaScript arrow functions, with full static type enforcement.

**Key Takeaway:** Master `Action` and `Func` to build clean retry wrappers, fluent assertion filters, and event listeners across your Playwright .NET framework.
