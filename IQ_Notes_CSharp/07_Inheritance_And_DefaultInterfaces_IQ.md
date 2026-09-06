# 07 - Inheritance & Default Interface Methods (Mixins) in C# vs TypeScript

## Overview
Both C# and TypeScript support single class inheritance (`extends` in TS, `:` in C#). Neither language supports multiple class inheritance.

In TypeScript, multiple behaviors are frequently shared across classes using **Mixins**—functions that take a class constructor and return an extended class, or prototype copying functions (`applyMixins`).

In C# (since C# 8), multiple behavior sharing without multiple class inheritance is achieved via **Default Interface Methods (DIM)**. Interfaces can provide concrete default implementations of methods, effectively serving as compile-safe mixins.

---

## Main Concept

### Class Inheritance & Constructor Forwarding

```csharp
// Base class
public class BaseTestComponent
{
    public string ComponentName { get; }

    public BaseTestComponent(string name)
    {
        ComponentName = name;
    }

    public virtual void ResetState()
    {
        Console.WriteLine($"[Base] Resetting state for {ComponentName}");
    }
}

// Derived class forwarding constructor using ': base(name)'
public class NavigationBarComponent : BaseTestComponent
{
    public NavigationBarComponent() : base("NavigationBar")
    {
    }

    public override void ResetState()
    {
        base.ResetState(); // Invoke base logic
        Console.WriteLine("[Derived] Clearing nav selection cache.");
    }
}
```

### TypeScript Prototype Mixin vs C# Default Interface Method

```typescript
// TypeScript: Function/Prototype Mixin
type Constructor = new (...args: any[]) => {};

function WithScreenshotMixin<TBase extends Constructor>(Base: TBase) {
  return class extends Base {
    takeScreenshot(label: string) {
      console.log(`[Screenshot] Captured: ${label}`);
    }
  };
}
```

```csharp
// C#: Default Interface Method (Compile-safe Mixin)
public interface IScreenshotCapture
{
    // Default implementation inside interface
    void TakeScreenshot(string label)
    {
        Console.WriteLine($"[Default Interface Mixin] Captured screenshot: '{label}'.png");
    }
}

public interface IMetricsRecorder
{
    void RecordDuration(string testName, double ms)
    {
        Console.WriteLine($"[Default Interface Mixin] Metric '{testName}': {ms:F2}ms");
    }
}

// A single class can implement multiple interfaces, acquiring all default behaviors!
public class ModernPageTest : BaseTestComponent, IScreenshotCapture, IMetricsRecorder
{
    public ModernPageTest(string name) : base(name) { }
}
```

---

## Common Mistakes

1. **Calling a Default Interface Method Directly on the Concrete Class**:
   In C#, default interface methods are not inherited directly into the class's public class contract. Calling `test.TakeScreenshot(...)` results in a compiler error unless called through the interface reference:
   ```csharp
   // Correct call:
   ((IScreenshotCapture)test).TakeScreenshot("login_error");
   // Or declare the variable as the interface:
   IScreenshotCapture capturer = test;
   capturer.TakeScreenshot("login_error");
   ```

2. **Forgetting `: base(...)` in Derived Constructors**:
   If a base class does not define a parameterless constructor, every derived class constructor must explicitly call `: base(...)`. Omitting it causes compile-time error `CS7036`.

3. **Creating Deep, Fragile Inheritance Hierarchies**:
   Inheriting more than 2-3 levels deep (`Base -> BaseUi -> BasePage -> BaseAuthPage -> LoginPage`) creates high coupling. Use interfaces and composition instead.

---

## Summary
- C# enforces single class inheritance using the colon (`:`) syntax and constructor forwarding (`: base(...)`).
- Multi-tier behavior composition is achieved cleanly via C# Default Interface Methods.
- DIM methods must be invoked via the interface reference or explicit casting.

**Key Takeaway:** Replace fragile TypeScript prototype mixins with C# Default Interface Methods to compose reusable test capabilities (e.g. logging, screenshot capture, metrics) across independent Page Objects.
