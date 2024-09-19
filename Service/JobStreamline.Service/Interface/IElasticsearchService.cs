using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.Configuration;

namespace JobStreamline.Service;

public interface IElasticsearchService
{
    public ElasticsearchClient Client { get; }
}