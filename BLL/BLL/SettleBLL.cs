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
    /// 结算中心.
    /// </summary>
    public class SettleBLL : Controller
    {
        private static readonly SqlSugarClient Db = DataBase.CreateClient();

        /// <summary>
        /// 获取结算列表.
        /// </summary>
        /// <param name="page">总页数.</param>
        /// <param name="limit">一页多少行数据.</param>
        /// <param name="search">查询字段.</param>
        /// <returns>结算列表.</returns>
        public ActionResult GetSettle(int page, int limit, string search = null)
        {
            List<BillDTO> list = null;
            int count;
            if (string.IsNullOrEmpty(search))
            {
                // 分页操作，Skip()跳过前面数据项
                var result = Db.Queryable<Record, Bill>((r, b) => new object[]
                {
                    JoinType.Inner, r.Id == b.RecordId && b.Status == "待支付",
                }).Select((r, b) => new BillDTO
                {
                    Id = b.Id,
                    Type = b.Type,
                    PatientName = r.Name,
                    RecordId = b.RecordId,
                    Time = b.Time,
                    Cost = b.Cost,
                });
                count = result.Count();
                list = result.Skip((page - 1) * limit).Take(limit).ToList();
            }
            else
            {
                var result = Db.Queryable<Record, Bill>((r, b) => new object[]
                {
                    JoinType.Inner, r.Id == b.RecordId && b.Status == "待支付" && r.Name.Contains(search),
                }).Select((r, b) => new BillDTO
                {
                    Id = b.Id,
                    Type = b.Type,
                    PatientName = r.Name,
                    RecordId = b.RecordId,
                    Time = b.Time,
                    Cost = b.Cost,
                });
                count = result.Count();
                list = result.Skip((page - 1) * limit).Take(limit).ToList();
            }

            // 参数必须一一对应，JsonRequestBehavior.AllowGet一定要加，表单要求code返回0
            return Json(new { code = 0, msg = string.Empty, count, data = list }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 结账操作.
        /// </summary>
        /// <param name="recordId">病历号.</param>
        /// <param name="type">账单类型.</param>
        /// <returns>Json.</returns>
        public ActionResult Pay(int recordId, string type)
        {
            var record = Db.Queryable<Record>().Where(it => it.Id == recordId).Single();
            var bill = Db.Queryable<Bill>().Where(it => it.Type == type && it.RecordId == recordId).Single();

            var user = Db.Queryable<User>().Where(it => it.RecordId == record.Id).Single();
            user.Message = null;
            Db.Updateable(user).ExecuteCommand();

            // 生成日志
            Log log = new Log
            {
                Id = 0,
                Time = DateTime.Now.ToString(),
                Content = "病历号：" + recordId + "-姓名：" + record.Name + "支付" + bill.Type + "账单",
            };

            // 自增列用法
            Db.Insertable(log).ExecuteReturnIdentity();

            // 更新
            bill.Status = "已支付";
            Db.Updateable(bill).ExecuteCommand();

            if (record.Status != "入院中(已付款)" && Db.Queryable<Bill>().Where(it => it.Status == "待支付" && it.RecordId == recordId && it.Type == "押金").ToList().Count() == 0)
            {
                record.Status = "入院中(未付款)";
                Db.Updateable(record).ExecuteCommand();
            }

            if (Db.Queryable<Bill>().Where(it => it.Status == "待支付" && it.RecordId == recordId).ToList().Count()==0)
            {
                record.Status = "入院中(已付款)";
                Db.Updateable(record).ExecuteCommand();
            }

            return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 催账操作.
        /// </summary>
        /// <param name="recordId">病历号.</param>
        /// <param name="type">账单类型.</param>
        /// <returns>Json.</returns>
        public ActionResult Press(int recordId, string type)
        {
            var record = Db.Queryable<Record>().Where(it => it.Id == recordId).Single();
            var user = Db.Queryable<User>().Where(it => it.RecordId == record.Id).Single();
            user.Message = record.Name + "的家属您好！请您尽快到结算中心处完成患者的" + type + "结账，否则我们将进行欠款处理，谢谢您的配合和理解！";
            Db.Updateable(user).ExecuteCommand();

            return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 欠款处理.
        /// </summary>
        /// <param name="recordId">病历号.</param>
        /// <param name="type">账单类型.</param>
        /// <returns>Json.</returns>
        public ActionResult Arrears(int recordId, string type)
        {
            var record = Db.Queryable<Record>().Where(it => it.Id == recordId).Single();
            var user = Db.Queryable<User>().Where(it => it.RecordId == record.Id).Single();
            user.Message = record.Name + "的家属您好！由于您长期未完成患者的" + type + "结账，我们已进行对应的欠款处理，请您尽快联系医方！";
            Db.Updateable(user).ExecuteCommand();

            return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
        }
    }
}