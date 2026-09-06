# 08 - Polymorphism: Virtual, Override & Method Hiding in C# vs TypeScript

## Overview
In JavaScript and TypeScript, every class method is virtual by default. Dynamic dispatch happens automatically via the prototype chain; if a subclass defines a method with the same name, it immediately overrides the parent method.

In C#, methods are **non-virtual by default** for maximum performance and explicit intent. Polymorphic runtime dispatch requires two explicit keywords:
1. `virtual` on the base class method.
2. `override` on the derived class method.

If a derived class introduces a method with the same name without `override`, C# treats it as **method hiding** (via the `new` keyword), which breaks virtual dispatch when called through a base reference.

---

## Main Concept

### TypeScript Automatic Virtual Dispatch vs C# Explicit Virtual / Override

```typescript
// TypeScript: Every method is virtual by default
class BaseStep {
  execute(): void {
    console.log("Base step execution");
  }
}

class UiStep extends BaseStep {
  execute(): void {
    console.log("UI step execution");
  }
}

const step: BaseStep = new UiStep();
step.execute(); // Outputs: "UI step execution"
```

```csharp
// C#: Explicit virtual / override contract
public class BaseTestStep
{
    // Must explicitly declare 'virtual' to allow dynamic dispatch
    public virtual void Execute()
    {
        Console.WriteLine("[Base] Generic step execution.");
    }
}

public class UiClickStep : BaseTestStep
{
    // Must explicitly declare 'override'
    public override void Execute()
    {
        Console.WriteLine("[UiClickStep] Clicking target element on DOM.");
    }
}

public class FlawedStep : BaseTestStep
{
    // Method hiding: Not polymorphic!
    public new void Execute()
    {
        Console.WriteLine("[FlawedStep] Custom step.");
    }
}
```

### The Polymorphic Dispatch Difference

```csharp
BaseTestStep s1 = new UiClickStep();
s1.Execute(); // Dispatches dynamically to [UiClickStep] Execute()!

BaseTestStep s2 = new FlawedStep();
s2.Execute(); // Dispatches statically to [Base] Generic step execution! (Bug!)
```

---

## Common Mistakes

1. **Omitting the `override` Keyword**:
   If you forget `override` when creating a subclass method, the C# compiler issues warning `CS0108` and defaults to method hiding (`new`). When your test runner iterates over a `List<BaseTestStep>`, it will call the base method, ignoring your child logic!

2. **Omitting `virtual` on the Base Class**:
   If the base method is not marked `virtual` (or `abstract`), a derived class cannot use `override`. Attempting to do so results in compile error `CS0506`.

3. **Confusing Method Overloading with Method Overriding**:
   - Overloading = Same method name, different parameter types in the same class.
   - Overriding = Same method name and exact same parameter signature in a derived class to replace base behavior.

---

## Summary
- JavaScript/TypeScript methods are virtual by default; C# methods are non-virtual by default.
- In C#, runtime polymorphism requires `virtual` on the parent and `override` on the child.
- Avoid method hiding (`new`) when building extensible test runners or page base classes.

**Key Takeaway:** Always mark your base framework extension hooks as `virtual` and your derived test implementations with `override` to ensure clean, reliable runtime polymorphism.
