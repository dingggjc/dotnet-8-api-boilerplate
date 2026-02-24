using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flow.Application.DTO.Auth
{
    public record LoginRequestDTO(string Email, string Password);
 
}
