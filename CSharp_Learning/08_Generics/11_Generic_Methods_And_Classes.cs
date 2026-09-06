// ============================================================================
// C# Learning Framework - Chapter 08: Generic Methods and Classes
// Equivalent to TypeScript: Generics <T>, Generic Functions, Constraints
// ============================================================================

namespace CSharpLearning.Generics;

/// <summary>
/// Sample user account model stored in generic test data collections.
/// </summary>
public record UserAccount(int Id, string Username, string Role);

/// <summary>
/// Sample environment configuration model.
/// </summary>
public record EnvironmentConfig(string BaseUrl, int TimeoutMs, bool Headless);

/// <summary>
/// Generic repository class demonstrating type parameters with constraints.
/// 
/// Key Difference from TypeScript:
/// - TypeScript: Generics are erased at compile time (Type Erasure). At runtime, JavaScript
///   has no knowledge of <T>.
/// - C#: Generics are REIFIED (first-class runtime constructs). The CLR creates specialized
///   types for value types and shares code for reference types, preserving typeof(T) at runtime.
/// </summary>
/// <typeparam name="T">Type of entity stored, constrained to reference types with a parameterless constructor or record.</typeparam>
public class TestDataStore<T> where T : class
{
    private readonly List<T> _items = new();

    public void Add(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        _items.Add(item);
        Console.WriteLine($"[TestDataStore<{typeof(T).Name}>] Added entity of type '{typeof(T).FullName}'. Current Count: {_items.Count}");
    }

    public IReadOnlyList<T> GetAll() => _items.AsReadOnly();

    public T? Find(Func<T, bool> predicate)
    {
        return _items.FirstOrDefault(predicate);
    }

    public int Count => _items.Count;
}

/// <summary>
/// Generic utility demonstrating generic static methods and type constraints.
/// </summary>
public static class TestDataUtility
{
    /// <summary>
    /// Generic method returning the first element or a default value.
    /// Demonstrates method-level generic parameter <T>.
    /// </summary>
    public static T GetFirstOrThrow<T>(IEnumerable<T> collection, string errorMessage)
    {
        var first = collection.FirstOrDefault();
        if (first is null)
        {
            throw new InvalidOperationException(errorMessage);
        }
        return first;
    }

    /// <summary>
    /// Generic method demonstrating multiple type constraints:
    /// - where TEntity : class (must be a reference type)
    /// - where TId : struct (must be a value type, e.g., int, Guid)
    /// </summary>
    public static void PrintMetadata<TEntity, TId>(TEntity entity, TId identifier)
        where TEntity : class
        where TId : struct
    {
        Console.WriteLine($"[Metadata] Entity Type: {typeof(TEntity).Name} | ID Type: {typeof(TId).Name} | ID Value: {identifier}");
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== C# Generic Methods & Classes (Reified Generics) ===");

        // 1. Generic Class instantiation with Record UserAccount
        var userStore = new TestDataStore<UserAccount>();
        userStore.Add(new UserAccount(1, "qa_admin", "SuperUser"));
        userStore.Add(new UserAccount(2, "qa_viewer", "ReadOnly"));

        var admin = userStore.Find(u => u.Role == "SuperUser");
        Console.WriteLine($"Found admin: {admin?.Username} (Role: {admin?.Role})");

        // 2. Generic Class instantiation with EnvironmentConfig
        var configStore = new TestDataStore<EnvironmentConfig>();
        configStore.Add(new EnvironmentConfig("https://staging.example.com", 30000, true));

        // 3. Generic Method invocation with type inference
        var firstUser = TestDataUtility.GetFirstOrThrow(userStore.GetAll(), "No users found in store!");
        Console.WriteLine($"First user in store: {firstUser.Username}");

        // 4. Multiple constraints demo
        TestDataUtility.PrintMetadata(firstUser, firstUser.Id);

        // 5. Runtime type introspection (Impossible in JavaScript/TypeScript due to Type Erasure!)
        Console.WriteLine($"\nCLR Runtime Type Check: userStore is TestDataStore<UserAccount> -> {userStore is TestDataStore<UserAccount>}");
        Console.WriteLine($"CLR Type definition: {typeof(TestDataStore<>).GetGenericArguments().Length} generic parameter(s)");
    }
}
