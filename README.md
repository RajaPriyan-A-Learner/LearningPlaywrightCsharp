# LearningPlaywrightCsharp 🎭⚡

> **The Enterprise C# (.NET 8+) Playwright Test Automation & Object-Oriented Mastery Framework**
> 
> A comprehensive, production-grade guide translating modern JavaScript/TypeScript automation concepts into idiomatic C# (.NET) architecture.

---

## 🌟 Highlights & Architecture Overview

`LearningPlaywrightCsharp` provides a complete, side-by-side architectural blueprint for QA Engineers, SDETs, and Software Developers mastering **Playwright for .NET** and modern C# (.NET 8+).

```
+------------------------------------+------------------------------------+
|      JavaScript / TypeScript       |             C# (.NET 8+)           |
+------------------------------------+------------------------------------+
| V8 / Node.js Engine                | Common Language Runtime (CoreCLR)  |
| Single-Threaded Event Loop         | Multi-Threaded ThreadPool & Tasks  |
| Structural Typing (Duck Typing)    | Nominal Typing & Reified Generics  |
| Type Erasure at Runtime            | Runtime Type Metadata (typeof(T))  |
| camelCase Async API (page.goto)    | PascalCase Async (Page.GotoAsync)  |
| Number / Undefined Dualities       | Strict Numeric Sizing & Nullable T?|
+------------------------------------+------------------------------------+
```

---

## 📂 Repository Directory Structure

```
LearningPlaywrightCsharp/
│
├── CSharp_Learning/                         # Executable C# (.NET) Source Code
│   ├── 01_Language_Fundamentals/
│   │   ├── 01_Primitives_And_Nullability.cs # Primitives (int, long, decimal) & Nullable Reference Types (T?)
│   │   └── 02_Collections_And_LINQ.cs       # Arrays, List<T>, and Deferred LINQ Querying (.Where, .Select)
│   ├── 02_Methods_And_Delegates/
│   │   ├── 03_Methods_And_Overloading.cs    # Compile-time Method Overloading & Named Arguments
│   │   └── 04_Delegates_And_Lambdas.cs      # Action<T>, Func<T, R>, and Retry Polling Lambdas
│   ├── 03_Async_Task_Concurrency/
│   │   └── 05_Task_Async_Await.cs           # Task, Task<T>, Task.WhenAll & ThreadPool Concurrency
│   ├── 04_Classes_And_Encapsulation/
│   │   └── 06_Class_Properties_Constructors.cs # Auto-properties, init, Constructor Chaining & Fluent APIs
│   ├── 05_Inheritance_And_Mixins/
│   │   └── 07_Inheritance_And_DefaultInterfaces.cs # Constructor Forwarding (: base) & Default Interfaces (Mixins)
│   ├── 06_Polymorphism/
│   │   └── 08_Polymorphism_Virtual_Override.cs # Explicit virtual/override Dynamic Subtype Polymorphism
│   ├── 07_Abstractions_And_Interfaces/
│   │   ├── 09_Interfaces_And_Indexers.cs    # Nominal Interfaces & Custom Indexers (this[key])
│   │   └── 10_Enums_And_Abstract_Classes.cs # Strongly-typed Enums & Abstract Template Method Pattern
│   ├── 08_Generics/
│   │   ├── 11_Generic_Methods_And_Classes.cs # Reified Generics (typeof(T)) & where Type Constraints
│   │   └── 12_Generic_API_Envelope.cs       # ApiResponse<T> Pattern & System.Text.Json Deserialization
│   ├── 09_Access_Modifiers/
│   │   └── 13_Access_Modifiers_And_Readonly.cs # CLR Encapsulation: private readonly, internal & protected
│   └── 10_Playwright_POM_Framework/
│       └── 14_Playwright_Page_Object_Model.cs # Production POM: Lazy Locators, PascalCase Async & Fluent Flows
│
└── IQ_Notes_CSharp/                         # In-Depth Interview & Architectural Notes
    ├── MASTER_CSharp_Architecture_IQ.md     # 9-Section Master Architecture Reference Guide
    ├── 01_Primitives_And_Nullability_IQ.md
    ├── 02_Collections_And_LINQ_IQ.md
    ├── 03_Methods_And_Overloading_IQ.md
    ├── 04_Delegates_And_Lambdas_IQ.md
    ├── 05_Task_Async_Await_IQ.md
    ├── 06_Class_Properties_Constructors_IQ.md
    ├── 07_Inheritance_And_DefaultInterfaces_IQ.md
    ├── 08_Polymorphism_Virtual_Override_IQ.md
    ├── 09_Interfaces_And_Indexers_IQ.md
    ├── 10_Enums_And_Abstract_Classes_IQ.md
    ├── 11_Generic_Methods_And_Classes_IQ.md
    ├── 12_Generic_API_Envelope_IQ.md
    ├── 13_Access_Modifiers_And_Readonly_IQ.md
    └── 14_Playwright_Page_Object_Model_IQ.md
```

