using MediatR;
using NextHome.Application.Auth.Responses;
using NextHome.Core.Interfaces;

namespace NextHome.Application.Auth.Commands;

public record LoginUserRequest(string Email, string Password);

public record LoginUserCommand(
    LoginUserRequest UserRequest) : IRequest<LoginUserResponse>;

public class LoginCommandHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
    : IRequestHandler<LoginUserCommand, LoginUserResponse>
{
    public async Task<LoginUserResponse> Handle(LoginUserCommand userCommand, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmail(userCommand.UserRequest.Email.ToLower(), cancellationToken) ??
                   throw new ArgumentException("Invalid email or password.");

        if (!BCrypt.Net.BCrypt.Verify(userCommand.UserRequest.Password, user.PasswordHash))
            throw new ArgumentException("Invalid email or password.");

        var token = jwtTokenGenerator.GenerateToken(user.Id, user.Email);

        return new LoginUserResponse(
            new UserResponse(user.Id, user.Email, user.FirstName, user.LastName, user.Country, user.City, user.Status,
                user.ImmigrationDate),
            token
        );
    }
}