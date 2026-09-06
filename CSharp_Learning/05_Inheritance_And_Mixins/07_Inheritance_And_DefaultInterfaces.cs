using System;

namespace CSharp_Learning.InheritanceAndMixins
{
    /// <summary>
    /// Demonstrates Single Class Inheritance, Constructor Chaining via ': base()',
    /// Multi-tier base method delegation, and Default Interface Methods (C# Mixin equivalent).
    /// 
    /// TS/JS Equivalent: 22_chapter_Inheritance (181 to 191), ex5.JS (Multi-tier super calls)
    /// </summary>
    public class InheritanceAndDefaultInterfaces
    {
        // 1. Single Inheritance Base Page Object
        public class BasePage
        {
            public string PageName { get; protected set; }

            public BasePage(string pageName)
            {
                PageName = pageName;
            }

            public virtual void Open()
            {
                Console.WriteLine($"[OPEN] Navigating to page: {PageName}");
            }
        }

        // Multi-level inheritance: BasePage -> AuthPage -> AdminPage
        public class AuthPage : BasePage
        {
            public AuthPage(string pageName) : base(pageName)
            {
            }

            public virtual void Login(string user)
            {
                Console.WriteLine($"[LOGIN] Authenticating user: {user}");
            }
        }

        public class AdminPage : AuthPage
        {
            public AdminPage() : base("Admin Dashboard")
            {
            }

            public void ManageUsers()
            {
                Console.WriteLine("[ADMIN] Managing user permissions");
            }
        }

        // 2. Multi-tier Base Delegation (Equivalent to ex5.JS: C -> B -> A)
        public class LevelA
        {
            public virtual string Who() => "A";
        }

        public class LevelB : LevelA
        {
            public override string Who() => "B>" + base.Who();
        }

        public class LevelC : LevelB
        {
            public override string Who() => "C>" + base.Who();
        }

        // 3. Simulating Mixins using C# 8+ Default Interface Methods (Equivalent to 189.js)
        public interface ILoggerMixin
        {
            void Log(string message)
            {
                Console.WriteLine($"[LOG] {message}");
            }
        }

        public interface IScreenshotMixin
        {
            void TakeScreenshot()
            {
                Console.WriteLine("[SCREENSHOT] Captured view state artifact");
            }
        }

        // Multiple interface inheritance enables composition of multiple behaviors!
        public class SmartTestCase : BasePage, ILoggerMixin, IScreenshotMixin
        {
            public SmartTestCase(string name) : base(name)
            {
            }
        }

        public static void RunDemo()
        {
            Console.WriteLine("=== C# Inheritance & Mixins Demo ===");

            var admin = new AdminPage();
            admin.Open();
            admin.Login("superadmin");
            admin.ManageUsers();

            // Multi-tier base call
            var chain = new LevelC();
            Console.WriteLine("Multi-tier Delegation Result: " + chain.Who()); // "C>B>A"

            // Using Default Interface Mixin
            var smartTest = new SmartTestCase("Payment Verification Test");
            smartTest.Open();
            ((ILoggerMixin)smartTest).Log("Transaction initiated");
            ((IScreenshotMixin)smartTest).TakeScreenshot();
        }
    }
}
