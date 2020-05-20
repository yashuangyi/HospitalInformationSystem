using System.Web.Mvc;
using SqlSugar;
using Entity.Models;
using DAL.DB;
using System.Collections.Generic;
using System.Linq;

namespace BLL.BLL
{
    /// <summary>
    /// 科室.
    /// </summary>
    public class DepartmentBLL : Controller
    {
        private static readonly SqlSugarClient Db = DataBase.CreateClient();

        /// <summary>
        /// 获取科室列表.
        /// </summary>
        /// <param name="page">总页数.</param>
        /// <param name="limit">一页多少行数据.</param>
        /// <param name="search">查询字段.</param>
        /// <returns>科室列表.</returns>
        public ActionResult GetDepartment(int page, int limit, string search = null)
        {
            List<Department> list = null;
            int count;
            if (string.IsNullOrEmpty(search))
            {
                // 分页操作，Skip()跳过前面数据项
                count = Db.Queryable<Department>().Count();
                list = Db.Queryable<Department>().Skip((page - 1) * limit).Take(limit).ToList();
            }
            else
            {
                count = Db.Queryable<Department>().Where(it => it.Name.Contains(search)).Count();
                list = Db.Queryable<Department>().Where(it => it.Name.Contains(search)).Skip((page - 1) * limit).Take(limit).ToList();
            }

            foreach (var item in list)
            {
                item.FreeDivTotal = item.FreeBedNum + " / " + item.BedNum;
            }

            // 参数必须一一对应，JsonRequestBehavior.AllowGet一定要加，表单要求code返回0
            return Json(new { code = 0, msg = string.Empty, count, data = list }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 新建科室.
        /// </summary>
        /// <param name="department">传入数据.</param>
        /// <returns>Json.</returns>
        public ActionResult AddDepartment(Department department)
        {
            department.FreeBedNum = department.BedNum;
            // 自增列用法
            int departmentId = Db.Insertable(department).ExecuteReturnIdentity();

            AddBed(department,departmentId);
            return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 新建床位.
        /// </summary>
        /// <param name="department">科室.</param>
        /// <param name="departmentId">科室编号.</param>
        public void AddBed(Department department, int departmentId)
        {
            for(int i = 1;i <= department.BedNum;i++)
            {
                Bed bed = new Bed
                {
                    Id = 0,
                    IsFree = "空闲",
                    RecordId = null,
                    DepartmentId = departmentId,
                    Name = department.Name + "——" + i +"号床",
                };
                // 自增列用法
                Db.Insertable(bed).ExecuteReturnIdentity();
            }
        }

        /// <summary>
        /// 加载科室选择框.
        /// </summary>
        /// <returns>Json.</returns>
        public ActionResult ShowDepartmentChoice()
        {
            List<Department> all = Db.Queryable<Department>().ToList();
            if(all == null)
            {
                return Json(new { code = 400 }, JsonRequestBehavior.AllowGet);
            }
            List<string> choice = new List<string>();
            foreach (Department department in all)
            {
                choice.Add(department.Id + " " + department.Name);
            }

            return Json(new { code = 200, choice }, JsonRequestBehavior.AllowGet);
        }
    }
}