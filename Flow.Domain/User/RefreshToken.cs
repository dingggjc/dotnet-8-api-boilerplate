using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flow.Domain.User
{
    public class RefreshToken
    {

        public int Id { get; set; }  

        public string Token { get; set; } = string.Empty; 

        public DateTimeOffset ExpiresAt { get; set; }     

        public DateTimeOffset CreatedAt { get; set; }     

        public string UserId { get; set; } = string.Empty; 

        public ApplicationUser User { get; set; } = null!; 
    }
}
