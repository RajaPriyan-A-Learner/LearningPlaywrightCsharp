# 06 - Classes, Properties & Constructors in C# vs TypeScript

## Overview
In TypeScript and JavaScript, classes use public fields by default or explicit `get`/`set` function declarations. Adding encapsulation often involves manual backing fields (e.g., `private _name: string`).

In C#, **Properties** are first-class language constructs that look like fields to callers while internally compiling to getter and setter methods (`get_Property()` / `set_Property()`). C# features auto-implemented properties `{ get; set; }`, `init`-only properties (immutable after object initialization), and constructor chaining using `: this(...)`.

---

## Main Concept

### TypeScript Class vs C# Class Properties & Constructor Chaining

```typescript
// TypeScript: Manual backing field or explicit accessor methods
class PlaywrightConfig {
  private _baseUrl: string;
  public timeoutMs: number;

  constructor(baseUrl: string, timeoutMs: number = 30000) {
    this._baseUrl = baseUrl;
    this.timeoutMs = timeoutMs;
  }

  get baseUrl(): string {
    return this._baseUrl;
  }
}
```

```csharp
// C#: Auto-properties, init-only properties, and constructor chaining
public class PlaywrightConfig
{
    // Immutable after construction
    public string BaseUrl { get; }
    
    // Settable only during object creation: new PlaywrightConfig { TimeoutMs = 5000 }
    public int TimeoutMs { get; init; } = 30_000;
    
    // Encapsulated state: readable anywhere, modifiable only by this class
    public bool IsHeadless { get; private set; } = true;

    // Primary constructor
    public PlaywrightConfig(string baseUrl, int timeoutMs)
    {
        BaseUrl = baseUrl;
        TimeoutMs = timeoutMs;
    }

    // Constructor chaining via ': this(...)'
    public PlaywrightConfig(string baseUrl) : this(baseUrl, 30_000)
    {
    }

    // Fluent builder pattern
    public PlaywrightConfig WithHeadless(bool headless)
    {
        IsHeadless = headless;
        return this; // Fluent method chaining
    }
}
```

---

## Common Mistakes

1. **Exposing Public Fields Instead of Properties**:
   In C#, never expose raw public variables (e.g., `public string BaseUrl;`). Always use properties (`public string BaseUrl { get; set; }`). Properties allow validation, logging, data-binding, and interface implementation without breaking binary compatibility.

2. **Accidentally Leaving Setters Public**:
   Making setters public (`{ get; set; }`) allows external test code to inadvertently mutate page or configuration state midway through a test. Use `{ get; private set; }` or `{ get; init; }` to protect state integrity.

3. **Duplicating Initialization Across Multiple Constructors**:
   Writing independent assignment logic in multiple constructor overloads leads to bugs when initialization rules change. Always chain constructors using `: this(...)`.

---

## Summary
- C# Properties provide idiomatic encapsulation with zero boilerplate through auto-implemented properties.
- `init` accessors allow immutability while supporting clean object-initializer syntax.
- Constructor chaining via `: this(...)` centralizes object creation logic.

**Key Takeaway:** Use auto-properties with private setters or `init` accessors to build robust, immutable configuration and state models for your Playwright test framework.
