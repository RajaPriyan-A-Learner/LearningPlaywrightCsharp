using System;
using System.Collections.Generic;

namespace CSharp_Learning.Polymorphism
{
    /// <summary>
    /// Demonstrates Runtime Polymorphism with 'virtual' and 'override' in C#.
    /// 
    /// TS/JS Equivalent: 23_chapter_Polymorphism (192.js - BaseTest and APIPage setup overriding)
    /// In C#, method overriding requires explicit opt-in: 'virtual' on the base class, 
    /// and 'override' on the derived class, unlike JavaScript where any prototype method can be shadowed.
    /// </summary>
    public class PolymorphismVirtualOverride
    {
        // Base Test Fixture with virtual lifecycle hooks
        public class BaseTest
        {
            public virtual void Setup()
            {
                Console.WriteLine("BaseTest: Initialize Chromium browser context");
            }

            public virtual void Teardown()
            {
                Console.WriteLine("BaseTest: Dispose browser context");
            }
        }

        // Overriding setup for specialized API testing (Equivalent to 192.js)
        public class ApiPageTest : BaseTest
        {
            public override void Setup()
            {
                Console.WriteLine("ApiPageTest: Initialize REST client and bearer token headers");
            }
        }

        // Overriding setup for Mobile Emulation
        public class MobilePageTest : BaseTest
        {
            public override void Setup()
            {
                base.Setup(); // Calls base browser launch, then specializes
                Console.WriteLine("MobilePageTest: Emulate Pixel 7 viewport and touch gestures");
            }
        }

        public static void RunDemo()
        {
            Console.WriteLine("=== C# Polymorphism (virtual/override) Demo ===");

            BaseTest standardTest = new BaseTest();
            BaseTest apiTest = new ApiPageTest();
            BaseTest mobileTest = new MobilePageTest();

            Console.WriteLine("--- Direct Invocations ---");
            standardTest.Setup();
            apiTest.Setup();
            mobileTest.Setup();

            Console.WriteLine("\n--- Polymorphic Collection Execution ---");
            // A single loop executes polymorphic methods across diverse test runners
            List<BaseTest> testSuite = new List<BaseTest> { standardTest, apiTest, mobileTest };
            foreach (var test in testSuite)
            {
                test.Setup();
                test.Teardown();
                Console.WriteLine("---");
            }
        }
    }
}
