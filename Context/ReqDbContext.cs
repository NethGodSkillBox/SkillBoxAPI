using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkillBoxAPI.Models;

namespace SkillBoxAPI.Context
{
    public class ReqDbContext : DbContext
    {
        public ReqDbContext(DbContextOptions options) : base(options) {  }
        public DbSet<Req> Reqs { get; set; }
        public DbSet<HtmlTemplate> Htmls { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
