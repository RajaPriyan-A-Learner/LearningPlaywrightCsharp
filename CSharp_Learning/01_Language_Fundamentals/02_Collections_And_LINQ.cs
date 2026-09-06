using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_Learning.LanguageFundamentals
{
    /// <summary>
    /// Demonstrates C# Array and List<T> collections, and Language Integrated Query (LINQ).
    /// 
    /// TS/JS Equivalent: 194.ts (Array.prototype.filter, map, arrow callbacks)
    /// </summary>
    public class CollectionsAndLinq
    {
        public static void RunDemo()
        {
            Console.WriteLine("=== C# Collections & LINQ Demo ===");

            // 1. Fixed-size Array and Dynamic List<T>
            int[] rawCodes = new int[] { 200, 201, 404, 500, 302, 403 };
            List<int> responseCodes = new List<int> { 200, 201, 404, 500, 302, 403 };

            // 2. LINQ Where (Equivalent to JS .filter())
            // In C#, LINQ uses deferred execution; ToList() materializes the result
            List<int> failedCodes = responseCodes
                .Where(code => code >= 400)
                .OrderBy(code => code)
                .ToList();

            // 3. LINQ Select (Equivalent to JS .map())
            List<string> formattedErrors = failedCodes
                .Select(code => $"[HTTP_ERR] Status Code: {code}")
                .ToList();

            Console.WriteLine("All Response Codes: " + string.Join(", ", responseCodes));
            Console.WriteLine("Filtered Failed Codes: " + string.Join(", ", failedCodes));
            Console.WriteLine("Formatted Error Messages:");
            foreach (var msg in formattedErrors)
            {
                Console.WriteLine("  " + msg);
            }
        }
    }
}
