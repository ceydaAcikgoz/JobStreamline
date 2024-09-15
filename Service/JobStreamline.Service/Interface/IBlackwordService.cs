namespace JobStreamline.Service;

public interface IBlackwordService
{
    void AddBlackword(string Word);
    bool IsWordBlacklisted(string Word);
    void RemoveWordFromBlackword(string Word);
    Task<Dictionary<string, string>> AllBlackword();
}
