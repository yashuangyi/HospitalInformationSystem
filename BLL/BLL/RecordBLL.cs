using System.Web.Mvc;
using SqlSugar;
using Entity.Models;
using DAL.DB;
using System.Collections.Generic;
using System.Linq;
using System;
using BLL.Util;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;

namespace BLL.BLL
{
    /// <summary>
    /// 病历.
    /// </summary>
    public class RecordBLL : Controller
    {
        private static readonly SqlSugarClient Db = DataBase.CreateClient();

        /// <summary>
        /// 获取病历列表.
        /// </summary>
        /// <param name="page">总页数.</param>
        /// <param name="limit">一页多少行数据.</param>
        /// <param name="search">查询字段.</param>
        /// <returns>病历列表.</returns>
        public ActionResult GetRecord(int page, int limit, string search = null)
        {
            List<RecordDTO> list = null;
            int count;
            if (string.IsNullOrEmpty(search))
            {
                // 分页操作，Skip()跳过前面数据项
                var result = Db.Queryable<Record, Department, Bed>((r, d, b) => new object[]
                {
                    JoinType.Inner, r.DepartmentId == d.Id,
                    JoinType.Inner, r.BedId == b.Id
                }).Select((r, d, b) => new RecordDTO
                {
                    Id = r.Id,
                    Name = r.Name,
                    Sex = r.Sex,
                    Age = r.Age,
                    Phone = r.Phone,
                    DepartmentId = r.DepartmentId,
                    BedId = r.BedId,
                    MedicalCost = r.MedicalCost,
                    Deposit = r.Deposit,
                    Account = r.Account,
                    PassWord = r.PassWord,
                    Department = d.Name,
                    Bed = b.Name,
                    InTime = r.InTime,
                    OutTime = r.OutTime,
                    Status = r.Status,
                });
                count = result.Count();
                list = result.Skip((page - 1) * limit).Take(limit).ToList();
            }
            else
            {
                var result = Db.Queryable<Record, Department, Bed>((r, d, b) => new object[]
                {
                    JoinType.Inner, r.DepartmentId == d.Id,
                    JoinType.Inner, r.BedId == b.Id && (r.Name.Contains(search) || r.Id.ToString()==search),
                }).Select((r, d, b) => new RecordDTO
                {
                    Id = r.Id,
                    Name = r.Name,
                    Sex = r.Sex,
                    Age = r.Age,
                    Phone = r.Phone,
                    DepartmentId = r.DepartmentId,
                    BedId = r.BedId,
                    MedicalCost = r.MedicalCost,
                    Deposit = r.Deposit,
                    Account = r.Account,
                    PassWord = r.PassWord,
                    Department = d.Name,
                    Bed = b.Name,
                    InTime = r.InTime,
                    OutTime = r.OutTime,
                    Status = r.Status,
                });
                count = result.Count();
                list = result.Skip((page - 1) * limit).Take(limit).ToList();
            }

            // 参数必须一一对应，JsonRequestBehavior.AllowGet一定要加，表单要求code返回0
            return Json(new { code = 0, msg = string.Empty, count, data = list }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 转科调床操作.
        /// </summary>
        /// <param name="recordId">病历号.</param>
        /// <param name="bed">新床位.</param>
        /// <param name="department">新科室.</param>
        /// <returns>Json.</returns>
        public ActionResult Change(int recordId, string bed, string department)
        {
            var record = Db.Queryable<Record>().Where(it => it.Id == recordId).Single();
            int newDepartmentId = int.Parse(department.Split(' ')[0]);
            int newBedId = int.Parse(bed.Split(' ')[0]);
            var newBed = Db.Queryable<Bed>().Where(it => it.Id == newBedId).Single();
            var newDepartment = Db.Queryable<Department>().Where(it => it.Id == newDepartmentId).Single();
            var oldBed = Db.Queryable<Bed>().Where(it => it.Id == record.BedId).Single();
            var oldDepartment = Db.Queryable<Department>().Where(it => it.Id == record.DepartmentId).Single();

            // 生成日志
            Log log = new Log
            {
                Id = 0,
                Time = DateTime.Now.ToString(),
                Content = "病历号：" + recordId + "-姓名：" + record.Name + "转科调床-从" + oldBed.Name +"转至" + newBed.Name,
            };

            // 自增列用法
            Db.Insertable(log).ExecuteReturnIdentity();

            // 更新病床和科室
            newDepartment.FreeBedNum--;
            oldDepartment.FreeBedNum++;
            newBed.RecordId = oldBed.RecordId;
            oldBed.RecordId = null;
            oldBed.IsFree = "空闲";
            newBed.IsFree = "已占用";
            Db.Updateable(oldBed).ExecuteCommand();
            Db.Updateable(newBed).ExecuteCommand();
            Db.Updateable(oldDepartment).ExecuteCommand();
            Db.Updateable(newDepartment).ExecuteCommand();

            record.BedId = newBed.Id;
            record.DepartmentId = newDepartment.Id;
            Db.Updateable(record).ExecuteCommand();

            return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 出院操作.
        /// </summary>
        /// <param name="recordId">病历号.</param>
        /// <returns>Json.</returns>
        public ActionResult Out(int recordId)
        {
            var record = Db.Queryable<Record>().Where(it => it.Id == recordId).Single();
            var bed = Db.Queryable<Bed>().Where(it => it.Id == record.BedId).Single();
            var department = Db.Queryable<Department>().Where(it => it.Id == record.DepartmentId).Single();

            // 生成日志
            Log log = new Log
            {
                Id = 0,
                Time = DateTime.Now.ToString(),
                Content = "病历号：" + recordId + "-姓名：" + record.Name + "从" + bed.Name + "出院",
            };

            // 自增列用法
            Db.Insertable(log).ExecuteReturnIdentity();

            // 更新病床和科室
            department.FreeBedNum++;
            bed.RecordId = null;
            bed.IsFree = "空闲";
            Db.Updateable(bed).ExecuteCommand();
            Db.Updateable(department).ExecuteCommand();

            record.OutTime = DateTime.Now.ToString();
            record.Status = "已出院";
            Db.Updateable(record).ExecuteCommand();

            return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 新建家属用户.
        /// </summary>
        /// <param name="record">病历.</param>
        /// <param name="recordId">病历号.</param>
        public void AddUser(Record record, int recordId)
        {
            User user = new User
            {
                Id = 0,
                Account = record.Account,
                Password = record.PassWord,
                Name = record.Name + "-家属",
                Power  = "家属",
                RecordId = recordId,
            };

            // 自增列用法
            Db.Insertable(user).ExecuteReturnIdentity();
        }

        /// <summary>
        /// 新建病历.
        /// </summary>
        /// <param name="recordDTO">传入数据.</param>
        /// <returns>Json.</returns>
        public ActionResult AddRecord(RecordDTO recordDTO)
        {
            Record record = new Record
            {
                Id = recordDTO.Id,
                Name = recordDTO.Name,
                Sex = recordDTO.Sex,
                Age = recordDTO.Age,
                Phone = recordDTO.Phone,
                DepartmentId = int.Parse(recordDTO.Department.Split(' ')[0]),
                BedId = int.Parse(recordDTO.Bed.Split(' ')[0]),
                MedicalCost = recordDTO.MedicalCost,
                Deposit = recordDTO.Deposit,
                Status = recordDTO.Status,
                InTime = DateTime.Now.ToString(),
                Account = GetRandomString.GenerateRandomNumber(),
                PassWord = "123456",
            };

            // 自增列用法
            int recordId = Db.Insertable(record).ExecuteReturnIdentity();

            // 生成日志
            Log log = new Log
            {
                Id = 0,
                Time = DateTime.Now.ToString(),
                Content = "病历号：" + recordId + "-姓名：" + record.Name + "办理入院登记",
            };

            // 自增列用法
            Db.Insertable(log).ExecuteReturnIdentity();

            // 生成账单
            Bill bill = new Bill
            {
                Id = 0,
                RecordId = recordId,
                Time = DateTime.Now.ToString(),
                Status = "待支付",
            };
            bill.Type = "押金";
            bill.Cost = record.Deposit;
            Db.Insertable(bill).ExecuteReturnIdentity();
            bill.Type = "医疗费";
            bill.Cost = record.MedicalCost;
            Db.Insertable(bill).ExecuteReturnIdentity();
            bill.Type = "床位费";
            var department = Db.Queryable<Department>().Where(it => it.Id == record.DepartmentId).Single();
            bill.Cost = department.Cost;
            Db.Insertable(bill).ExecuteReturnIdentity();

            // 更新病床和科室
            department.FreeBedNum--;
            var bed = Db.Queryable<Bed>().Where(it => it.Id == record.BedId).Single();
            bed.IsFree = "已占用";
            bed.RecordId = recordId;
            Db.Updateable(department).ExecuteCommand();
            Db.Updateable(bed).ExecuteCommand();

            // 新增家属
            AddUser(record, recordId);

            return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 打印病案首页操作.
        /// </summary>
        /// <param name="recordId">病历号.</param>
        /// <returns>Json.</returns>
        public ActionResult Print(int recordId)
        {
            var record = Db.Queryable<Record>().Where(it => it.Id == recordId).Single();
            var department = Db.Queryable<Department>().Where(it => it.Id == record.DepartmentId).Single();
            var bed = Db.Queryable<Bed>().Where(it => it.Id == record.BedId).Single();

            // 创建Excel对象 工作薄
            HSSFWorkbook excelBook = new HSSFWorkbook();

            // 创建Excel工作表 Sheet
            ISheet sheet = excelBook.CreateSheet("病案首页");

            // 标题
            IRow rowTitle = sheet.CreateRow(0);
            rowTitle.CreateCell(0).SetCellValue("病历号");
            rowTitle.CreateCell(1).SetCellValue("患者名");
            rowTitle.CreateCell(2).SetCellValue("性别");
            rowTitle.CreateCell(3).SetCellValue("年龄");
            rowTitle.CreateCell(4).SetCellValue("联系方式");
            rowTitle.CreateCell(5).SetCellValue("入院时间");
            rowTitle.CreateCell(6).SetCellValue("科室名");
            rowTitle.CreateCell(7).SetCellValue("床位名");
            rowTitle.CreateCell(8).SetCellValue("状态");
            rowTitle.CreateCell(9).SetCellValue("家属账号");
            rowTitle.CreateCell(10).SetCellValue("家属密码");

            IRow row = sheet.CreateRow(1);
            row.CreateCell(0).SetCellValue(record.Id);
            row.CreateCell(1).SetCellValue(record.Name);
            row.CreateCell(2).SetCellValue(record.Sex);
            row.CreateCell(3).SetCellValue(record.Age);
            row.CreateCell(4).SetCellValue(record.Phone);
            row.CreateCell(5).SetCellValue(record.InTime);
            row.CreateCell(6).SetCellValue(department.Name);
            row.CreateCell(7).SetCellValue(bed.Name);
            row.CreateCell(8).SetCellValue(record.Status);
            row.CreateCell(9).SetCellValue(record.Account);
            row.CreateCell(10).SetCellValue(record.PassWord);

            string fileName = record.Name + "-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "病案首页.xls";
            MemoryStream stream = new MemoryStream();
            excelBook.Write(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/vnd.ms-excel", fileName);
        }
    }
}