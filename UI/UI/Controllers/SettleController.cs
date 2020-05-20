using System.Web.Mvc;
using BLL.BLL;

namespace UI.Controllers
{
    /// <summary>
    /// 结算中心的控制器.
    /// </summary>
    public class SettleController : SettleBLL
    {
        /// <summary>
        /// 进入结算中心界面.
        /// </summary>
        /// <returns>结算中心界面.</returns>
        public ActionResult Settle()
        {
            return View();
        }
    }
}