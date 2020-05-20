using SqlSugar;

namespace Entity.Models
{
    /// <summary>
    /// 用户实体类.
    /// </summary>
    [SugarTable("user")]
    public partial class User
    {
        // 指定主键和自增列

        /// <summary>
        /// Gets or sets id.
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets 账号.
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// Gets or sets 密码.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets 名称.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets 权限.
        /// </summary>
        public string Power { get; set; }

        /// <summary>
        /// Gets or sets 查询绑定的病历号.
        /// </summary>
        public int RecordId { get; set; }

        /// <summary>
        /// Gets or sets 家属消息.
        /// </summary>
        public string Message { get; set; }
    }
}