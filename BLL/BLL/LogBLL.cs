using System.Web.Mvc;
using SqlSugar;
using Entity.Models;
using DAL.DB;
using System.Collections.Generic;
using System.Linq;

namespace BLL.BLL
{
    /// <summary>
    /// 日志.
    /// </summary>
    public class LogBLL : Controller
    {
        private static readonly SqlSugarClient Db = DataBase.CreateClient();

        /// <summary>
        /// 获取日志列表.
        /// </summary>
        /// <param name="page">总页数.</param>
        /// <param name="limit">一页多少行数据.</param>
        /// <param name="search">查询字段.</param>
        /// <returns>床位列表.</returns>
        public ActionResult GetLog(int page, int limit, string search = null)
        {
            List<Log> list = null;
            int count;
            if (string.IsNullOrEmpty(search))
            {
                // 分页操作，Skip()跳过前面数据项
                count = Db.Queryable<Log>().Count();
                list = Db.Queryable<Log>().Skip((page - 1) * limit).Take(limit).ToList();
            }
            else
            {
                count = Db.Queryable<Log>().Where(it => it.Content.Contains(search)).Count();
                list = Db.Queryable<Log>().Where(it => it.Content.Contains(search)).Skip((page - 1) * limit).Take(limit).ToList();
            }

            // 参数必须一一对应，JsonRequestBehavior.AllowGet一定要加，表单要求code返回0
            return Json(new { code = 0, msg = string.Empty, count, data = list }, JsonRequestBehavior.AllowGet);
        }
    }
}