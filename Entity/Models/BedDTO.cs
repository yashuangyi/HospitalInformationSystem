
namespace Entity.Models
{
    public class BedDTO : Bed
    {
        /// <summary>
        /// Gets or sets 科室名称.
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// Gets or sets 患者名称.
        /// </summary>
        public string PatientName { get; set; }
    }
}
