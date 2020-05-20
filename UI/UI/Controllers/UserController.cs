using System.Web.Mvc;
using BLL.BLL;

namespace UI.Controllers
{
    /// <summary>
    /// 用户的控制器.
    /// </summary>
    public class UserController : UserBLL
    {
        /// <summary>
        /// 进入用户管理界面.
        /// </summary>
        /// <returns>用户管理界面.</returns>
        public ActionResult User()
        {
            return View();
        }
    }
}