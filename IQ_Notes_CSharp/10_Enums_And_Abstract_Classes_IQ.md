# 10 - Enums & Abstract Classes: Type-Safe Constants & Template Method Pattern

## Overview
In TypeScript, developers frequently model states and fixed options using string literal unions (e.g., `type TestStatus = "Passed" | "Failed" | "Skipped"`). While expressive, string literals risk typo-based bugs if type checking is loosened (e.g., `any` casts).

In C#, **Enums** are distinct value types backed by integer primitives, offering strict type safety. Furthermore, when designing extensible test architectures, C# **Abstract Classes** enable the **Template Method Pattern**—defining the skeleton of an algorithm in a base class while deferring specific steps to subclasses.

---

## Main Concept

### TypeScript String Literal Unions vs C# Enums

```typescript
// TypeScript: String literal union
type BrowserType = "Chromium" | "Firefox" | "WebKit";
type TestStatus = "Passed" | "Failed" | "Skipped";

function executeOn(browser: BrowserType, status: TestStatus) {
  // String comparisons at runtime
  if (browser === "Chromium") { ... }
}
```

```csharp
// C#: Strongly-typed enum backed by integral values
public enum BrowserType
{
    Chromium,
    Firefox,
    WebKit
}

public enum TestStatus
{
    Pending = 0,
    Running = 1,
    Passed = 2,
    Failed = 3,
    Skipped = 4
}
```

### Abstract Classes & The Template Method Pattern

An abstract class cannot be instantiated directly with `new`. It defines abstract contracts that derived classes *must* implement, while providing reusable concrete lifecycle logic:

```csharp
public abstract class BaseTestRunner
{
    public string TestName { get; }
    public TestStatus Status { get; protected set; } = TestStatus.Pending;

    protected BaseTestRunner(string testName) => TestName = testName;

    // Abstract methods: Mandatory child implementations
    public abstract void Setup();
    public abstract void Execute();

    // Virtual method: Optional hook
    public virtual void Teardown() => Console.WriteLine("Default teardown.");

    // Template method: Dictates execution lifecycle order
    public void Run()
    {
        Setup();
        try
        {
            Execute();
            Status = TestStatus.Passed;
        }
        catch
        {
            Status = TestStatus.Failed;
            throw;
        }
        finally
        {
            Teardown();
        }
    }
}
```

---

## Abstract Class vs Interface: When to Use Which?

| Feature | Interface (`interface`) | Abstract Class (`abstract class`) |
| :--- | :--- | :--- |
| Purpose | Defines a public capability contract | Defines base identity & template lifecycle |
| State Storage | Cannot store instance fields | Can declare protected/private fields & state |
| Constructors | Cannot have constructors | Can have parameterized `protected` constructors |
| Multiple Inheritance | A class can implement multiple interfaces | A class can inherit only one abstract class |
| Access Modifiers | All members public by default | Can have `protected`, `internal`, `private` |

---

## Common Mistakes

1. **Comparing Enums with Raw Strings or Numbers**:
   Do not use string parsing (`Enum.Parse`) inside hot test loops unless deserializing external JSON. Use strongly-typed enum comparisons: `if (test.Status == TestStatus.Passed)`.

2. **Overriding Without Calling Base Hooks When Needed**:
   If a derived class overrides a virtual `Teardown()` method in an abstract runner, forgetting `base.Teardown()` can skip essential cleanup routines (e.g. closing browser contexts).

3. **Using an Abstract Class When an Interface Suffices**:
   If your component only requires a contract without shared state or a lifecycle template, prefer an `interface` to avoid consuming the class's single inheritance slot.

---

## Summary
- C# Enums eliminate magic string bugs by enforcing compile-time type safety for browser engines and test outcomes.
- Abstract classes establish the Template Method Pattern, coordinating `Setup()`, `Execute()`, and `Teardown()` reliably across UI and API test suites.
- Combine abstract classes for base lifecycle control with interfaces for composable behaviors.

**Key Takeaway:** Use Enums for discrete test states and Abstract Base Classes to standardize the lifecycle execution pipeline across diverse automation runners.
