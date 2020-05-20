using SqlSugar;

namespace Entity.Models
{
    /// <summary>
    /// 病床实体类.
    /// </summary>
    [SugarTable("bed")]
    public partial class Bed
    {
        // 指定主键和自增列

        /// <summary>
        /// Gets or sets 病床号.
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets 所属科室号.
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Gets or sets 名称.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets 是否空闲（空闲，已占用）.
        /// </summary>
        public string IsFree { get; set; }

        /// <summary>
        /// Gets or sets 所住病历号.
        /// </summary>
        public int? RecordId { get; set; }
    }
}