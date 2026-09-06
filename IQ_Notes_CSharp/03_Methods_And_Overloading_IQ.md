# 03 - Methods & Method Overloading in C# vs TypeScript

## Overview
In TypeScript and JavaScript, functions and methods can only have **one** single runtime implementation body. To achieve "overloading" in TypeScript, you must declare multiple ambient type signatures followed by a single function containing runtime `typeof` or `instanceof` checks.

In C#, **Method Overloading** is a native, first-class feature of the Common Language Runtime (CLR). A class can have multiple methods with the exact same name, provided their parameter types, arity, or modifiers differ. The C# compiler selects the precise method overload at compile time without any runtime branching.

---

## Main Concept

### TypeScript "Overload" (Single Implementation) vs C# True Overloading

```typescript
// TypeScript: Multiple signatures, single implementation with manual runtime checks
class Navigator {
  navigateTo(url: string): void;
  navigateTo(url: string, timeoutMs: number): void;
  navigateTo(url: string, timeoutMs?: number): void {
    const timeout = timeoutMs ?? 30000;
    console.log(`Navigating to ${url} with timeout ${timeout}ms`);
  }
}
```

```csharp
// C#: True method overloads compiled as separate binary methods
public class Navigator
{
    // Overload 1: Base navigation
    public void NavigateTo(string url)
    {
        NavigateTo(url, 30_000); // Chains into overload 2
    }

    // Overload 2: Navigation with custom timeout
    public void NavigateTo(string url, int timeoutMs)
    {
        Console.WriteLine($"Navigating to {url} with timeout {timeoutMs}ms");
    }

    // Overload 3: Navigation with timeout and retry flag
    public void NavigateTo(string url, int timeoutMs, bool retryOnFailure)
    {
        Console.WriteLine($"Navigating to {url} [Timeout: {timeoutMs}ms, Retry: {retryOnFailure}]");
    }
}
```

### Named Arguments in C#
C# allows callers to pass arguments by name, improving test readability:
```csharp
navigator.NavigateTo(url: "https://example.com", timeoutMs: 5000, retryOnFailure: true);
```

---

## Common Mistakes

1. **Attempting Overloads Differing Only by Return Type**:
   In C#, methods cannot be overloaded if they differ *only* by return type (e.g., `int GetData()` and `string GetData()` cannot coexist). Overloads must differ in their parameter signatures.

2. **Ambiguous Optional Parameters**:
   Combining multiple optional parameters with overloads can lead to compiler error `CS0121: The call is ambiguous`. Prefer explicit overloads or named arguments to resolve ambiguity.

3. **Duplicating Code Across Overloads**:
   Avoid copying implementation logic across overloads. Use constructor or method chaining (have simpler overloads invoke the canonical, most comprehensive overload with default values).

---

## Summary
- TypeScript simulates overloading through ambient declarations over a single runtime implementation.
- C# provides true compile-time method overloading with distinct binary entry points.
- C# supports named arguments (`paramName: value`) and expression-bodied methods (`=>`) for clean, concise test automation APIs.

**Key Takeaway:** Build robust Page Object and utility APIs by creating distinct method overloads that chain into a canonical implementation, avoiding tedious runtime `typeof` branching.
