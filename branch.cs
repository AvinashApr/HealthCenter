using System.ComponentModel.DataAnnotations;

namespace healt_Center.Models
{
    public class branch
    {
        [Key]
        public int B_id { get; set; }
        public string? B_Number { get; set; }
        public string? B_email { get; set; }
        public string? B_opening { get; set; }
        public string? B_saturday { get; set; }
        public string? B_logo { get; set; }
        public string? B_sunday { get; set; }
        public string? fb_link { get; set; }
        public string? insta_link { get; set; }
        public string? twt_link { get; set; }
        public string? B_description { get; set; }
    }
}
