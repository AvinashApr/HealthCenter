using System.ComponentModel.DataAnnotations;

namespace healt_Center.Models
{
    public class AboutDoctor
    {
        [Key]
        public int? Doctor_id { get; set; }
        public string? Doctor_name { get; set; }
        public string? Doctor_email { get; set; }
        public string? Doctor_gender { get; set; }
        public string? Doctor_number { get; set; }
        public string? Doctor_department { get; set; }
        public string? Doctor_Pic { get; set; }
        public string? LinkdIn { get; set; }

    }
}
