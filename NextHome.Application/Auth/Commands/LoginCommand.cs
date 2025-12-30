using MediatR;
using NextHome.Application.Auth.Responses;
using NextHome.Core.Interfaces;

namespace NextHome.Application.Auth.Commands;

public record LoginCommand(
    string Email,
    string Password) : IRequest<LoginResponse>;

public class LoginCommandHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
    : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmail(request.Email, cancellationToken) ??
                   throw new ArgumentException("Invalid email or password.");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new ArgumentException("Invalid email or password.");

        var token = jwtTokenGenerator.GenerateToken(user.Id, user.Email);

        return new LoginResponse(
            new UserResponse(user.Id, user.Email, user.FirstName, user.LastName, user.Country, user.City, user.Status,
                user.ImmigrationDate),
            token
        );
    }
}