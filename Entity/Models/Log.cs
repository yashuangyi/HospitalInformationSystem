using SqlSugar;

namespace Entity.Models
{
    /// <summary>
    /// 日志实体类.
    /// </summary>
    [SugarTable("log")]
    public partial class Log
    {
        // 指定主键和自增列

        /// <summary>
        /// Gets or sets id.
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets 时间.
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// Gets or sets 内容.
        /// </summary>
        public string Content { get; set; }
    }
}