using System.Web.Mvc;
using BLL.BLL;

namespace UI.Controllers
{
    /// <summary>
    /// 科室的控制器.
    /// </summary>
    public class BedController : BedBLL
    {
        /// <summary>
        /// 进入床位管理界面.
        /// </summary>
        /// <returns>床位管理界面.</returns>
        public ActionResult Bed()
        {
            return View();
        }
    }
}