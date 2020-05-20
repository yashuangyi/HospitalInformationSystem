using System.Web.Mvc;
using BLL.BLL;

namespace UI.Controllers
{
    /// <summary>
    /// 账单的控制器.
    /// </summary>
    public class BillController : BillBLL
    {
        /// <summary>
        /// 进入账单管理界面.
        /// </summary>
        /// <returns>账单管理界面.</returns>
        public ActionResult Bill()
        {
            return View();
        }
    }
}