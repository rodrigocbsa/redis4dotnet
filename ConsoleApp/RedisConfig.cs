using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class RedisConfig : IRedisConfig
    {
        public ConfigurationOptions Options {  get; set; }
        public ConnectionMultiplexer Redis {  get; set; }
        public IDatabase DB {  get; set; }

        public RedisConfig() 
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
            Redis = ConnectionMultiplexer.Connect(Options);
            DB = Redis.GetDatabase();
        }
    }
}
