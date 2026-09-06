using System;

namespace CSharp_Learning.MethodsAndDelegates
{
    /// <summary>
    /// Demonstrates C# Method Signatures and Native Method Overloading.
    /// 
    /// TS/JS Equivalent: 193.ts (Function Type Annotations), 197.ts (Void Return), 
    /// and contrasts with JS where method overloading is impossible natively.
    /// </summary>
    public class MethodsAndOverloading
    {
        // 1. Standard Method with explicit parameter & return types
        public static string BuildEndpoint(string baseUrl, string path)
        {
            return baseUrl.TrimEnd('/') + "/" + path.TrimStart('/');
        }

        // 2. Boolean validation helper
        public static bool IsSuccessCode(int statusCode)
        {
            return statusCode >= 200 && statusCode < 300;
        }

        // 3. Void method (Side-effect only: logs step)
        public static void LogTestStep(string stepName)
        {
            Console.WriteLine($"[STEP] {stepName}");
        }

        // 4. Native Method Overloading (Compile-time Polymorphism)
        // C# natively distinguishes methods by parameter signatures:
        public static string FormatTestResult(string testName, string status)
        {
            return $"Test '{testName}' concluded with status: {status}";
        }

        public static string FormatTestResult(string testName, string status, long durationMs)
        {
            return $"Test '{testName}' concluded with status: {status} in {durationMs}ms";
        }

        public static void RunDemo()
        {
            Console.WriteLine("=== C# Methods & Overloading Demo ===");

            string endpoint = BuildEndpoint("https://api.staging.com", "users");
            Console.WriteLine("Built Endpoint: " + endpoint);

            Console.WriteLine("200 is Success: " + IsSuccessCode(200));
            Console.WriteLine("404 is Success: " + IsSuccessCode(404));

            LogTestStep("Executing authentication check");

            // Invoking overloaded variants:
            Console.WriteLine(FormatTestResult("LoginTest", "PASS"));
            Console.WriteLine(FormatTestResult("CheckoutTest", "PASS", 1450L));
        }
    }
}