---

## 🗺️ TypeScript to C# Syntax & Architecture Mapping

| Concept | TypeScript / JavaScript | C# (.NET 8+) | Why It Matters in Test Automation |
| :--- | :--- | :--- | :--- |
| **Financial / Cart Assertions** | `number` (IEEE 754 float) | `decimal` (128-bit exact) | Zero floating-point rounding errors (`0.1 + 0.2 == 0.3m`) |
| **Absence of Value** | `null` & `undefined` | `null` & `T?` | Eliminates accidental uninitialized states; compiler catches CS8600 |
| **Collection Queries** | `.filter()`, `.map()`, `.find()` | LINQ: `.Where()`, `.Select()` | Deferred execution (`IEnumerable<T>`) optimizes memory allocation |
| **Method Overloading** | Ambient declarations + 1 body | True compile-time overloads | Multiple binary methods; zero runtime `typeof` branching |
| **Callbacks & Handlers** | Arrow functions `() => void` | `Action` / `Func<T, R>` | Type-safe CLR delegates with multicast execution |
| **Async Execution** | `Promise<T>` on Event Loop | `Task<T>` on ThreadPool | Multi-threaded multicore execution with `CancellationToken` |
| **Subtype Polymorphism** | Virtual by default | `virtual` (base) + `override` (child) | Explicit dispatch intent prevents accidental method overrides |
| **Multiple Behavior (Mixins)**| Prototype composition | Default Interface Methods (DIM) | Interface-based behavior composition without inheritance coupling |
| **Generics** | Type Erasure (Compile-only) | Reified Generics (`typeof(T)`) | Runtime type inspection and specialized native machine code |
| **Encapsulation** | Compile-time `private` | CLR-enforced `private readonly` | Locators cannot be accessed or reassigned by test scripts |
| **Playwright Actions** | `await page.goto(url)` | `await Page.GotoAsync(url)` | Standard .NET asynchronous naming convention |

---

## 🎭 Playwright for .NET: Page Object Model Example

```csharp
using Microsoft.Playwright;

namespace AutomationFramework.Pages;

public abstract class BasePage
{
    protected readonly IPage Page;
    protected BasePage(IPage page) => Page = page ?? throw new ArgumentNullException(nameof(page));
}

public class LoginPage : BasePage
{
    // Lazy locators: Evaluated dynamically when accessed to utilize Playwright auto-waiting
    private ILocator UsernameInput => Page.Locator("#user-name");
    private ILocator PasswordInput => Page.Locator("#password");
    private ILocator LoginButton => Page.Locator("#login-button");
    private ILocator ErrorBanner => Page.Locator("[data-test='error']");

    public LoginPage(IPage page) : base(page) { }

    public async Task OpenAsync(string baseUrl) => await Page.GotoAsync($"{baseUrl}/login");

    // Fluent method returning the next strongly-typed Page Object
    public async Task<DashboardPage> LoginAsValidUserAsync(string username, string password)
    {
        await UsernameInput.FillAsync(username);
        await PasswordInput.FillAsync(password);
        await LoginButton.ClickAsync();
        return new DashboardPage(Page);
    }

    public async Task<bool> HasErrorAsync() => await ErrorBanner.IsVisibleAsync();
}
```

---

## 🚀 Getting Started

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- PowerShell 7+ or any terminal
- Visual Studio 2022, JetBrains Rider, or VS Code with C# Dev Kit

### Cloning the Repository
```bash
git clone https://github.com/RajaPriyan-A-Learner/LearningPlaywrightCsharp.git
cd LearningPlaywrightCsharp
```

---

## 📚 Master Reference Guide
For the complete 9-section master guide detailing Node.js vs CLR internals, concurrency models, reified generics, and architectural design patterns, see:
👉 [IQ_Notes_CSharp/MASTER_CSharp_Architecture_IQ.md](IQ_Notes_CSharp/MASTER_CSharp_Architecture_IQ.md)
