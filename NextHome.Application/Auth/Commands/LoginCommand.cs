using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NextHome.Application.Auth.Responses;
using NextHome.Core.Interfaces;

namespace NextHome.Application.Auth.Commands;

public record LoginCommand(
    string Email,
    string Password) : IRequest<LoginResponse>;

public class LoginCommandHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmail(request.Email, cancellationToken) ?? 
                   throw new UnauthorizedAccessException("Invalid credentials.");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials.");

        var token = jwtTokenGenerator.GenerateToken(user.Id, user.Email);

        return new LoginResponse(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            token
        );
    }
}