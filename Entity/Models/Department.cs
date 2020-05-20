using SqlSugar;

namespace Entity.Models
{
    /// <summary>
    /// 临床科室实体类.
    /// </summary>
    [SugarTable("department")]
    public partial class Department
    {
        // 指定主键和自增列

        /// <summary>
        /// Gets or sets id.
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets 科室名.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets 床位费.
        /// </summary>
        public double Cost { get; set; }

        /// <summary>
        /// Gets or sets 空闲床位.
        /// </summary>
        public int FreeBedNum { get; set; }

        /// <summary>
        /// Gets or sets 总床位.
        /// </summary>
        public int BedNum { get; set; }

        /// <summary>
        /// Gets or sets 比例.
        /// </summary>
        public string FreeDivTotal { get; set; }
    }
}