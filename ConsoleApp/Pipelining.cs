using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class Pipelining
    {
        private readonly IRedisConfig Redis;
        public Pipelining(IRedisConfig config) 
        {
            this.Redis = config;
        }

        public async Task Run()
        {
            /* Enviando comandos de forma Serial */
            var stopwatch = Stopwatch.StartNew();

            for (var i = 0; i < 1000; i++)
            {
                await Redis.DB.PingAsync();
            }
            Console.WriteLine($"1000 un-pipelined ping took {stopwatch.ElapsedMilliseconds} ms to execute");


            /* Enviando comandos de forma Paralela (default) */
            var pingTasks = new List<Task<TimeSpan>>();
            stopwatch.Restart();

            for (var i = 0; i < 1000; i++)
            {
                pingTasks.Add(Redis.DB.PingAsync());
            }
            await Task.WhenAll(pingTasks);

            Console.WriteLine($"1000 automatically pipelined ping took {stopwatch.ElapsedMilliseconds} ms to execute");


            /* Enviando todos os comandos em lote (+ rápido) */
            pingTasks.Clear();
            var batch = Redis.DB.CreateBatch();
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
