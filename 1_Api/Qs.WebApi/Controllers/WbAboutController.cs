using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Qs.Comm;
using Qs.Repository.Request;
using Qs.Repository.Response;

namespace Qs.WebApi.Controllers
{

    /// <summary>
    /// tb_InviteLinkRecord操作
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WbAboutController : ControllerBase
    {


        /// <summary>
        /// /构造函数
        /// </summary>
        public WbAboutController()
        {

        }

        /// <summary>
        /// 列表查询(不分页,page,limit不需要传)
        /// </summary>
        [HttpGet]
        public Response<List<string>> ListByWhere([FromQuery] ReqQuInviteLinkRecord req)
        {
            Response<List<string>> res = new Response<List<string>>();
            var tick = $"?tick={xConv.GetTimeStampTen(DateTime.Now)}";
            List<string> listPicUrl = new List<string>()
            {
                $"https://wxappapi.webandxc.com/wwwroot/Abort/01.jpg{tick}",
                $"https://wxappapi.webandxc.com/wwwroot/Abort/02.jpg{tick}",
                $"https://wxappapi.webandxc.com/wwwroot/Abort/03.jpg{tick}",
                $"https://wxappapi.webandxc.com/wwwroot/Abort/04.jpg{tick}",
                $"https://wxappapi.webandxc.com/wwwroot/Abort/05.jpg{tick}",
                $"https://wxappapi.webandxc.com/wwwroot/Abort/06.jpg{tick}",
                $"https://wxappapi.webandxc.com/wwwroot/Abort/07.jpg{tick}",
            };
         
            res.Result = listPicUrl;
            return res;
        }
    }
}
