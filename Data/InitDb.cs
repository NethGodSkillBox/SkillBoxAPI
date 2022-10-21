using Microsoft.EntityFrameworkCore;
using SkillBoxAPI.Context;

namespace SkillBoxAPI.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ReqDbContext context)
        {
            context.Database.EnsureCreated();
            using (var trans = context.Database.BeginTransaction())
            {
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Reqs] ON");
                context.SaveChanges();
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Reqs] OFF");
                trans.Commit();
            }
            return;
        }
    }
}
