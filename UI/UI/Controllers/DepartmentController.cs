using System.Web.Mvc;
using BLL.BLL;

namespace UI.Controllers
{
    /// <summary>
    /// 科室的控制器.
    /// </summary>
    public class DepartmentController : DepartmentBLL
    {
        /// <summary>
        /// 进入科室管理界面.
        /// </summary>
        /// <returns>科室管理界面.</returns>
        public ActionResult Department()
        {
            return View();
        }
    }
}