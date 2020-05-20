using System.Web.Mvc;
using SqlSugar;
using Entity.Models;
using DAL.DB;
using System.Collections.Generic;
using System.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.IO;

namespace BLL.BLL
{
    /// <summary>
    /// 床位.
    /// </summary>
    public class UserBLL : Controller
    {
        private static readonly SqlSugarClient Db = DataBase.CreateClient();

        /// <summary>
        /// 获取用户列表.
        /// </summary>
        /// <param name="page">总页数.</param>
        /// <param name="limit">一页多少行数据.</param>
        /// <param name="search">查询字段.</param>
        /// <returns>管理员列表.</returns>
        public ActionResult GetUser(int page, int limit, string search = null)
        {
            List<User> list = null;
            int count;
            // 分页操作，Skip()跳过前面数据项
            if (string.IsNullOrEmpty(search))
            {
                // 分页操作，Skip()跳过前面数据项
                count = Db.Queryable<User>().Count();
                list = Db.Queryable<User>().Skip((page - 1) * limit).Take(limit).ToList();
            }
            else
            {
                count = Db.Queryable<User>().Where(it => it.Name.Contains(search)).Count();
                list = Db.Queryable<User>().Where(it => it.Name.Contains(search)).Skip((page - 1) * limit).Take(limit).ToList();
            }

            // 参数必须一一对应，JsonRequestBehavior.AllowGet一定要加，表单要求code返回0
            return Json(new { code = 0, msg = string.Empty, count, data = list }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 新建用户账户.
        /// </summary>
        /// /// <param name="user">传入数据.</param>
        /// <returns>Json.</returns>
        public ActionResult AddUser(User user)
        {
            var isExist = Db.Queryable<User>().Where(it => it.Account == user.Account).Single();
            if (isExist != null)
            {
                return Json(new { code = 402 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // 自增列用法
                int userId = Db.Insertable(user).ExecuteReturnIdentity();
                user.Id = userId;
                Db.Updateable(user).ExecuteCommand();
                return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 修改用户.
        /// </summary>
        /// <param name="user">用户.</param>
        /// <returns>Json.</returns>
        public ActionResult EditUser(User user)
        {
            Db.Updateable(user).ExecuteCommand();
            return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除账户.
        /// </summary>
        /// <param name="userId">账户编号.</param>
        /// <returns>Json.</returns>
        public ActionResult DelUser(int userId)
        {
            Db.Deleteable<User>().Where(it => it.Id == userId).ExecuteCommand();
            return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 数据备份.
        /// </summary>
        /// <returns>Excel.</returns>
        public ActionResult ExportData()
        {
            // 创建Excel对象 工作薄
            HSSFWorkbook excelBook = new HSSFWorkbook();
            int j = 0;

            // 创建Excel工作表 Sheet
            ISheet sheet = excelBook.CreateSheet("数据备份");

            // 标题
            IRow record = sheet.CreateRow(j);
            record.CreateCell(0).SetCellValue("病历Record");
            List<Record> listRecord = Db.Queryable<Record>().ToList();
            IRow rowTitle = sheet.CreateRow(++j);
            rowTitle.CreateCell(0).SetCellValue("id");
            rowTitle.CreateCell(1).SetCellValue("name");
            rowTitle.CreateCell(2).SetCellValue("sex");
            rowTitle.CreateCell(3).SetCellValue("age");
            rowTitle.CreateCell(4).SetCellValue("phone");
            rowTitle.CreateCell(5).SetCellValue("departmentId");
            rowTitle.CreateCell(6).SetCellValue("bedId");
            rowTitle.CreateCell(7).SetCellValue("medicalCost");
            rowTitle.CreateCell(8).SetCellValue("deposit");
            rowTitle.CreateCell(9).SetCellValue("status");
            rowTitle.CreateCell(10).SetCellValue("inTime");
            rowTitle.CreateCell(11).SetCellValue("outTime");
            rowTitle.CreateCell(12).SetCellValue("account");
            rowTitle.CreateCell(13).SetCellValue("password");

            for (int i = 0 ; i < listRecord.Count(); i++)
            {
                IRow row = sheet.CreateRow(++j);
                row.CreateCell(0).SetCellValue(listRecord[i].Id);
                row.CreateCell(1).SetCellValue(listRecord[i].Name);
                row.CreateCell(2).SetCellValue(listRecord[i].Sex);
                row.CreateCell(3).SetCellValue(listRecord[i].Age);
                row.CreateCell(4).SetCellValue(listRecord[i].Phone);
                row.CreateCell(5).SetCellValue(listRecord[i].DepartmentId.ToString());
                row.CreateCell(6).SetCellValue(listRecord[i].BedId.ToString());
                row.CreateCell(7).SetCellValue(listRecord[i].MedicalCost);
                row.CreateCell(8).SetCellValue(listRecord[i].Deposit);
                row.CreateCell(9).SetCellValue(listRecord[i].Status);
                row.CreateCell(10).SetCellValue(listRecord[i].InTime);
                row.CreateCell(11).SetCellValue(listRecord[i].OutTime);
                row.CreateCell(12).SetCellValue(listRecord[i].Account);
                row.CreateCell(13).SetCellValue(listRecord[i].PassWord);
            }

            j += 2;
            // 标题
            IRow user = sheet.CreateRow(j);
            user.CreateCell(0).SetCellValue("用户User");
            List<User> listUser = Db.Queryable<User>().ToList();
            rowTitle = sheet.CreateRow(++j);
            rowTitle.CreateCell(0).SetCellValue("id");
            rowTitle.CreateCell(1).SetCellValue("account");
            rowTitle.CreateCell(2).SetCellValue("password");
            rowTitle.CreateCell(3).SetCellValue("name");
            rowTitle.CreateCell(4).SetCellValue("power");
            rowTitle.CreateCell(5).SetCellValue("recordId");
            rowTitle.CreateCell(6).SetCellValue("message");

            for (int i = 0; i < listUser.Count(); i++)
            {
                IRow row = sheet.CreateRow(++j);
                row.CreateCell(0).SetCellValue(listUser[i].Id);
                row.CreateCell(1).SetCellValue(listUser[i].Account);
                row.CreateCell(2).SetCellValue(listUser[i].Password);
                row.CreateCell(3).SetCellValue(listUser[i].Name);
                row.CreateCell(4).SetCellValue(listUser[i].Power);
                row.CreateCell(5).SetCellValue(listUser[i].RecordId);
                row.CreateCell(6).SetCellValue(listUser[i].Message);
            }

            j += 2;
            // 标题
            IRow log = sheet.CreateRow(j);
            log.CreateCell(0).SetCellValue("日志Log");
            List<Log> listLog = Db.Queryable<Log>().ToList();
            rowTitle = sheet.CreateRow(++j);
            rowTitle.CreateCell(0).SetCellValue("id");
            rowTitle.CreateCell(1).SetCellValue("time");
            rowTitle.CreateCell(2).SetCellValue("content");

            for (int i = 0; i < listLog.Count(); i++)
            {
                IRow row = sheet.CreateRow(++j);
                row.CreateCell(0).SetCellValue(listLog[i].Id);
                row.CreateCell(1).SetCellValue(listLog[i].Time);
                row.CreateCell(2).SetCellValue(listLog[i].Content);
            }

            j += 2;
            // 标题
            IRow department = sheet.CreateRow(j);
            department.CreateCell(0).SetCellValue("科室Department");
            List<Department> listDepartment = Db.Queryable<Department>().ToList();
            rowTitle = sheet.CreateRow(++j);
            rowTitle.CreateCell(0).SetCellValue("id");
            rowTitle.CreateCell(1).SetCellValue("name");
            rowTitle.CreateCell(2).SetCellValue("cost");
            rowTitle.CreateCell(3).SetCellValue("freeBedNum");
            rowTitle.CreateCell(4).SetCellValue("bedNum");
            rowTitle.CreateCell(5).SetCellValue("freeDivTotal");

            for (int i = 0; i < listDepartment.Count(); i++)
            {
                IRow row = sheet.CreateRow(++j);
                row.CreateCell(0).SetCellValue(listDepartment[i].Id);
                row.CreateCell(1).SetCellValue(listDepartment[i].Name);
                row.CreateCell(2).SetCellValue(listDepartment[i].Cost);
                row.CreateCell(3).SetCellValue(listDepartment[i].FreeBedNum);
                row.CreateCell(4).SetCellValue(listDepartment[i].BedNum);
                row.CreateCell(5).SetCellValue(listDepartment[i].FreeDivTotal);
            }

            j += 2;
            // 标题
            IRow bed = sheet.CreateRow(j);
            bed.CreateCell(0).SetCellValue("床位Bed");
            List<Bed> listBed = Db.Queryable<Bed>().ToList();
            rowTitle = sheet.CreateRow(++j);
            rowTitle.CreateCell(0).SetCellValue("id");
            rowTitle.CreateCell(1).SetCellValue("name");
            rowTitle.CreateCell(2).SetCellValue("departmentId");
            rowTitle.CreateCell(3).SetCellValue("isFree");
            rowTitle.CreateCell(4).SetCellValue("recordId");

            for (int i = 0; i < listBed.Count(); i++)
            {
                IRow row = sheet.CreateRow(++j);
                row.CreateCell(0).SetCellValue(listBed[i].Id);
                row.CreateCell(1).SetCellValue(listBed[i].Name);
                row.CreateCell(2).SetCellValue(listBed[i].DepartmentId);
                row.CreateCell(3).SetCellValue(listBed[i].IsFree);
                row.CreateCell(4).SetCellValue(listBed[i].RecordId.ToString());
            }

            j += 2;
            // 标题
            IRow bill = sheet.CreateRow(j);
            bill.CreateCell(0).SetCellValue("账单Bill");
            List<Bill> listBill = Db.Queryable<Bill>().ToList();
            rowTitle = sheet.CreateRow(++j);
            rowTitle.CreateCell(0).SetCellValue("id");
            rowTitle.CreateCell(1).SetCellValue("recordId");
            rowTitle.CreateCell(2).SetCellValue("cost");
            rowTitle.CreateCell(3).SetCellValue("time");
            rowTitle.CreateCell(4).SetCellValue("type");
            rowTitle.CreateCell(5).SetCellValue("status");

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

            string fileName = "数据备份-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".xls";
            MemoryStream stream = new MemoryStream();
            excelBook.Write(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/vnd.ms-excel", fileName);
        }
    }
}