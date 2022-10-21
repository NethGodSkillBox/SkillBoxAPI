using SkillBoxAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillBoxAPI.Interfaces
{
    public interface IReq
    {
        IEnumerable<Req> GetReq();
        IEnumerable<HtmlTemplate> GetHtml();
        bool AddReq(Req req);
        bool RemoveReq(Req req);
        bool UpdateReq(Req req);
        bool UpdateTemplate(HtmlTemplate htmlTemplate);
        bool RemoveHtml(HtmlTemplate htmlTemplate);

        Task<bool> Check(string username, string password);
    }
}
