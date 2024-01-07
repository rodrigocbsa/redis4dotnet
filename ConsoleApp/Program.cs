using ConsoleApp;

RedisConfig redisConfig = new RedisConfig(); // É estudo mas é bem feito kkk

/* Pipelining tutorial 
Pipelining pipe = new(redisConfig);
await pipe.Run();
*/

/* Dados */
DataStructures data = new(redisConfig);
await data.Run();
