using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public interface IRedisConfig
    {
        ConfigurationOptions Options { get; set; }
        ConnectionMultiplexer Redis { get; set; }
        IDatabase DB { get; set; }
    }
}
