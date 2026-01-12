using Microsoft.Extensions.Options;
using NextHome.Application.Users.Responses;
using NextHome.QdrantService;

namespace NextHome.Application.Common;

public interface ICardValidationService
{
    string Validate(string description);
}

public class CardModerationService(IOptions<QdrantOptions> qdrantOptions) : ICardValidationService
{
    public string Validate(string description)
    {
        throw new NotImplementedException();
    }
}