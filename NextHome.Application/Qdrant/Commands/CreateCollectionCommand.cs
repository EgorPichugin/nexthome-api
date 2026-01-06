using MediatR;
using NextHome.QdrantService;

namespace NextHome.Application.Qdrant.Commands;

/// <summary>
/// Command to create collection in Qdrant.
/// </summary>
/// <param name="CollectionName">Collection name.</param>
public record CreateCollectionCommand(
    string? CollectionName) : IRequest<Unit>;

/// <summary>
/// Handler to creat collection in Qdrant.
/// </summary>
/// <param name="qdrantService">Service that is responsible for managing Qdrant requests.</param>
public class CreateCollectionHandler(IQdrantService qdrantService) : IRequestHandler<CreateCollectionCommand, Unit>
{
    /// <inheritdoc />
    public async Task<Unit> Handle(CreateCollectionCommand request, CancellationToken cancellationToken)
    {
        await qdrantService.CreateCollection(request.CollectionName, cancellationToken: cancellationToken);
        return Unit.Value;
    }
}