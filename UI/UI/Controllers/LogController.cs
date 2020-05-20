using System.Web.Mvc;
using BLL.BLL;

namespace UI.Controllers
{
    /// <summary>
    /// 日志的控制器.
    /// </summary>
    public class LogController : LogBLL
    {
        /// <summary>
        /// 进入日志管理界面.
        /// </summary>
        /// <returns>日志管理界面.</returns>
        public ActionResult Log()
        {
            return View();
        }
    }
}