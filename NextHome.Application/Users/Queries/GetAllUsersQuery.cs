using MediatR;
using NextHome.Core.Entities;
using NextHome.Core.Interfaces;

namespace NextHome.Application.Users.Queries;

public record GetAllUsersQuery: IRequest<List<UserEntity>>;

public class GetAllUsersRequestHandler(IUserRepository userRepository) : IRequestHandler<GetAllUsersQuery, List<UserEntity>>
{
    public async Task<List<UserEntity>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
    {
        return await userRepository.GetAll(cancellationToken) ?? [];
    }
}