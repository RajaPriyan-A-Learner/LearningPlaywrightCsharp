using System;

namespace CSharp_Learning.MethodsAndDelegates
{
    /// <summary>
    /// Demonstrates C# Delegates, Action, Func, and Lambda Expressions.
    /// 
    /// TS/JS Equivalent: 201.ts (Arrow Functions), 206_Test_hooks.ts (Callable Interfaces / Hook Types)
    /// </summary>
    public class DelegatesAndLambdas
    {
        // 1. Custom Delegate definition (Equivalent to TypeScript Callable Interface)
        public delegate void TestHookDelegate(string testName);

        public static void RunDemo()
        {
            Console.WriteLine("=== C# Delegates & Lambdas Demo ===");

            // 2. Action<T> delegate: Takes parameters, returns void (Perfect for hooks & side-effects)
            Action<string> beforeEachHook = testName =>
            {
                Console.WriteLine($"[BEFORE_EACH] Initializing browser context for: {testName}");
            };

            Action<string> afterEachHook = testName =>
            {
                Console.WriteLine($"[AFTER_EACH] Disposing browser context for: {testName}");
            };

            beforeEachHook("Checkout Flow Spec");
            afterEachHook("Checkout Flow Spec");

            // 3. Func<T1, T2, TResult> delegate: Takes parameters, returns typed result (Equivalent to JS Arrow Functions)
            Func<int, int, int> multiply = (a, b) => a * b;
            Console.WriteLine("Product (6 * 7): " + multiply(6, 7));

            // 4. Predicate<T> / Func<T, bool>: Condition evaluator for assertions
            Func<int, bool> isCriticalStatus = code => code >= 500;
            Console.WriteLine("500 is Critical: " + isCriticalStatus(500));
            Console.WriteLine("200 is Critical: " + isCriticalStatus(200));
        }
    }
}
