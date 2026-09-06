// ============================================================================
// C# Learning Framework - Chapter 09: Access Modifiers & Readonly Encapsulation
// Equivalent to TypeScript: public, private (#private), protected, readonly, and modules
// ============================================================================

namespace CSharpLearning.AccessModifiers;

/// <summary>
/// Dummy locator representation simulating Playwright's ILocator for encapsulation demo.
/// </summary>
public record SimulatedLocator(string Selector)
{
    public void Click() => Console.WriteLine($"Clicked selector: '{Selector}'");
    public void Fill(string text) => Console.WriteLine($"Filled '{Selector}' with: '{text}'");
}

/// <summary>
/// Demonstrates C#'s rich access modifier system applied to Page Object Model encapsulation.
/// 
/// Comparison with TypeScript:
/// - TypeScript: `private` keyword is purely compile-time and can be bypassed via `(obj as any).field`.
///   `#private` is runtime private. TS has no `internal` modifier (uses export boundaries).
/// - C#: Modifiers are strictly enforced by the CLR at compile time AND runtime.
///   - `public`: Accessible anywhere.
///   - `private`: Accessible only within this class.
///   - `protected`: Accessible within this class and derived classes.
///   - `internal`: Accessible anywhere within the same compiled assembly (.dll).
///   - `protected internal`: Accessible within assembly OR derived classes in other assemblies.
///   - `readonly`: Can only be assigned at declaration or in a constructor.
/// </summary>
public class BasePage
{
    // Protected readonly: Subclasses can access the URL path and driver, but cannot reassign after construction
    protected readonly string BaseUrl;
    
    // Internal state: Framework-level monitoring tools in the same assembly can inspect this
    internal int NavigationCount { get; private set; }

    public BasePage(string baseUrl)
    {
        BaseUrl = baseUrl;
    }

    protected void TrackNavigation()
    {
        NavigationCount++;
        Console.WriteLine($"[BasePage] Navigation count incremented to: {NavigationCount}");
    }
}

/// <summary>
/// Concrete Page Object illustrating strict encapsulation.
/// Raw locators are NEVER exposed publicly to test methods.
/// </summary>
public class SecureLoginPage : BasePage
{
    // 1. Private readonly locators: Completely hidden from tests. Prevents test fragility.
    private readonly SimulatedLocator _usernameInput = new("#txt-username");
    private readonly SimulatedLocator _passwordInput = new("#txt-password");
    private readonly SimulatedLocator _submitButton = new("button[type='submit']");
    private readonly SimulatedLocator _errorMessage = new(".alert-danger");

    // 2. Public property exposing safe, high-level page information
    public string PageRoute => $"{BaseUrl}/auth/login";

    public SecureLoginPage(string baseUrl) : base(baseUrl)
    {
    }

    // 3. Public high-level action method: Tests interact through business verbs
    public void PerformLogin(string username, string password)
    {
        Console.WriteLine($"[SecureLoginPage] Navigating to {PageRoute}...");
        TrackNavigation();

        Console.WriteLine("[SecureLoginPage] Entering credentials...");
        _usernameInput.Fill(username);
        _passwordInput.Fill("********"); // Log masked password
        _submitButton.Click();
    }

    // 4. Internal method: Can be called by test fixtures/helpers in the same assembly for test setup
    internal void FastBypassLoginWithCookie(string sessionToken)
    {
        Console.WriteLine($"[Internal Setup] Injected auth session token: {sessionToken}");
        TrackNavigation();
    }

    // 5. Public assertion helper: Exposes boolean state without exposing the raw locator
    public bool IsErrorDisplayed()
    {
        Console.WriteLine($"Checking visibility of error message '{_errorMessage.Selector}'");
        return false;
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== C# Access Modifiers & Encapsulation Demonstration ===");

        var loginPage = new SecureLoginPage("https://staging.app.com");

        // Public surface area:
        Console.WriteLine($"Page URL: {loginPage.PageRoute}");
        loginPage.PerformLogin("automation_user", "SecretPass123!");

        // Internal assembly member access:
        loginPage.FastBypassLoginWithCookie("sess_abc123xyz");
        Console.WriteLine($"Total navigations recorded (internal read): {loginPage.NavigationCount}");

        // Note: The following lines would fail at compile time in C#:
        // loginPage._usernameInput.Fill("hacker"); // Compile Error: '_usernameInput' is inaccessible due to its protection level.
        // loginPage.BaseUrl = "http://evil.com";    // Compile Error: 'BaseUrl' is inaccessible and readonly.
        Console.WriteLine("\nRaw locators are strictly inaccessible outside SecureLoginPage class.");
    }
}
