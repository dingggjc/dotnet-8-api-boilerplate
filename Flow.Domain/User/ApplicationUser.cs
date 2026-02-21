
using Flow.Domain.Constant;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flow.Domain.User
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string LastName { get; set; }

        public Guid? OrganizationId { get; set; }

        //public virtual Organization? Organization { get; set; }

        public  UserTypes UserTypes { get; set; }

        public DateTimeOffset CreatedAt { set; get; }
        public string? CreatedById { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastModifiedById { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTimeOffset? LastModifiedDate { get; set; }
        public string? RefreshToken { get; set; }
        public DateTimeOffset? RefreshTokenExpiryTime { get; set; }
        public bool IsDeleted { get; set; }

    }
}
