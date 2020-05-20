using System.Web.Mvc;
using SqlSugar;
using Entity.Models;
using DAL.DB;

namespace BLL.BLL
{
    /// <summary>
    /// 主页.
    /// </summary>
    public class HomeBLL : Controller
    {
        private static readonly SqlSugarClient Db = DataBase.CreateClient();

        /// <summary>
        /// 初始化数据.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <returns>json.</returns>
        public ActionResult ReadState(int userId)
        {
            var login = Db.Queryable<User>().Where(it => it.Id == userId).Single();
            if (login != null)
            {
                var userName = login.Name;
                var userPower = login.Power;
                if (login.Message != null)
                {
                    var message = string.Copy(login.Message);
                    login.Message = null;
                    Db.Updateable(login).ExecuteCommand();
                    return Json(new { code = 202, userName, userPower, message }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { code = 200, userName, userPower }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = 404 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 初始化首页数据.
        /// </summary>
        /// <returns>json.</returns>
        public ActionResult ReadPageState()
        {
            int inCount = Db.Queryable<Record>().Where(it => it.Status != "已出院").Count();
            int outCount = Db.Queryable<Record>().Where(it => it.Status == "已出院").Count();
            int freeBed = Db.Queryable<Bed>().Where(it => it.IsFree == "空闲").Count();

            return Json(new { code = 200, inCount, outCount, freeBed }, JsonRequestBehavior.AllowGet);
        }
    }
}