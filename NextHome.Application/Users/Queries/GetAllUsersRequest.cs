using MediatR;
using NextHome.Core.Entities;
using NextHome.Core.Interfaces;

namespace NextHome.Application.Users.Queries;

public record GetAllUsersRequest: IRequest<List<User>>;

public class GetAllUsersRequestHandler(IUserRepository userRepository) : IRequestHandler<GetAllUsersRequest, List<User>>
{
    public async Task<List<User>> Handle(GetAllUsersRequest request, CancellationToken cancellationToken)
    {
        return await userRepository.GetAll(cancellationToken) ?? [];
    }
}