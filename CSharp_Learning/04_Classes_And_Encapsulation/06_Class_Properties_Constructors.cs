using System;

namespace CSharp_Learning.ClassesAndEncapsulation
{
    /// <summary>
    /// Demonstrates C# Class Properties, Encapsulation, Constructor Chaining, and Fluent Method Chaining.
    /// 
    /// TS/JS Equivalent: 20_chapter_Class_objects, 21_chapter_Encapsulation, 
    /// EX1.JS (Bug), EX2.js (Environment), EX4.js (Fluent Counter)
    /// </summary>
    public class ClassPropertiesConstructors
    {
        // 1. Defect entity class (Equivalent to EX1.JS)
        public class BugReport
        {
            // Auto-implemented Properties with private setters
            public string Title { get; private set; }
            public string Severity { get; private set; }

            public BugReport(string title, string severity)
            {
                Title = title;
                Severity = severity;
            }

            public void Display()
            {
                Console.WriteLine($"[{Severity}] {Title}");
            }
        }

        // 2. Environment class with Constructor Chaining via ': this()' (Equivalent to EX2.js)
        public class EnvironmentConfig
        {
            public string Name { get; set; }
            public int Port { get; set; }

            // Default constructor chaining to parameterized constructor
            public EnvironmentConfig() : this("staging", 3000)
            {
            }

            public EnvironmentConfig(string name, int port)
            {
                Name = name;
                Port = port;
            }

            public string GetUrl()
            {
                return $"http://{Name}:{Port}";
            }
        }

        // 3. Fluent Counter with Method Chaining (Equivalent to EX4.js)
        public class FluentCounter
        {
            public int Count { get; private set; } = 0;

            public FluentCounter Increment()
            {
                Count++;
                return this; // Enables .Increment().Increment().Display() chaining
            }

            public FluentCounter Display()
            {
                Console.WriteLine($"Fluent Count: {Count}");
                return this;
            }
        }

        public static void RunDemo()
        {
            Console.WriteLine("=== C# Classes, Encapsulation & Fluent Chaining Demo ===");

            var bug = new BugReport("NullReferenceException on Checkout", "Critical");
            bug.Display();

            var env1 = new EnvironmentConfig();
            var env2 = new EnvironmentConfig("production", 8080);
            Console.WriteLine("Default Env: " + env1.GetUrl());
            Console.WriteLine("Production Env: " + env2.GetUrl());

            new FluentCounter().Increment().Increment().Increment().Display();
        }
    }
}
