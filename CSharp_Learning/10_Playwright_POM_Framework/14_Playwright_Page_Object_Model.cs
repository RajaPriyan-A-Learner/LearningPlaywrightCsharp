// ============================================================================
// C# Learning Framework - Chapter 10: Playwright for .NET Page Object Model
// Equivalent to TypeScript: Chapter 28 - Page Object Model & Playwright Patterns
// ============================================================================

namespace CSharpLearning.PlaywrightPom;

/// <summary>
/// Interface matching Microsoft.Playwright.ILocator for standalone structural fidelity.
/// Demonstrates the .NET Playwright naming convention: PascalCase + Async suffix.
/// </summary>
public interface IPlaywrightLocator
{
    string Selector { get; }
    Task ClickAsync();
    Task FillAsync(string value);
    Task<string> TextContentAsync();
    Task<bool> IsVisibleAsync();
}

/// <summary>
/// Interface matching Microsoft.Playwright.IPage for standalone structural fidelity.
/// </summary>
public interface IPlaywrightPage
{
    string Url { get; }
    Task GotoAsync(string url);
    IPlaywrightLocator Locator(string selector);
    Task WaitForLoadStateAsync(string state = "load");
}

#region Simulated Engine Implementation for Demonstration

internal class MockPlaywrightLocator : IPlaywrightLocator
{
    public string Selector { get; }
    private string _storedValue = "";

    public MockPlaywrightLocator(string selector) => Selector = selector;

    public async Task ClickAsync()
    {
        await Task.Delay(20);
        Console.WriteLine($"[Playwright .NET] Clicked locator '{Selector}'");
    }

    public async Task FillAsync(string value)
    {
        await Task.Delay(20);
        _storedValue = value;
        Console.WriteLine($"[Playwright .NET] Filled locator '{Selector}' with '{(Selector.Contains("password") ? "••••••••" : value)}'");
    }

    public async Task<string> TextContentAsync()
    {
        await Task.Delay(10);
        return _storedValue;
    }

    public async Task<bool> IsVisibleAsync()
    {
        await Task.Delay(10);
        return true;
    }
}

internal class MockPlaywrightPage : IPlaywrightPage
{
    public string Url { get; private set; } = "about:blank";

    public async Task GotoAsync(string url)
    {
        await Task.Delay(50);
        Url = url;
        Console.WriteLine($"[Playwright .NET] Navigated to '{url}'");
    }

    public IPlaywrightLocator Locator(string selector) => new MockPlaywrightLocator(selector);

    public async Task WaitForLoadStateAsync(string state = "load")
    {
        await Task.Delay(10);
        Console.WriteLine($"[Playwright .NET] Load state reached: '{state}'");
    }
}

#endregion

/// <summary>
/// Base Page class for all Playwright .NET Page Objects.
/// Provides protected access to IPage and universal navigation/assertion utilities.
/// </summary>
public abstract class BasePage
{
    protected readonly IPlaywrightPage Page;

    protected BasePage(IPlaywrightPage page)
    {
        Page = page ?? throw new ArgumentNullException(nameof(page));
    }

    public string CurrentUrl => Page.Url;

    public async Task NavigateToAsync(string url)
    {
        await Page.GotoAsync(url);
        await Page.WaitForLoadStateAsync();
    }
}

/// <summary>
/// Dashboard Page Object returned upon successful login.
/// Demonstrates POM fluent navigation transitions.
/// </summary>
public class DashboardPage : BasePage
{
    private IPlaywrightLocator HeaderGreeting => Page.Locator("h1.welcome-message");
    private IPlaywrightLocator LogoutButton => Page.Locator("button#btn-logout");

    public DashboardPage(IPlaywrightPage page) : base(page)
    {
    }

    public async Task<bool> IsLoadedAsync()
    {
        return await HeaderGreeting.IsVisibleAsync();
    }

    public async Task LogoutAsync()
    {
        Console.WriteLine("[DashboardPage] Performing logout...");
        await LogoutButton.ClickAsync();
    }
}

/// <summary>
/// Login Page Object implementing enterprise POM patterns in C# Playwright.
/// 
/// Comparison with TypeScript Playwright:
/// 1. Methods follow .NET async convention: PascalCase + Async (e.g. GotoAsync vs goto).
/// 2. Properties use C# expression-bodied syntax `=> Page.Locator(...)` for lazy locator resolution.
/// 3. Returns strongly-typed next Page Objects (e.g. DashboardPage) for fluent test writing.
/// </summary>
public class LoginPage : BasePage
{
    // Lazy locators: Evaluated dynamically when accessed
    private IPlaywrightLocator UsernameInput => Page.Locator("#user-name");
    private IPlaywrightLocator PasswordInput => Page.Locator("#password");
    private IPlaywrightLocator LoginButton => Page.Locator("#login-button");
    private IPlaywrightLocator ErrorMessageBanner => Page.Locator("[data-test='error']");

    public LoginPage(IPlaywrightPage page) : base(page)
    {
    }

    public async Task OpenAsync(string baseUrl)
    {
        await NavigateToAsync($"{baseUrl}/login");
    }

    /// <summary>
    /// Executes login credentials entry and submission.
    /// </summary>
    public async Task EnterCredentialsAsync(string username, string password)
    {
        await UsernameInput.FillAsync(username);
        await PasswordInput.FillAsync(password);
        await LoginButton.ClickAsync();
    }

    /// <summary>
    /// Fluent workflow method: Logs in with valid credentials and transitions to DashboardPage.
    /// </summary>
    public async Task<DashboardPage> LoginAsValidUserAsync(string username, string password)
    {
        Console.WriteLine($"[LoginPage] Submitting valid login for user: {username}");
        await EnterCredentialsAsync(username, password);
        return new DashboardPage(Page);
    }

    /// <summary>
    /// Verification helper checking error state on invalid login attempt.
    /// </summary>
    public async Task<bool> HasErrorMessageAsync()
    {
        return await ErrorMessageBanner.IsVisibleAsync();
    }
}

public class Program
{
    public static async Task Main()
    {
        Console.WriteLine("=== C# Playwright Page Object Model (POM) Demonstration ===");

        IPlaywrightPage page = new MockPlaywrightPage();
        var loginPage = new LoginPage(page);

        // 1. Navigate to page
        await loginPage.OpenAsync("https://automation-demo.playwright.dev");

        // 2. Perform login and fluently transition to DashboardPage
        DashboardPage dashboard = await loginPage.LoginAsValidUserAsync("standard_user", "secret_sauce");

        // 3. Interact with Dashboard
        bool isDashboardReady = await dashboard.IsLoadedAsync();
        Console.WriteLine($"Dashboard successfully loaded: {isDashboardReady}");

        await dashboard.LogoutAsync();

        Console.WriteLine("\n=== Playwright POM Execution Complete ===");
    }
}
