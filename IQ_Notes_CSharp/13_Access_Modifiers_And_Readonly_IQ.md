# 13 - Access Modifiers & Readonly Encapsulation in C# vs TypeScript

## Overview
Encapsulation is the primary defense against brittle test automation. If test scripts can directly access raw selectors or modify internal page state, any small DOM change breaks dozens of tests.

In TypeScript, access modifiers (`private`, `protected`) exist purely at compile-time and can be trivially bypassed using `(page as any).selector`. TypeScript lacks an `internal` modifier for assembly-level boundaries.

In C#, access modifiers are strictly enforced by the **Common Language Runtime (CLR)** at both compile-time and runtime. C# provides an enterprise-grade accessibility matrix, including `internal` and `readonly`.

---

## Main Concept

### Accessibility Matrix in C#

| Access Modifier | Accessibility Scope | TypeScript Equivalent |
| :--- | :--- | :--- |
| `public` | Accessible anywhere in any assembly | `public` |
| `private` | Accessible only within the declaring class | `private` or `#field` |
| `protected` | Accessible in declaring class & derived classes | `protected` |
| `internal` | Accessible anywhere within the same assembly (`.dll`) | N/A (module export boundary) |
| `protected internal` | Accessible within assembly OR derived classes elsewhere | N/A |
| `readonly` | Can only be assigned at declaration or in a constructor | `readonly` |

### Page Object Model Encapsulation Best Practice

```csharp
public class SecureLoginPage
{
    // 1. Private Readonly: Tests CANNOT touch raw selectors. Protected from test fragility.
    private readonly ILocator _usernameField;
    private readonly ILocator _passwordField;
    private readonly ILocator _loginButton;

    // 2. Readonly Base State
    public readonly string PageUrl;

    public SecureLoginPage(IPage page, string baseUrl)
    {
        PageUrl = $"{baseUrl}/login";
        _usernameField = page.Locator("#username");
        _passwordField = page.Locator("#password");
        _loginButton = page.Locator("#submit");
    }

    // 3. Public Business Action: Tests only interact through high-level intent
    public async Task LoginAsync(string user, string pass)
    {
        await _usernameField.FillAsync(user);
        await _passwordField.FillAsync(pass);
        await _loginButton.ClickAsync();
    }

    // 4. Internal Helper: Accessible by test fixtures in this project, but hidden from consumers
    internal async Task InjectSessionTokenAsync(string token)
    {
        // Direct session bypass logic for performance
    }
}
```

---

## Common Mistakes

1. **Exposing Locators as `public`**:
   Making `public ILocator SubmitBtn` allows test writers to write `page.SubmitBtn.ClickAsync()`. When the UI changes from a button to a div or changes IDs, every test file must be updated. Keep locators `private readonly`.

2. **Using Mutable Fields Instead of `readonly`**:
   Omitting `readonly` on locators or base page references allows other methods inside the POM to accidentally overwrite references midway through execution.

3. **Overusing `public` When `internal` is Appropriate**:
   Helper methods intended solely for test harness infrastructure (like cookie injectors or mock managers) should be marked `internal`, preventing test script authors from misusing them.

---

## Summary
- C# access modifiers are enforced by both compiler and CLR runtime.
- `internal` allows secure sharing across the test automation assembly without exposing implementation details publicly.
- `readonly` guarantees that locator references cannot be reassigned after initialization.

**Key Takeaway:** Lock down Page Object locators with `private readonly`, expose only high-level business methods as `public`, and use `internal` for framework-level test fixtures.
