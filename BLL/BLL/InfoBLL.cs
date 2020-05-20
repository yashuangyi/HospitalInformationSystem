using System.Web.Mvc;
using SqlSugar;
using Entity.Models;
using DAL.DB;
using System.Collections.Generic;
using System.Linq;
using System;

namespace BLL.BLL
{
    /// <summary>
    /// 个人信息.
    /// </summary>
    public class InfoBLL : Controller
    {
        private static readonly SqlSugarClient Db = DataBase.CreateClient();

        /// <summary>
        /// 初始化信息.
        /// </summary>
        /// <param name="userId">用户ID.</param>
        /// <returns>Json.</returns>
        public ActionResult GetInfo(int userId)
        {
            var user = Db.Queryable<User>().Where(it => it.Id == userId).Single();
            object data = new
            {
                id = user.Id,
                name = user.Name,
                account = user.Account,
                power = user.Power,
            };
            return Json(new { code = 200, data }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改信息.
        /// </summary>
        /// <param name="id">用户id.</param>
        /// <param name="name">用户新昵称.</param>
        /// <returns>Json.</returns>
        public ActionResult EditInfo(int id, string name)
        {
            var user = Db.Queryable<User>().Where(it => it.Id == id).Single();
            user.Name = name;
            Db.Updateable(user).ExecuteCommand();
            return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改信息.
        /// </summary>
        /// <param name="id">用户id.</param>
        /// <param name="password">用户新密码.</param>
        /// <returns>Json.</returns>
        public ActionResult EditPassword(int id, string password)
        {
            var user = Db.Queryable<User>().Where(it => it.Id == id).Single();
            user.Password = password;
            Db.Updateable(user).ExecuteCommand();
            return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
        }
    }
}