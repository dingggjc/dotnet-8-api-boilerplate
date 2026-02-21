using Flow.Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flow.Infrastructure.Persistence
{
    public class FlowDbContext : IdentityDbContext<ApplicationUser>
    {

        public FlowDbContext(DbContextOptions<FlowDbContext> options) : base(options)
        { }
    }
}
