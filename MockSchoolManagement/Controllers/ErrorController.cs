using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MockSchoolManagement.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> logger;

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
                    logger.LogWarning($"发生了一个404错误。路径={statusCodeResult.OriginalPath}以及查询字符串={statusCodeResult.OriginalQueryString}");
                    break;
            }

            return View("NotFound");
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 注入Asp.net Core ILogger服务，将控制器指定为泛型参数，有助于确定哪个类或控制器产生了异常然后记录它
        /// </summary>
        /// <param name="logger"></param>
        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }

        [Route("Error")]
        public IActionResult Error()
        {
            //获取异常详情信息
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            //logError()方法将异常记录作为日志中的错误类别记录
            logger.LogError($"路径{exceptionHandlerPathFeature.Path}产生一个错误{exceptionHandlerPathFeature.Error}");
            return View("Error");
        }
    }
}
