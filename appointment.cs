using System.ComponentModel.DataAnnotations;

namespace healt_Center.Models
{
    public class appointment
    {
        [Key]
        public int app_Id { get; set; }
        public string? app_name { get; set; }
        public string? app_email { get; set; }
        public DateTime app_date { get; set; }
        public string? app_department { get; set; }
        public string? app_phone { get; set; }

        public TimeSpan? admin_time { get; set; }
        public string? admin_message { get; set; }
        public int status { get; set; } = 0;

    }
}
