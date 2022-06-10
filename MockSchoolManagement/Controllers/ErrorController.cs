using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MockSchoolManagement.Controllers
{
    public class ErrorController : Controller
    {
        /// <summary>
        /// 使用属性路由，状态码为404则将路径变为Error/404
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "抱歉，读者访问到的页面不存在";
                    //可获取url请求信息
                    ViewBag.Path = statusCodeResult.OriginalPath;
                    //可获取查询字符串的搜索信息
                    ViewBag.QS = statusCodeResult.OriginalQueryString;
                    //可获取异常的堆栈信息
                    //ViewBag.StackTrace = statusCodeResult.
                    break;
            }

            return View("NotFound");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
