using SqlSugar;

namespace Entity.Models
{
    /// <summary>
    /// 病历实体类.
    /// </summary>
    [SugarTable("record")]
    public partial class Record
    {
        // 指定主键和自增列

        /// <summary>
        /// Gets or sets 病历编号.
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets 名字.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets 性别.
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// Gets or sets 年龄.
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Gets or sets 联系方式.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets 科室号.
        /// </summary>
        public int? DepartmentId { get; set; }

        /// <summary>
        /// Gets or sets 床号.
        /// </summary>
        public int? BedId { get; set; }

        /// <summary>
        /// Gets or sets 医疗费.
        /// </summary>
        public double MedicalCost { get; set; }

        /// <summary>
        /// Gets or sets 住院押金.
        /// </summary>
        public double Deposit { get; set; }

        /// <summary>
        /// Gets or sets 状态（待付押金，入院中(未付款)，入院中(已付款)，已出院）.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets 入院时间.
        /// </summary>
        public string InTime { get; set; }

        /// <summary>
        /// Gets or sets 出院时间.
        /// </summary>
        public string OutTime { get; set; }

        /// <summary>
        /// Gets or sets 查询账号.
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// Gets or sets 查询密码.
        /// </summary>
        public string PassWord { get; set; }
    }
}