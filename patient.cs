using System.ComponentModel.DataAnnotations;

namespace healt_Center.Models
{
    public class patient
    {
        [Key]
        public int Id { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
        public DateTime date { get; set; }
        public string?  Department { get; set; }
        public string? phone { get; set; }
        public string? message { get; set; }
        public int? departmentd_id { get; set; }


        
        public TimeOnly? admin_time { get; set; }

        public string? admin_message { get; set; }

        public int status { get; set; } = 0;

       
        
        
    }
}
