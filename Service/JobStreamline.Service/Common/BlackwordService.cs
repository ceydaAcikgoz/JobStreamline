namespace JobStreamline.Service;

public class BlackwordService : IBlackwordService
{
    private readonly IRedisService _iRedisService;
    private readonly string _key = "blacklist";

    public BlackwordService(IRedisService RedisService)
    {
        _iRedisService = RedisService;
    }

    public void AddBlackword(string Word)
    {
        _iRedisService.Add(Word, Word);
    }

    public bool IsWordBlacklisted(string Word)
    {
        return _iRedisService.IsListed(_key, Word);
    }

    public void RemoveWordFromBlackword(string Word)
    {
        _iRedisService.Remove(Word, Word);
    }

    public async Task<Dictionary<string, string>> AllBlackword()
    {
        return await _iRedisService.GetAll(_key + ":*");
    }
}
