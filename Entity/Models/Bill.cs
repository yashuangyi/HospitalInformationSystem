using SqlSugar;

namespace Entity.Models
{
    /// <summary>
    /// 账单实体类.
    /// </summary>
    [SugarTable("bill")]
    public partial class Bill
    {
        // 指定主键和自增列

        /// <summary>
        /// Gets or sets id.
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets 病历号.
        /// </summary>
        public int RecordId { get; set; }

        /// <summary>
        /// Gets or sets 时间.
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// Gets or sets 费用.
        /// </summary>
        public double Cost { get; set; }

        /// <summary>
        /// Gets or sets 缴费类型（医疗费，押金，床位费）.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets 状态（待支付，已支付）.
        /// </summary>
        public string Status { get; set; }
    }
}