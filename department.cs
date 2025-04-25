using System.ComponentModel.DataAnnotations;

namespace healt_Center.Models
{
    public class department
    {
        [Key]
        public int? departmentd_id { get; set; }
        public string departments { get; set; }
        
    }
}
