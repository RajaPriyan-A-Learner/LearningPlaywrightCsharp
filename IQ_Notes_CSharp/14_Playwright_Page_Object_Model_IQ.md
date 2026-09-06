# 14 - Playwright for .NET: Page Object Model (POM) in C# vs TypeScript

## Overview
The Page Object Model (POM) is the industry standard design pattern for resilient UI test automation. Chapter 28 introduced the POM in TypeScript using `@playwright/test`.

In C# (.NET), the official library is `Microsoft.Playwright`. While the core philosophy remains identical, C# adheres strictly to .NET idioms:
- Asynchronous methods follow the **PascalCase + `Async`** suffix convention (e.g., `GotoAsync`, `ClickAsync`, `FillAsync`).
- Locators are lazily resolved using expression-bodied properties (`=> Page.Locator(...)`).
- Method transitions return strongly-typed next Page Objects to enable fluent, compile-time verified test authoring.

---

## Main Concept

### Playwright API Comparison: TypeScript vs C# (.NET)

| Action | TypeScript (`@playwright/test`) | C# (`Microsoft.Playwright`) |
| :--- | :--- | :--- |
| Navigation | `await page.goto(url);` | `await Page.GotoAsync(url);` |
| Text Input | `await locator.fill(text);` | `await locator.FillAsync(text);` |
| Click | `await locator.click();` | `await locator.ClickAsync();` |
| Text Assertion | `await expect(loc).toHaveText("OK");` | `await Expect(loc).ToHaveTextAsync("OK");` |
| Visibility Assertion | `await expect(loc).toBeVisible();` | `await Expect(loc).ToBeVisibleAsync();` |
| Load State | `await page.waitForLoadState("load");` | `await Page.WaitForLoadStateAsync(LoadState.Load);` |

### Idiomatic C# Page Object Implementation

```csharp
public abstract class BasePage
{
    protected readonly IPage Page;

    protected BasePage(IPage page)
    {
        Page = page ?? throw new ArgumentNullException(nameof(page));
    }

    public string CurrentUrl => Page.Url;
}

public class LoginPage : BasePage
{
    // Lazy locators: Evaluated on every access; prevents stale element issues
    private ILocator UsernameInput => Page.Locator("#user-name");
    private ILocator PasswordInput => Page.Locator("#password");
    private ILocator LoginButton => Page.Locator("#login-button");
    private ILocator ErrorBanner => Page.Locator("[data-test='error']");

    public LoginPage(IPage page) : base(page) { }

    public async Task OpenAsync(string baseUrl)
    {
        await Page.GotoAsync($"{baseUrl}/login");
    }

    // High-level fluent transition returning the next Page Object
    public async Task<DashboardPage> LoginAsValidUserAsync(string username, string password)
    {
        await UsernameInput.FillAsync(username);
        await PasswordInput.FillAsync(password);
        await LoginButton.ClickAsync();

        // Return next Page Object initialized with the active page context
        return new DashboardPage(Page);
    }

    public async Task<bool> HasErrorAsync()
    {
        return await ErrorBanner.IsVisibleAsync();
    }
}
```

### Clean Test Method Consumption

```csharp
[Test]
public async Task ValidLogin_NavigatesToDashboard()
{
    var loginPage = new LoginPage(Page);
    await loginPage.OpenAsync("https://automation.example.com");

    // Fluent transition: returns DashboardPage instance
    DashboardPage dashboard = await loginPage.LoginAsValidUserAsync("qa_admin", "Secr3t!");

    // Assert state on the new page
    Assert.True(await dashboard.IsLoadedAsync());
}
```

---

## Common Mistakes

1. **Forgetting the `Async` Suffix in C# Playwright**:
   Unlike TypeScript where methods are `page.goto()` and `page.click()`, .NET Playwright methods require `Async` (`Page.GotoAsync()`, `locator.ClickAsync()`). Omitting the suffix will fail compilation.

2. **Storing Eager Locator References in Fields Instead of Expression Properties**:
   Declaring `private ILocator btn = page.Locator("#btn");` evaluates once upon construction. Using expression-bodied getters `private ILocator Btn => Page.Locator("#btn");` evaluates lazily, ensuring auto-waiting and dynamic re-resolution when DOM elements re-render.

3. **Returning Void from Action Methods Instead of Fluent Next Page Objects**:
   When an action (like `Login`) always navigates the user to a new screen, returning the next Page Object (`Task<DashboardPage>`) guides test authors through valid user journeys at compile-time.

---

## Summary
- `Microsoft.Playwright` brings full Playwright capabilities to the .NET ecosystem with native `Task`-based asynchronous operations.
- Methods follow PascalCase with the `Async` suffix convention.
- Expression-bodied properties (`=> Page.Locator(...)`) provide lazy locator resolution, pairing perfectly with Playwright's built-in auto-waiting engine.

**Key Takeaway:** Build enterprise-grade Playwright .NET frameworks by combining encapsulated lazy locators, PascalCase `Async` methods, and fluent Page Object transitions.
