using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestApiConsoleApp
{
    class Program
    {
        private static int numberOfIteration = 50; 
        private static int milliseconds = 10000; 
        private static int millisecondsAssert = 1000;

        static async Task Main(string[] args)
        {

            HttpClient client =  new HttpClient();

            //Console.WriteLine("GetWait");
            //await GetWait(client);
            //Console.WriteLine();

            //Console.WriteLine("GetAwait");
            //await GetAwait(client);
            //Console.WriteLine();

            //Console.WriteLine("GetWaitWithAssert");
            //await GetWaitWithAssert(client);
            //Console.WriteLine();

            Console.WriteLine("GetAwaitAssert");
            await GetAwaitWithAssert(client);
            Console.WriteLine();

            //Console.WriteLine("GetWhenAll");
            //await GetWhenAll(client);
            //Console.WriteLine();

            //Console.WriteLine("GetWaitAll");
            //await GetWaitAll(client);
            //Console.WriteLine();

            //Console.WriteLine("GetWhenAllWithAssert");
            //await GetWhenAllWithAssert(client);
            //Console.WriteLine();

            //Console.WriteLine("GetWaitAllWithAssert");
            //await GetWaitAllWithAssert(client);
            //Console.WriteLine();

            Console.ReadKey();
        }

        private static async Task GetAwait(HttpClient client)
        {
            List<Task<HttpResponseMessage>> tasks = new List<Task<HttpResponseMessage>>();
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            for (int i = 0; i < numberOfIteration; i++)
            {
                tasks.Add(client.GetAsync($"https://localhost:5001/WeatherForecast/milliseconds/{milliseconds}/get-await"));
            }
            await Task.WhenAll(tasks);

            stopWatch.Stop();
            string successfullMsg = tasks.Select(t => t.Result.IsSuccessStatusCode)
                                     .All(x => x)
                                     ? "Yes"
                                     : "No";

            Console.WriteLine($"Time elapsed {stopWatch.Elapsed}");
            Console.WriteLine($"Were all calls successfull: {successfullMsg}");
        }

        private static async Task GetWait(HttpClient client)
        {
            List<Task<HttpResponseMessage>> tasks = new List<Task<HttpResponseMessage>>();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            for (int i = 0; i < numberOfIteration; i++)
            {
                tasks.Add(client.GetAsync($"https://localhost:5001/WeatherForecast/milliseconds/{milliseconds}/get-wait"));
            }
            await Task.WhenAll(tasks);

            stopWatch.Stop();
            string successfullMsg = tasks.Select(t => t.Result.IsSuccessStatusCode)
                                      .All(x => x)
                                      ? "Yes"
                                      : "No";

            Console.WriteLine($"Time elapsed {stopWatch.Elapsed}");
            Console.WriteLine($"Were all calls successfull: {successfullMsg}");
        }

        private static async Task GetAwaitWithAssert(HttpClient client)
        {
            List<Task<HttpResponseMessage>> tasks = new List<Task<HttpResponseMessage>>();

            for (int i = 0; i < numberOfIteration; i++)
            {
                tasks.Add(client.GetAsync($"https://localhost:5001/WeatherForecast/milliseconds/{millisecondsAssert}/get-await"));
            }

            await Assert(tasks);
        }

        private static async Task GetWaitWithAssert(HttpClient client)
        {
            List<Task<HttpResponseMessage>> tasks = new List<Task<HttpResponseMessage>>();

            for (int i = 0; i < numberOfIteration; i++)
            {
                tasks.Add(client.GetAsync($"https://localhost:5001/WeatherForecast/milliseconds/{millisecondsAssert}/get-wait"));
            }

            await Assert(tasks);
        }

        private static async Task GetWhenAll(HttpClient client)
        {
            List<Task<HttpResponseMessage>> tasks = new List<Task<HttpResponseMessage>>();
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            for (int i = 0; i < numberOfIteration; i++)
            {
                tasks.Add(client.GetAsync($"https://localhost:5001/WeatherForecast/milliseconds/{milliseconds}/number-of-iteration/20/get-when-all"));
            }
            await Task.WhenAll(tasks);

            stopWatch.Stop();
            Console.WriteLine($"Time elapsed {stopWatch.Elapsed}");
        }

        private static async Task GetWaitAll(HttpClient client)
        {
            List<Task<HttpResponseMessage>> tasks = new List<Task<HttpResponseMessage>>();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            for (int i = 0; i < numberOfIteration; i++)
            {
                tasks.Add(client.GetAsync($"https://localhost:5001/WeatherForecast/milliseconds/{milliseconds}/number-of-iteration/20/get-wait-all"));
            }
            await Task.WhenAll(tasks);

            stopWatch.Stop();
            Console.WriteLine($"Time elapsed {stopWatch.Elapsed}");
        }

        private static async Task GetWhenAllWithAssert(HttpClient client)
        {
            List<Task<HttpResponseMessage>> tasks = new List<Task<HttpResponseMessage>>();

            for (int i = 0; i < numberOfIteration; i++)
            {
                tasks.Add(client.GetAsync($"https://localhost:5001/WeatherForecast/milliseconds/{milliseconds}/number-of-iteration/20/get-when-all"));
            }
            await Task.WhenAll(tasks);

            await Assert(tasks);
        }

        private static async Task GetWaitAllWithAssert(HttpClient client)
        {
            List<Task<HttpResponseMessage>> tasks = new List<Task<HttpResponseMessage>>();

            for (int i = 0; i < numberOfIteration; i++)
            {
                tasks.Add(client.GetAsync($"https://localhost:5001/WeatherForecast/milliseconds/{milliseconds}/number-of-iteration/20/get-wait-all"));
            }
            await Task.WhenAll(tasks);

            await Assert(tasks);
        }

        private static async Task Assert(List<Task<HttpResponseMessage>> tasks)
        {
            int countSame = 0;
            int countDifferent = 0;

            foreach (var task in tasks)
            {
                var body = await task.Result.Content.ReadAsStringAsync();
                var areSameThread = JsonConvert.DeserializeObject<bool>(body);

                if (areSameThread)
                    countSame++;
                else
                    countDifferent++;

            }

            Console.WriteLine($"Number of time the thread was the same before and after the task was executed {countSame}");
            Console.WriteLine($"Number of time the thread was not the same before and after the task was executed {countDifferent}");
            Console.WriteLine($"Was it always the same thread before and after the task was executed {countDifferent == 0}");
        }
    }
}
