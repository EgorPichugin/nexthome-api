using MediatR;
using NextHome.QdrantService;

namespace NextHome.Application.Qdrant.Queries;

/// <summary>
/// Query to get list of collections from Qdrant.
/// </summary>
public record GetCollectionListQuery : IRequest<List<string>>;

/// <summary>
/// Handler to get list of collection from Qdrant.
/// </summary>
/// <param name="qdrantService">The service is responsible for managing Qdrant requests.</param>
public class GetCollectionListHandler(IQdrantService qdrantService)
    : IRequestHandler<GetCollectionListQuery, List<string>>
{
    /// <inheridoc/>
    public async Task<List<string>> Handle(GetCollectionListQuery request, CancellationToken cancellationToken)
    {
        return await qdrantService.GetCollectionList() ?? [];
    }
}