using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SkillBoxAPI.Interfaces;
using SkillBoxAPI.Models;
using System.Collections.Generic;

namespace SkillBoxAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        private IReq reqData;
        public HomeController(IReq ReqData)
        {
            this.reqData = ReqData;
        }


        [Authorize]
        [Route("getlogin")]
        public IActionResult GetLogin()
        {
            return Ok($"Ваш логин: {User.Identity.Name}");
        }

        [Authorize]
        [Route("getrole")]
        public IActionResult GetRole()
        {
            return Ok("Ваша роль: администратор");
        }

        [Route("addReq")]
        [HttpPost]
        public IActionResult AddReq(Req req)
        {
            //var req = JsonConvert.DeserializeObject<Req>(json);
            var add = reqData.AddReq(req);
            if(add)
                return Ok("Заявка добавлена");

            return BadRequest(new { errorText = "Не удалось добавить заявку" });
        }

        [Authorize]
        [Route("updateReq")]
        [HttpPost]
        public IActionResult UpdateReq(Req req)
        {
            var update = reqData.UpdateReq(req);
            if (update)
                return Ok("Заявка обновлена");

            return BadRequest(new { errorText = "Не удалось обновить заявку" });
        }

        [Authorize]
        [Route("removeReq")]
        [HttpPost]
        public IActionResult RemoveReq(Req req)
        {
            var delete = reqData.RemoveReq(req);
            if (delete)
                return Ok("Заявка удалена");

            return BadRequest(new { errorText = "Не удалось удалить заявку" });
        }

        [Authorize]
        [Route("updateHtml")]
        [HttpPost]
        public IActionResult UpdateHtml(HtmlTemplate htmlTemplate)
        {
            var update = reqData.UpdateTemplate(htmlTemplate);
            if (update)
                return Ok("Документ обновлен");

            return BadRequest(new { errorText = "Не удалось обновить документ" });
        }

        [Authorize]
        [Route("removeHtml")]
        [HttpPost]
        public IActionResult RemoveHtml(HtmlTemplate htmlTemplate)
        {
            var update = reqData.RemoveHtml(htmlTemplate);
            if (update)
                return Ok("Документ удален");

            return BadRequest(new { errorText = "Не удалось удалить документ" });
        }


        [Route("getHtml")]
        [HttpGet]
        public IActionResult GetHtml()
        {
            var items = reqData.GetHtml();

            if (items != null)
                return Ok(JsonConvert.SerializeObject(items));

            return BadRequest(new { errorText = "Не удалось получить элементы" });
        }

        [Authorize]
        [Route("getReqs")]
        [HttpGet]
        public IActionResult GetReqs()
        {
            var items = reqData.GetReq();

            if (items != null)
                return Ok(JsonConvert.SerializeObject(items));

            return BadRequest(new { errorText = "Не удалось получить элементы" });
        }

    }
}
