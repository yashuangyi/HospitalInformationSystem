using System.Web.Mvc;
using BLL.BLL;

namespace UI.Controllers
{
    /// <summary>
    /// 修改密码的控制器.
    /// </summary>
    public class EditPasswordController : Controller
    {
        /// <summary>
        /// 进入修改密码界面.
        /// </summary>
        /// <returns>修改密码界面.</returns>
        public ActionResult EditPassword()
        {
            return View();
        }
    }
}