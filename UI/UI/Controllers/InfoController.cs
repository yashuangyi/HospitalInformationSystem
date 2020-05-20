using System.Web.Mvc;
using BLL.BLL;

namespace UI.Controllers
{
    /// <summary>
    /// 个人信息的控制器.
    /// </summary>
    public class InfoController : InfoBLL
    {
        /// <summary>
        /// 进入个人信息管理界面.
        /// </summary>
        /// <returns>个人信息管理界面.</returns>
        public ActionResult Info()
        {
            return View();
        }
    }
}