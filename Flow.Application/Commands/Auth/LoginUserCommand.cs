using ErrorOr;
using Flow.Application.DTO.Auth;
using MediatR;

namespace Flow.Application.Commands.Auth
{
    public record LoginUserCommand(LoginRequestDTO Request)
      : IRequest<ErrorOr<LoginResponseDTO>>;
}
