using System.Web.Mvc;
using SqlSugar;
using Entity.Models;
using DAL.DB;
using System.Collections.Generic;
using System.Linq;
using System;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;

namespace BLL.BLL
{
    /// <summary>
    /// 账单.
    /// </summary>
    public class BillBLL : Controller
    {
        private static readonly SqlSugarClient Db = DataBase.CreateClient();

        /// <summary>
        /// 获取账单列表.
        /// </summary>
        /// <param name="page">总页数.</param>
        /// <param name="limit">一页多少行数据.</param>
        /// <param name="search">查询字段.</param>
        /// <returns>账单列表.</returns>
        public ActionResult GetBill(int page, int limit, string search = null)
        {
            List<BillDTO> list = null;
            int count;
            if (search == null)
            {
                // 分页操作，Skip()跳过前面数据项
                var result = Db.Queryable<Record, Bill>((r, b) => new object[]
                {
                    JoinType.Inner, r.Id == b.RecordId,
                }).Select((r, b) => new BillDTO
                {
                    Id = b.Id,
                    Type = b.Type,
                    PatientName = r.Name,
                    Time = b.Time,
                    RecordId = b.RecordId,
                    Cost = b.Cost,
                    Status = b.Status,
                });
                count = result.Count();
                list = result.Skip((page - 1) * limit).Take(limit).ToList();
            }
            else
            {
                var result = Db.Queryable<Record, Bill>((r, b) => new object[]
                {
                    JoinType.Inner, r.Id == b.RecordId && r.Name.Contains(search),
                }).Select((r, b) => new BillDTO
                {
                    Id = b.Id,
                    Type = b.Type,
                    PatientName = r.Name,
                    Time = b.Time,
                    RecordId = b.RecordId,
                    Cost = b.Cost,
                    Status = b.Status,
                });
                count = result.Count();
                list = result.Skip((page - 1) * limit).Take(limit).ToList();
            }

            // 参数必须一一对应，JsonRequestBehavior.AllowGet一定要加，表单要求code返回0
            return Json(new { code = 0, msg = string.Empty, count, data = list }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 家属获取费用使用情况列表.
        /// </summary>
        /// <param name="page">总页数.</param>
        /// <param name="limit">一页多少行数据.</param>
        /// <param name="userId">查询字段.</param>
        /// <returns>账单列表.</returns>
        public ActionResult GetParentBill(int page, int limit, int userId)
        {
            User user = Db.Queryable<User>().Where(it => it.Id == userId).Single();
            List<BillDTO> list = null;
            int count;
            // 分页操作，Skip()跳过前面数据项
            var result = Db.Queryable<Record, Bill>((r, b) => new object[]
            {
                    JoinType.Inner, r.Id == b.RecordId && r.Id == user.RecordId,
            }).Select((r, b) => new BillDTO
            {
                Id = b.Id,
                Type = b.Type,
                PatientName = r.Name,
                Time = b.Time,
                RecordId = b.RecordId,
                Cost = b.Cost,
                Status = b.Status,
            });
            count = result.Count();
            list = result.Skip((page - 1) * limit).Take(limit).ToList();

            // 参数必须一一对应，JsonRequestBehavior.AllowGet一定要加，表单要求code返回0
            return Json(new { code = 0, msg = string.Empty, count, data = list }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 作废账单操作.
        /// </summary>
        /// <param name="recordId">病历号.</param>
        /// <param name="type">账单类型.</param>
        /// <returns>Json.</returns>
        public ActionResult Cancel(int recordId, string type)
        {
            var record = Db.Queryable<Record>().Where(it => it.Id == recordId).Single();
            var bill = Db.Queryable<Bill>().Where(it => it.Type == type && it.RecordId == recordId).Single();
            Db.Deleteable(bill).ExecuteCommand();

            // 生成日志
            Log log = new Log
            {
                Id = 0,
                Time = DateTime.Now.ToString(),
                Content = "病历号：" + recordId + "-姓名：" + record.Name + "的押金单据已作废",
            };

            // 自增列用法
            Db.Insertable(log).ExecuteReturnIdentity();

            // 生成账单
            Bill newBill = new Bill
            {
                Id = 0,
                RecordId = recordId,
                Time = DateTime.Now.ToString(),
                Status = "待支付",
                Type = "押金",
                Cost = record.Deposit,
            };
            Db.Insertable(newBill).ExecuteReturnIdentity();

            record.Status = "待付押金";
            Db.Updateable(record).ExecuteCommand();

            return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 打印账单操作.
        /// </summary>
        /// <param name="recordId">病历号.</param>
        /// <param name="type">账单类型.</param>
        /// <returns>Json.</returns>
        public ActionResult Print(int recordId, string type)
        {
            var bill = Db.Queryable<Bill>().Where(it => it.Type == type && it.RecordId == recordId).Single();
            var record = Db.Queryable<Record>().Where(it => it.Id == recordId).Single();
            var department = Db.Queryable<Department>().Where(it => it.Id == record.DepartmentId).Single();
            var bed = Db.Queryable<Bed>().Where(it => it.Id == record.BedId).Single();

            // 创建Excel对象 工作薄
            HSSFWorkbook excelBook = new HSSFWorkbook();

            // 创建Excel工作表 Sheet
            ISheet sheet = excelBook.CreateSheet("押金收据");

            // 标题
            IRow rowTitle = sheet.CreateRow(0);
            rowTitle.CreateCell(0).SetCellValue("账单号");
            rowTitle.CreateCell(1).SetCellValue("患者名");
            rowTitle.CreateCell(2).SetCellValue("性别");
            rowTitle.CreateCell(3).SetCellValue("年龄");
            rowTitle.CreateCell(4).SetCellValue("联系方式");
            rowTitle.CreateCell(5).SetCellValue("时间");
            rowTitle.CreateCell(6).SetCellValue("科室名");
            rowTitle.CreateCell(7).SetCellValue("床位名");
            rowTitle.CreateCell(8).SetCellValue("押金费用");
            rowTitle.CreateCell(9).SetCellValue("状态");

            IRow row = sheet.CreateRow(1);
            row.CreateCell(0).SetCellValue(bill.Id);
            row.CreateCell(1).SetCellValue(record.Name);
            row.CreateCell(2).SetCellValue(record.Sex);
            row.CreateCell(3).SetCellValue(record.Age);
            row.CreateCell(4).SetCellValue(record.Phone);
            row.CreateCell(5).SetCellValue(bill.Time);
            row.CreateCell(6).SetCellValue(department.Name);
            row.CreateCell(7).SetCellValue(bed.Name);
            row.CreateCell(8).SetCellValue(bill.Cost);
            row.CreateCell(9).SetCellValue(bill.Status);

            string fileName = record.Name + "-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "押金收据.xls";
            MemoryStream stream = new MemoryStream();
            excelBook.Write(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/vnd.ms-excel", fileName);
        }

        /// <summary>
        /// 统计报表.
        /// </summary>
        /// <returns>Excel.</returns>
        public ActionResult ExportExcel()
        {
            // 创建Excel对象 工作薄
            HSSFWorkbook excelBook = new HSSFWorkbook();
            int j = 0;

            // 创建Excel工作表 Sheet
            ISheet sheet = excelBook.CreateSheet("财务报表");

            
            // 标题
            IRow bill = sheet.CreateRow(j);
            bill.CreateCell(0).SetCellValue("财务报表Bill");
            List<Bill> listBill = Db.Queryable<Bill>().ToList();
            IRow rowTitle = sheet.CreateRow(++j);
            rowTitle.CreateCell(0).SetCellValue("账单号");
            rowTitle.CreateCell(1).SetCellValue("绑定的病历号");
            rowTitle.CreateCell(2).SetCellValue("费用");
            rowTitle.CreateCell(3).SetCellValue("生成时间");
            rowTitle.CreateCell(4).SetCellValue("费用类型");
            rowTitle.CreateCell(5).SetCellValue("是否已支付");

            for (int i = 0; i < listBill.Count(); i++)
            {
                IRow row = sheet.CreateRow(++j);
                row.CreateCell(0).SetCellValue(listBill[i].Id);
                row.CreateCell(1).SetCellValue(listBill[i].RecordId);
                row.CreateCell(2).SetCellValue(listBill[i].Cost);
                row.CreateCell(3).SetCellValue(listBill[i].Time);
                row.CreateCell(4).SetCellValue(listBill[i].Type);
                row.CreateCell(5).SetCellValue(listBill[i].Status);
            }

            string fileName = "财务报表-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".xls";
            MemoryStream stream = new MemoryStream();
            excelBook.Write(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/vnd.ms-excel", fileName);
        }
    }
}