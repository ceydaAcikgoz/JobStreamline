using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.Configuration;

namespace JobStreamline.Service;

public class ElasticsearchService:IElasticsearchService
{
    private readonly ElasticsearchClient _client;

    public ElasticsearchService(IConfiguration configuration)
    {
        var settings = new ElasticsearchClientSettings(new Uri(configuration["Elasticsearch:Url"]!))
            .ServerCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors) => true)
            .DefaultIndex(configuration["Elasticsearch:DefaultIndex"]!).Authentication(new ApiKey(configuration["Elasticsearch:Apikey"]));

        _client = new ElasticsearchClient(settings);
    }

    public ElasticsearchClient Client => _client;
}