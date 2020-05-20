using System.Web.Mvc;
using BLL.BLL;

namespace UI.Controllers
{
    /// <summary>
    /// 病历的控制器.
    /// </summary>
    public class RecordController : RecordBLL
    {
        /// <summary>
        /// 进入床位管理界面.
        /// </summary>
        /// <returns>床位管理界面.</returns>
        public ActionResult Record()
        {
            return View();
        }
    }
}