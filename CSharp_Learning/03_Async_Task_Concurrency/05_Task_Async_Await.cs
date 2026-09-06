using System;
using System.Threading.Tasks;

namespace CSharp_Learning.AsyncTaskConcurrency
{
    /// <summary>
    /// Demonstrates C# Task-based Asynchronous Pattern (TAP) using async and await.
    /// 
    /// TS/JS Equivalent: 17_chapter_Promise, 18_chapter_Async_Await (Promise, Promise.all)
    /// </summary>
    public class TaskAsyncAwait
    {
        // 1. Simulating asynchronous network fetch (Equivalent to JS Promise returning fetch)
        public static async Task<string> FetchUserDataAsync(int userId)
        {
            // Task.Delay simulates non-blocking I/O (Equivalent to setTimeout / await new Promise)
            await Task.Delay(200);
            return $"{{\"id\": {userId}, \"name\": \"User_{userId}\", \"role\": \"QA\"}}";
        }

        // 2. Void-equivalent async task (Task without generic argument represents Promise<void>)
        public static async Task NavigatePageAsync(string url)
        {
            await Task.Delay(150);
            Console.WriteLine($"[NAVIGATION] Completed loading: {url}");
        }

        // 3. Parallel async orchestration (Equivalent to Promise.all in JavaScript)
        public static async Task RunParallelSuiteAsync()
        {
            Console.WriteLine("Dispatching parallel async requests...");

            Task<string> user1Task = FetchUserDataAsync(101);
            Task<string> user2Task = FetchUserDataAsync(102);

            // WhenAll awaits multiple tasks in parallel
            string[] results = await Task.WhenAll(user1Task, user2Task);

            Console.WriteLine("Received Parallel Responses:");
            foreach (var res in results)
            {
                Console.WriteLine("  " + res);
            }
        }

        public static async Task RunDemoAsync()
        {
            Console.WriteLine("=== C# Task & Async/Await Concurrency Demo ===");
            await NavigatePageAsync("https://staging.app.com/dashboard");
            await RunParallelSuiteAsync();
        }
    }
}
