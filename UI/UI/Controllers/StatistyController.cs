using System.Web.Mvc;
using BLL.BLL;

namespace UI.Controllers
{
    /// <summary>
    /// 统计中心的控制器.
    /// </summary>
    public class StatistyController : StatistyBLL
    {
        /// <summary>
        /// 进入用户管理界面.
        /// </summary>
        /// <returns>用户管理界面.</returns>
        public ActionResult Statisty()
        {
            return View();
        }
    }
}