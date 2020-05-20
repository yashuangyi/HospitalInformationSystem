
namespace Entity.Models
{
    public class RecordDTO : Record
    {
        /// <summary>
        /// Gets or sets 科室.
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// Gets or sets 床位.
        /// </summary>
        public string Bed { get; set; }
    }
}
