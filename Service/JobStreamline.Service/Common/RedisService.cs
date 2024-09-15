using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

public class RedisService : IRedisService
{
    private readonly IConnectionMultiplexer _iConnectionMultiplexer;
    private readonly IDatabase _database;
    private readonly IConfiguration _iConfiguration;


    public RedisService(IConfiguration Configuration,IConnectionMultiplexer ConnectionMultiplexer)
    {
        _iConnectionMultiplexer = ConnectionMultiplexer;
        _database = _iConnectionMultiplexer.GetDatabase();
        _iConfiguration= Configuration;
    }

    public void Add(string Key, string Value)
    {
        _database.SetAdd(Key, Value);
    }

    public bool ContainsListed(string Key, string Value)
    {
        var words = Value.Split(' '); // Basit bir kelime ayırma işlemi
        foreach (var word in words)
        {
            if (IsListed(Key, word))
            {
                return true; // Eğer yasaklı bir kelime varsa true döner
            }
        }
        return false; // Yasaklı kelime yoksa false döner
    }

    public bool IsListed(string Key, string Value)
    {
        return _database.SetContains(Key, Value);
    }

    public void Remove(string Key, string Value)
    {
        _database.SetRemove(Key, Value);
    }

    public async Task<Dictionary<string, string>> GetAll(string Pattern)
    {
        var server = _iConnectionMultiplexer.GetServer(_iConnectionMultiplexer.Configuration);
        IEnumerable<RedisKey> keys = server.Keys(pattern: Pattern); 
        var blacklist = new Dictionary<string, string>();
        foreach (var key in keys)
        {
            string value = await _database.StringGetAsync(key);
            blacklist.Add(key, value);
        }
        return blacklist;
    }

}
