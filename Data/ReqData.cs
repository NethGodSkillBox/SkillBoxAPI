using SkillBoxAPI.Context;
using SkillBoxAPI.Interfaces;
using SkillBoxAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillBoxAPI.Data
{
    public class ReqData : IReq
    {
        private ReqDbContext context;
        public ReqData(ReqDbContext Context)
        {
            this.context = Context;
        }
        
        public bool AddReq(Req req)
        {
            var add = context.Reqs.Add(req);
            if(add.State.ToString() == "Added")
            {
                context.SaveChanges();
                var list = context.Reqs.ToList();
                return true;
            }
            return false;
        }

        public async Task<bool> Check(string username, string password)
        {
            var userList = context.Users.ToList();
            var User = context.Users.FirstOrDefault(x => x.Username.ToLower() == username.ToLower() && x.Password == password);
            
            if (User != null)
                return true;
            
            return false;
        }

        public IEnumerable<HtmlTemplate> GetHtml()
        {
            return context.Htmls;
        }

        public IEnumerable<Req> GetReq()
        {
            return context.Reqs;
        }

        public IEnumerable<Req> GetReqs()
        {
            throw new System.NotImplementedException();
        }

        public bool RemoveReq(Req req)
        {
            var update = context.Remove(req);
            if (update.State.ToString() == "Removed")
            {
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool UpdateReq(Req req)
        {
            var update = context.Update(req);
            if (update.State.ToString() == "Modified")
            {
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool UpdateTemplate(HtmlTemplate htmlTemplate)
        {
            var update = context.Update(htmlTemplate);
            if (update.State.ToString() == "Modified" || update.State.ToString() == "Added")
            {
                context.SaveChanges();
                return true;
            }
            return false;
        }
        public bool RemoveHtml(HtmlTemplate htmlTemplate)
        {
            var update = context.Remove(htmlTemplate);
            if (update.State.ToString() == "Deleted")
            {
                context.SaveChanges();
                return true;
            }
            return false;
        }

    }
}
