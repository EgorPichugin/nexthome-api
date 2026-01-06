using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace NextHome.QdrantService;

public interface IQdrantService
{
    // public QdrantOptions Options { get; }
}

public sealed class QdrantService(IOptions<QdrantOptions> options) : IQdrantService
{
    
}