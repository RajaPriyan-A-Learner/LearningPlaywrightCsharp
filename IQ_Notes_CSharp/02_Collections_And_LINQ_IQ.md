# 02 - Collections & LINQ in C# vs JavaScript Array Methods

## Overview
In JavaScript and TypeScript, collections are almost exclusively handled by dynamic arrays (`Array<T>`), which possess high-level prototype methods such as `.filter()`, `.map()`, `.find()`, and `.sort()`. Every call creates an immediate, eager array in memory.

In C#, arrays (`T[]`) have a fixed size upon allocation, while dynamic lists are provided by `List<T>`. Advanced querying and transformation across all collections is provided by **LINQ (Language-Integrated Query)**. Crucially, LINQ query operators use **deferred (lazy) execution**, evaluating elements on-demand rather than allocating intermediate collections.

---

## Main Concept

### The LINQ to JavaScript Mapping Table

| JavaScript / TypeScript Array Method | C# LINQ Equivalent Method | Behavior |
| :--- | :--- | :--- |
| `items.filter(predicate)` | `items.Where(predicate)` | Lazy evaluation |
| `items.map(transform)` | `items.Select(transform)` | Lazy projection |
| `items.find(predicate)` | `items.FirstOrDefault(predicate)` | Returns match or `default` |
| `items.some(predicate)` | `items.Any(predicate)` | Returns boolean |
| `items.every(predicate)` | `items.All(predicate)` | Returns boolean |
| `items.sort((a, b) => ...)` | `items.OrderBy(keySelector)` | Non-mutating order |
| `items.reduce((acc, curr) => ...)` | `items.Aggregate(seed, func)` | Accumulation |

### Code Comparison

```typescript
// TypeScript: Eager Array operations
const scores = [45, 82, 91, 30, 75];

const passingHighToLow = scores
  .filter(s => s >= 60)
  .sort((a, b) => b - a)
  .map(s => `Score: ${s}`);
// passingHighToLow is an immediate array in memory
```

```csharp
// C#: Dynamic List & Deferred LINQ
List<int> scores = new() { 45, 82, 91, 30, 75 };

// LINQ pipeline is deferred until enumerated (.ToList(), foreach)
List<string> passingHighToLow = scores
    .Where(s => s >= 60)
    .OrderByDescending(s => s)
    .Select(s => $"Score: {s}")
    .ToList(); // Materializes query into a new List<string>
```

---

## Common Mistakes

1. **Unintended Multiple Enumeration**:
   Because LINQ queries are deferred (`IEnumerable<T>`), executing `query.Count()` followed by `foreach (var item in query)` will execute the underlying query logic twice. If the query calls expensive methods, materialize it once with `.ToList()` or `.ToArray()`.

2. **Expecting `OrderBy` to Mutate in Place**:
   In JavaScript, `array.sort()` mutates the source array in place. In C#, `items.OrderBy(...)` does **not** mutate the original list; it returns a newly ordered sequence.

3. **Confusing `First()` and `FirstOrDefault()`**:
   Calling `.First()` on an empty collection or when no match exists throws an `InvalidOperationException`. In automated test assertions, prefer `.FirstOrDefault()` and assert against `null` or default value, unless an exception is explicitly desired.

---

## Summary
- C# distinguishes between fixed-size arrays (`T[]`) and dynamically sized collections (`List<T>`).
- LINQ provides a unified, expressive query syntax across all collection types with deferred execution.
- Materialize queries with `.ToList()` or `.ToArray()` when repeated access is required to prevent multiple enumeration overhead.

**Key Takeaway:** Replace JS `.filter()` and `.map()` with C# LINQ `.Where()` and `.Select()`, keeping deferred execution in mind for optimal automation performance.
