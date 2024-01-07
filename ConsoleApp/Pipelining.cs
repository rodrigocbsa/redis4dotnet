using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public static class Pipelining
    {
        static private ConfigurationOptions? Options;
        static private ConnectionMultiplexer? redis;
        public static async Task Main()
        {
            Options = new ConfigurationOptions()
            {
                EndPoints = { Environment.GetEnvironmentVariable("REDIS_ENDPOINT") },
                Password = Environment.GetEnvironmentVariable("REDIS_PASSWD"),
                SslClientAuthenticationOptions = new Func<string, System.Net.Security.SslClientAuthenticationOptions>(hostName => new System.Net.Security.SslClientAuthenticationOptions
                {
                    EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls13
                })
            };
            redis = ConnectionMultiplexer.Connect(Options);

            var db = redis.GetDatabase();


            /* Enviando comandos de forma Serial */
            var stopwatch = Stopwatch.StartNew();

            for (var i = 0; i < 1000; i++)
            {
                await db.PingAsync();
            }
            Console.WriteLine($"1000 un-pipelined ping took {stopwatch.ElapsedMilliseconds} ms to execute");


            /* Enviando comandos de forma Paralela (default) */
            var pingTasks = new List<Task<TimeSpan>>();
            stopwatch.Restart();

            for (var i = 0; i < 1000; i++)
            {
                pingTasks.Add(db.PingAsync());
            }
            await Task.WhenAll(pingTasks);

            Console.WriteLine($"1000 automatically pipelined ping took {stopwatch.ElapsedMilliseconds} ms to execute");


            /* Enviando todos os comandos em lote (+ rápido) */
            pingTasks.Clear();
            var batch = db.CreateBatch();
            stopwatch.Restart();
            for (var i = 0; i < 1000; i++)
            {
                pingTasks.Add(batch.PingAsync());
            }
            batch.Execute();
            await Task.WhenAll(pingTasks);
            Console.WriteLine($"1000 batched ping took {stopwatch.ElapsedMilliseconds} ms to execute");
        }
    }
}
