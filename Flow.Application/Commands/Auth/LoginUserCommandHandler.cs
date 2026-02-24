using ErrorOr;
using Flow.Application.Common.Interfaces;
using Flow.Application.DTO.Auth;
using Flow.Domain.User;

using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flow.Application.Commands.Auth
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ErrorOr<LoginResponseDTO>>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;

        public LoginUserCommandHandler(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<ErrorOr<LoginResponseDTO>> Handle(
            LoginUserCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Request;

    
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                return Error.Unauthorized(description: "Invalid email or password");
            }

   
            var signInResult = await _signInManager.CheckPasswordSignInAsync(
                user,
                dto.Password,
                lockoutOnFailure: false);

            if (!signInResult.Succeeded)
            {
                return Error.Unauthorized(description: "Invalid email or password");
            }

    
            if (user.IsDeleted)
            {
                return Error.Unauthorized(description: "Account is deactivated");
            }

      
            var (accessToken, refreshToken) = await _tokenService.GenerateTokensAsync(user);

       
            var response = new LoginResponseDTO(
                AccessToken: accessToken,
                RefreshToken: refreshToken,
                ExpiresAt: DateTimeOffset.UtcNow.AddMinutes(30), 
                UserId: user.Id,
                Email: user.Email!,
                FirstName: user.FirstName,
                LastName: user.LastName
            );

            return response;
        }
    }
}
