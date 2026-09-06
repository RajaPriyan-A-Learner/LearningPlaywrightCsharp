// ============================================================================
// C# Learning Framework - Chapter 07: Enums & Abstract Classes
// Equivalent to TypeScript: String Literal Unions / Enums & Abstract Base Classes
// ============================================================================

namespace CSharpLearning.AbstractionsAndInterfaces;

/// <summary>
/// Strongly-typed Enum representing test execution outcomes.
/// In TypeScript, developers frequently use string literal unions:
///   type TestStatus = "Passed" | "Failed" | "Skipped" | "Running";
/// In C#, enums are first-class, integral-backed named constants with strict type safety.
/// </summary>
public enum TestStatus
{
    Pending = 0,
    Running = 1,
    Passed = 2,
    Failed = 3,
    Skipped = 4
}

/// <summary>
/// Enum representing target browser engines.
/// </summary>
public enum BrowserType
{
    Chromium,
    Firefox,
    WebKit
}

/// <summary>
/// Abstract base class establishing the template pattern for test runners.
/// Unlike an interface which only defines public contracts, an abstract class can:
/// 1. Maintain protected state (e.g., TestName, Status, ExecutionTime).
/// 2. Provide concrete common logic (e.g., logging, metrics recording).
/// 3. Force derived classes to implement abstract members via `override`.
/// 4. Provide optional hooks via `virtual` methods.
/// </summary>
public abstract class BaseTestRunner
{
    public string TestName { get; }
    public TestStatus Status { get; protected set; } = TestStatus.Pending;
    public DateTime? StartedAt { get; private set; }
    public DateTime? FinishedAt { get; private set; }

    protected BaseTestRunner(string testName)
    {
        TestName = testName;
    }

    /// <summary>
    /// Abstract method: Derived classes MUST provide an implementation.
    /// </summary>
    public abstract void Setup();

    /// <summary>
    /// Abstract method: Derived classes MUST implement the primary test payload.
    /// </summary>
    public abstract void Execute();

    /// <summary>
    /// Virtual hook: Base provides a default cleanup, derived classes can override if needed.
    /// </summary>
    public virtual void Cleanup()
    {
        Console.WriteLine($"[Teardown] Default cleanup completed for '{TestName}'.");
    }

    /// <summary>
    /// Template method coordinating test lifecycle.
    /// </summary>
    public void Run()
    {
        Status = TestStatus.Running;
        StartedAt = DateTime.UtcNow;
        Console.WriteLine($"\n--- Starting Test '{TestName}' [Status: {Status}] ---");

        try
        {
            Setup();
            Execute();
            Status = TestStatus.Passed;
            Console.WriteLine($"[Success] Test '{TestName}' PASSED.");
        }
        catch (Exception ex)
        {
            Status = TestStatus.Failed;
            Console.WriteLine($"[Error] Test '{TestName}' FAILED: {ex.Message}");
        }
        finally
        {
            Cleanup();
            FinishedAt = DateTime.UtcNow;
            var duration = (FinishedAt.Value - StartedAt.Value).TotalMilliseconds;
            Console.WriteLine($"--- Finished '{TestName}' with final status [{Status}] in {duration:F2}ms ---");
        }
    }
}

/// <summary>
/// Concrete derived runner for UI Tests.
/// </summary>
public class UiTestRunner : BaseTestRunner
{
    public BrowserType TargetBrowser { get; }

    public UiTestRunner(string testName, BrowserType targetBrowser)
        : base(testName)
    {
        TargetBrowser = targetBrowser;
    }

    public override void Setup()
    {
        Console.WriteLine($"[Setup] Launching browser engine: {TargetBrowser}");
    }

    public override void Execute()
    {
        Console.WriteLine($"[Execute] Navigating to dashboard and asserting DOM state on {TargetBrowser}...");
    }

    public override void Cleanup()
    {
        Console.WriteLine($"[Teardown] Closing browser context and flushing video recordings for {TargetBrowser}.");
        base.Cleanup();
    }
}

/// <summary>
/// Concrete derived runner for API Tests.
/// </summary>
public class ApiTestRunner : BaseTestRunner
{
    public string EndpointUrl { get; }

    public ApiTestRunner(string testName, string endpointUrl)
        : base(testName)
    {
        EndpointUrl = endpointUrl;
    }

    public override void Setup()
    {
        Console.WriteLine($"[Setup] Initializing HttpClient with base endpoint: {EndpointUrl}");
    }

    public override void Execute()
    {
        Console.WriteLine($"[Execute] Sending GET request to {EndpointUrl}/health and verifying 200 OK status.");
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== C# Enums and Abstract Classes Demonstration ===");

        // Polymorphic list of BaseTestRunner instances
        List<BaseTestRunner> suite = new()
        {
            new UiTestRunner("Verify Dashboard Rendering", BrowserType.Chromium),
            new ApiTestRunner("Verify Healthcheck API", "https://api.example.com")
        };

        foreach (var test in suite)
        {
            test.Run();
        }

        Console.WriteLine("\n=== Suite Execution Complete ===");
    }
}
