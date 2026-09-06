using System;
using System.Collections.Generic;

namespace CSharp_Learning.AbstractionsAndInterfaces
{
    /// <summary>
    /// Demonstrates C# Interfaces, Class Implementation, and C# Indexers.
    /// 
    /// TS/JS Equivalent: 207_REAL.ts (TestConfig), 208_CLASS_REAL.ts (class TestCase implements Executable), 
    /// 209_Interface_Mics.ts (Index Signatures [key: string]: string)
    /// </summary>
    public class InterfacesAndIndexers
    {
        // 1. Interface for Test Configuration (Equivalent to 207_REAL.ts)
        public interface ITestConfig
        {
            string Browser { get; }
            bool Headless { get; }
            string BaseUrl { get; }
            int? Timeout { get; }  // Nullable int represents optional property (timeout?: number)
            int? Retries { get; }  // Nullable int represents optional property (retries?: number)
        }

        public class LocalConfig : ITestConfig
        {
            public string Browser => "Chrome";
            public bool Headless => true;
            public string BaseUrl => "https://staging.app.com";
            public int? Timeout => null; // Omitted / Default
            public int? Retries => null; // Omitted / Default
        }

        public class CiConfig : ITestConfig
        {
            public string Browser => "Firefox";
            public bool Headless => false;
            public string BaseUrl => "http://localhost:3000";
            public int? Timeout => 10000;
            public int? Retries => 3;
        }

        // 2. Executable Contract (Equivalent to 208_CLASS_REAL.ts)
        public interface IExecutable
        {
            string Name { get; }
            void Run();
            string GetStatus();
        }

        public class TestCase : IExecutable
        {
            public string Name { get; }

            public TestCase(string name)
            {
                Name = name;
            }

            public void Run()
            {
                Console.WriteLine($"[RUN] {Name}");
            }

            public string GetStatus()
            {
                return "PASS";
            }
        }

        // 3. C# Indexer: Equivalent to TypeScript Index Signature '[key: string]: string' (209_Interface_Mics.ts)
        public class HeaderDictionary
        {
            private readonly Dictionary<string, string> _headers = new();

            // Indexer syntax in C#: public ReturnType this[KeyType key]
            public string this[string key]
            {
                get => _headers.TryGetValue(key, out var val) ? val : string.Empty;
                set => _headers[key] = value;
            }
        }

        public static void RunDemo()
        {
            Console.WriteLine("=== C# Interfaces & Indexers Demo ===");

            ITestConfig local = new LocalConfig();
            ITestConfig ci = new CiConfig();
            Console.WriteLine($"Local: {local.Browser} | Timeout: {local.Timeout?.ToString() ?? "default"}");
            Console.WriteLine($"CI: {ci.Browser} | Timeout: {ci.Timeout?.ToString() ?? "default"}");

            IExecutable test = new TestCase("User Login Verification");
            test.Run();
            Console.WriteLine("Status: " + test.GetStatus());

            // Using Indexer
            var headers = new HeaderDictionary();
            headers["Content-Type"] = "application/json";
            headers["Authorization"] = "Bearer token_secret_456";
            Console.WriteLine("Auth Header from Indexer: " + headers["Authorization"]);
        }
    }
}
