using DAL.DB;
using Entity.Models;
using SqlSugar;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BLL.BLL
{
    public class StatistyBLL : Controller
    {
        private static readonly SqlSugarClient Db = DataBase.CreateClient();

        /// <summary>
        /// echarts饼图对象.
        /// </summary>
        public class Echarts
        {
            /// <summary>
            /// Gets or sets 值.
            /// </summary>
            public double value { get; set; }

            /// <summary>
            /// Gets or sets 名称.
            /// </summary>
            public string name { get; set; }
        }

        /// <summary>
        /// echarts柱状图对象.
        /// </summary>
        public class EchartsTwo
        {
            /// <summary>
            /// Gets or sets 值1.
            /// </summary>
            public double value1 { get; set; }

            /// <summary>
            /// Gets or sets 值2.
            /// </summary>
            public double value2 { get; set; }

            /// <summary>
            /// Gets or sets 名称.
            /// </summary>
            public string name { get; set; }
        }

        /// <summary>
        /// 获取echarts饼图所需信息.
        /// </summary>
        /// <returns>Json.</returns>
        public ActionResult GetEchartsOne()
        {
            var departments = Db.Queryable<Department>().ToList();
            var departmentNum = departments.Count();

            List<Echarts> list = new List<Echarts>();

            for(int i = 0; i < departmentNum; i++)
            {
                Echarts echarts = new Echarts
                {
                    value = departments[i].Cost,
                    name = departments[i].Name,
                };
                list.Add(echarts);
            }

            return Json(new { code = 200, data = list, count = list.Count() }, JsonRequestBehavior.AllowGet);
        }

        // <summary>
        /// 获取echarts柱状图所需信息.
        /// </summary>
        /// <returns>Json.</returns>
        public ActionResult GetEchartsTwo()
        {
            var departments = Db.Queryable<Department>().ToList();
            var departmentNum = departments.Count();

            List<EchartsTwo> list = new List<EchartsTwo>();

            for (int i = 0; i < departmentNum; i++)
            {
                EchartsTwo echarts = new EchartsTwo
                {
                    value1 = departments[i].FreeBedNum,
                    value2 = departments[i].BedNum - departments[i].FreeBedNum,
                    name = departments[i].Name,
                };
                list.Add(echarts);
            }

            return Json(new { code = 200, data = list, count = list.Count() }, JsonRequestBehavior.AllowGet);
        }
    }
}