using System;

namespace CSharp_Learning.LanguageFundamentals
{
    /// <summary>
    /// Demonstrates C# primitive data types, explicit numeric precision,
    /// and C# Nullable Reference Types (NRT) in contrast to JavaScript/TypeScript.
    /// 
    /// TS/JS Equivalent: 198.ts (Primitive Types, Number, String, Boolean, Null, Undefined)
    /// </summary>
    public class PrimitivesAndNullability
    {
        public static void RunDemo()
        {
            Console.WriteLine("=== C# Primitives & Nullability Demo ===");

            // 1. Explicit Numeric Types (Contrast: TS/JS only has single IEEE 754 'number')
            int intValue = 42;                     // 32-bit signed integer
            long largeCount = 398765434567L;       // 64-bit signed integer
            double piDouble = 3.14159265359;       // 64-bit binary floating point
            decimal financialRate = 19.99m;        // 128-bit precise decimal (ideal for currency/asserting prices)

            // 2. Boolean & Text
            bool isActive = true;
            string testName = "Login Test Suite";

            // 3. Nullability in C# vs TS/JS
            // In C#, there is NO 'undefined'. Unassigned reference types evaluate to null.
            string nonNullableText = "Guaranteed Non-Null";
            string? nullableText = null; // C# 8+ Nullable Reference Type

            // Null-conditional operator (?.) and Null-coalescing operator (??)
            int length = nullableText?.Length ?? 0;

            Console.WriteLine($"Int: {intValue} | Long: {largeCount}");
            Console.WriteLine($"Double: {piDouble} | Decimal: {financialRate}");
            Console.WriteLine($"Status: {isActive} | Test: {testName}");
            Console.WriteLine($"Nullable Length Result: {length}");
        }
    }
}
