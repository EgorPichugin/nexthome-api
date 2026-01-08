using MediatR;
using NextHome.QdrantService;

namespace NextHome.Application.Qdrant.Commands;

/// <summary>
/// Command to delete collection from Qdrant.
/// </summary>
/// <param name="CollectionName">Collection name that should be deleted.</param>
public record DeleteCollectionCommand(string CollectionName) : IRequest<Unit>;

/// <summary>
/// Handler to delete collection from Qdrant.
/// </summary>
/// <param name="qdrantService">Service is responsible for managing Qdrant requests.</param>
public class DeleteCollectionHandler(IQdrantService qdrantService) : IRequestHandler<DeleteCollectionCommand, Unit>
{
    /// <inhertidoc />
    public async Task<Unit> Handle(DeleteCollectionCommand request, CancellationToken cancellationToken)
    {
        await qdrantService.DeleteCollection(request.CollectionName, cancellationToken: cancellationToken);
        return Unit.Value;
    }
}