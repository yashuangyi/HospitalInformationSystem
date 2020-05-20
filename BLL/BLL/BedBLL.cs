using System.Web.Mvc;
using SqlSugar;
using Entity.Models;
using DAL.DB;
using System.Collections.Generic;
using System.Linq;

namespace BLL.BLL
{
    /// <summary>
    /// 床位.
    /// </summary>
    public class BedBLL : Controller
    {
        private static readonly SqlSugarClient Db = DataBase.CreateClient();

        /// <summary>
        /// 获取床位列表.
        /// </summary>
        /// <param name="page">总页数.</param>
        /// <param name="limit">一页多少行数据.</param>
        /// <param name="search">查询字段.</param>
        /// <returns>床位列表.</returns>
        public ActionResult GetBed(int page, int limit, string search = null)
        {
            List<BedDTO> list = null;
            int count;
            if (string.IsNullOrEmpty(search))
            {
                // 分页操作，Skip()跳过前面数据项
                var result = Db.Queryable<Bed, Department, Record>((b, d, r) => new object[]
                {
                    JoinType.Inner, b.DepartmentId == d.Id,
                    JoinType.Left, b.RecordId == r.Id
                }).Select((b, d, r) => new BedDTO
                {
                    Id = b.Id,
                    Name = b.Name,
                    IsFree = b.IsFree,
                    PatientName = r.Name,
                    DepartmentName = d.Name,
                    RecordId = b.RecordId,
                    DepartmentId = b.DepartmentId,
                });
                count = result.Count();
                list = result.Skip((page - 1) * limit).Take(limit).ToList();
            }
            else
            {
                var result = Db.Queryable<Bed, Department, Record>((b, d, r) => new object[]
                {
                    JoinType.Inner, b.DepartmentId == d.Id ,
                    JoinType.Inner, b.Id == r.BedId  && r.Name.Contains(search),
                }).Select((b, d, r) => new BedDTO
                {
                    Id = b.Id,
                    Name = b.Name,
                    IsFree = b.IsFree,
                    PatientName = r.Name,
                    DepartmentName = d.Name,
                    RecordId = b.RecordId,
                    DepartmentId = b.DepartmentId,
                });
                count = result.Count();
                list = result.Skip((page - 1) * limit).Take(limit).ToList();
            }

            // count用于分页，必须提前取得
            // 参数必须一一对应，JsonRequestBehavior.AllowGet一定要加，表单要求code返回0
            return Json(new { code = 0, msg = string.Empty, count, data = list }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 加载床位选择框.
        /// </summary>
        /// <param name="department">已选择的科室.</param>
        /// <returns>Json.</returns>
        public ActionResult ShowBedChoice(string department)
        {
            List<string> choice = new List<string>();
            if (department == "")
            {
                return Json(new { code = 200, choice }, JsonRequestBehavior.AllowGet);
            }

            int departmentId = int.Parse(department.Split(' ')[0]);
            List<Bed> all = Db.Queryable<Bed>().Where(it => it.DepartmentId == departmentId && it.IsFree == "空闲").ToList();
            if(all == null)
            {
                return Json(new { code = 400 }, JsonRequestBehavior.AllowGet);
            }

            foreach (Bed bed in all)
            {
                choice.Add(bed.Id + " " + bed.Name);
            }

            double cost = Db.Queryable<Department>().Where(it => it.Id == departmentId).Single().Cost;

            return Json(new { code = 200, choice, cost }, JsonRequestBehavior.AllowGet);
        }
    }
}