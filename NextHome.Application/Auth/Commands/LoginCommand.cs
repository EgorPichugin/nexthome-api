using MediatR;
using NextHome.Application.Auth.Responses;
using NextHome.Core.Interfaces;

namespace NextHome.Application.Auth.Commands;

public record LoginCommand(
    string Email,
    string Password) : IRequest<UserResponse>;

public class LoginCommandHandler(IUserRepository userRepository) : IRequestHandler<LoginCommand, UserResponse>
{
    public async Task<UserResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmail(request.Email, cancellationToken) ?? 
                   throw new UnauthorizedAccessException("Invalid credentials.");

        var isValidPassword =
            BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        
        if (!isValidPassword)
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        return new UserResponse(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName
        );
    }
}