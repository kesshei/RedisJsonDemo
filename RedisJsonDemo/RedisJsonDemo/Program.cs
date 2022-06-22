using Newtonsoft.Json;
using NReJSON;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace RedisJsonDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "RedisJson Demo by 蓝创精英团队";
            //创建redis对象
            var db = ConnectionMultiplexer.Connect("127.0.0.1").GetDatabase();
            var key = "test";
            string json = JsonConvert.SerializeObject(new UserInfo() { Age = 19, Name = "张三", Time = DateTime.Now, Address = new Address() { Name = "北京" } }, new JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd HH:mm:ss" });
            OperationResult result = await db.JsonSetAsync(key, json);
            if (result.IsSuccess)
            {
                Console.WriteLine("json保存成功!");
            }
            RedisResult result2 = await db.JsonGetAsync(key, ".");
            if (!result2.IsNull)
            {
                Console.WriteLine($"获取成功：{result2}");
            }
            OperationResult result3 = await db.JsonSetAsync(key, JsonConvert.SerializeObject("成都"), ".Address.Name");
            OperationResult result4 = await db.JsonSetAsync(key, JsonConvert.SerializeObject("王五"), ".Name");
            if (result3.IsSuccess && result4.IsSuccess)
            {
                Console.WriteLine("json修改成功!");
            }
            RedisResult result5 = await db.JsonGetAsync(key, ".Name", ".Age", ".Time", ".Address.Name");
            if (!result5.IsNull)
            {
                Console.WriteLine($"获取成功:{result5}");
            }
            Console.WriteLine("redis json 测试!");
            Console.ReadLine();
        }
    }
    public class UserInfo
    {
        public int Age { get; set; }
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public object Address { get; set; }
    }
    public class Address
    {
        public string Name { get; set; }
    }
}
