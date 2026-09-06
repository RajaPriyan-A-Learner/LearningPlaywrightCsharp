# 09 - Interfaces & Indexers: Nominal Typing vs Structural Duck Typing

## Overview
In TypeScript, type checking is **structural** ("duck typing"). If an object possesses the properties and shapes required by an `interface`, TypeScript considers it valid without requiring the class to declare `implements InterfaceName`. TypeScript also allows dynamic property access via index signatures `[key: string]: any`.

In C#, type checking is strictly **nominal**. A class only implements an interface if it explicitly declares it in its inheritance list (`class Foo : IFoo`). To provide dictionary-like bracket access (`foo["key"]`), C# provides a specialized language feature called **Indexers** (`this[...]`).

---

## Main Concept

### Structural (TS) vs Nominal (C#) Interfaces

```typescript
// TypeScript: Structural Duck Typing
interface ITestConfig {
  baseUrl: string;
  timeoutMs?: number; // Optional property
  [key: string]: any;  // Index signature
}

// Any matching object literal passes without explicit declaration!
const cfg: ITestConfig = {
  baseUrl: "https://staging.test.io",
  customHeader: "Bearer 123"
};
```

```csharp
// C#: Nominal Typing & Explicit Indexers
public interface ITestConfig
{
    string BaseUrl { get; }
    int? TimeoutMs { get; } // Nullable replaces optional '?'
    string this[string key] { get; set; } // C# Indexer
}

// MUST explicitly declare ': ITestConfig'
public class TestConfigStore : ITestConfig
{
    private readonly Dictionary<string, string> _customSettings = new();

    public string BaseUrl { get; }
    public int? TimeoutMs { get; }

    public TestConfigStore(string baseUrl, int? timeoutMs = null)
    {
        BaseUrl = baseUrl;
        TimeoutMs = timeoutMs;
    }

    // C# Indexer implementation
    public string this[string key]
    {
        get => _customSettings.TryGetValue(key, out var val) ? val : string.Empty;
        set => _customSettings[key] = value;
    }
}
```

### Usage Comparison
```csharp
var config = new TestConfigStore("https://staging.test.io", 10_000);

// Using the indexer just like a dictionary or JS object
config["AuthToken"] = "token_xyz987";
Console.WriteLine(config["AuthToken"]); // Outputs: token_xyz987
```

---

## Common Mistakes

1. **Expecting Duck Typing in C#**:
   Creating a class with matching property names will not allow it to be passed into a method expecting `ITestConfig`. In C#, the class must explicitly list `: ITestConfig`.

2. **Crashing with `KeyNotFoundException` in Indexers**:
   If you implement an indexer getter using `_dict[key]` directly, querying an absent key throws `KeyNotFoundException`. Always use `_dict.TryGetValue(...)` and return a safe default or `null`.

3. **Confusing Optional Properties with Default Values**:
   In TypeScript, optional properties can be omitted. In C#, model optional interface members using nullable types (`int?`, `string?`).

---

## Summary
- C# requires nominal interface declaration (`class Foo : IFoo`); structural duck typing does not exist at runtime.
- Model TypeScript optional interface fields using C# nullable types (`T?`).
- Use C# Indexers (`this[TKey]`) to provide bracket syntax on custom data stores and configuration managers.

**Key Takeaway:** Implement nominal C# interfaces with indexers to create strongly-typed, dictionary-accessible configuration envelopes for your automation test suites.
