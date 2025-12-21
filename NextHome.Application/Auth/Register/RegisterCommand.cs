using MediatR;

namespace NextHome.Application.Auth.Register;

public record RegisterCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName
) : IRequest<RegisterResponse>;
