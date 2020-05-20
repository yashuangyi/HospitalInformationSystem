using System.Web.Mvc;
using SqlSugar;
using Entity.Models;
using DAL.DB;

namespace BLL.BLL
{
    /// <summary>
    /// 登录界面的控制器.
    /// </summary>
    public class LoginBLL : Controller
    {
        private static readonly SqlSugarClient Db = DataBase.CreateClient();

        /// <summary>
        /// 后台登录校验.
        /// </summary>
        /// <param name="user">后台登录信息.</param>
        /// <returns>状态码.</returns>
        public ActionResult Check(User user)
        {
            var account = user.Account;
            var password = user.Password;
            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
            {
                return Json(new { code = 401 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var login = Db.Queryable<User>().Where(it => it.Account == account && it.Password == password).Single();
                if (login != null)
                {
                    Session.Add("userId", login.Id);
                    if (login.Power != "家属")
                    {
                        return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { code = 300 }, JsonRequestBehavior.AllowGet);

                    }
                }
                else
                {
                    return Json(new { code = 404 }, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}