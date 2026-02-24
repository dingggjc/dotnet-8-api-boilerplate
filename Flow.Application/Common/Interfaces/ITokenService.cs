using Flow.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flow.Application.Common.Interfaces
{
    public interface ITokenService
    {
        Task<(string AccessToken, string RefreshToken)> GenerateTokensAsync(ApplicationUser user);
    }
}
