# 01 - Primitives & Nullability in C# vs TypeScript/JavaScript

## Overview
In JavaScript and TypeScript, all numeric values are represented by a single IEEE 754 64-bit float (`number`) or `bigint`. Furthermore, absence of value is split between two separate primitives: `null` (intentional absence) and `undefined` (uninitialized or missing).

In C# (.NET), numeric types are explicitly sized for memory efficiency and mathematical precision (`int`, `long`, `double`, `decimal`). Absence of value is unified under `null`; there is **no `undefined`** in C#. Since C# 8, Nullable Reference Types (`T?`) bring compile-time null safety directly to reference types.

---

## Main Concept

### Numeric Precision: Why Playwright Automators Must Care
- In financial or e-commerce test validations (e.g. verifying shopping cart totals), JavaScript `number` can suffer floating point drift (e.g., `0.1 + 0.2 !== 0.3`).
- In C#, `decimal` has 128-bit precision with 28-29 significant digits and exact decimal representation, eliminating floating-point drift in monetary assertions.

### TypeScript vs C# Comparison

```typescript
// TypeScript: Single numeric type & null/undefined duality
let count: number = 42;
let price: number = 19.99;
let missingValue: string | undefined = undefined;
let nullValue: string | null = null;

// Safe navigation & null coalescing
let length = missingValue?.length ?? 0;
```

```csharp
// C#: Explicit numeric sizing & unified nullability
int count = 42;                // 32-bit signed integer
long largeId = 9_000_000_000L; // 64-bit signed integer
double speed = 12.34;          // 64-bit floating point
decimal price = 19.99m;        // 128-bit exact decimal (ideal for finance/assertions)

// Nullable reference type (C# 8+)
string? optionalHeader = null; // Valid, explicitly marked nullable
// string nonNullHeader = null; // Compiler warning: CS8600

// Safe navigation (?.) and Null Coalescing (??)
int length = optionalHeader?.Length ?? 0;
```

---

## Common Mistakes

1. **Assuming `undefined` exists in C#**:
   Attempting to check `if (val == undefined)` will result in a compile-time error. In C#, fields and variables default to `default` (`0` for value types, `null` for reference types).

2. **Using `double` for currency or exact assertions**:
   Using `double` or `float` for assertions like shopping cart totals can produce floating point rounding errors (`0.30000000000000004`). Always use `decimal` with the `m` suffix for monetary and precise values.

3. **Ignoring Nullable Reference Type warnings**:
   Treating `string?` as `string` without checking for null can throw `NullReferenceException` at runtime. Always use pattern matching (`if (val is not null)`), `?.`, or `??`.

---

## Summary
- C# provides explicit, memory-efficient numeric primitives (`int`, `long`, `double`, `decimal`) compared to JavaScript's single `number`.
- `decimal` prevents floating point assertion drift in automated UI and API tests.
- C# has no `undefined`; value absence is handled exclusively via `null` and Nullable Reference Types (`T?`).

**Key Takeaway:** Choose the right primitive for your automation payloads (`int` for IDs, `decimal` for prices), and leverage `T?` with `?.` and `??` to eliminate `NullReferenceException` in your test suite.
