using StackExchange.Redis;
using System;

public interface IRedisService
{
    void Add(string Key, string Value);
    void Remove(string Key, string Value);
    bool IsListed(string Key, string Value);
    bool ContainsListed(string Key, string Value);
    Task<Dictionary<string, string>> GetAll(string Pattern);
}