using System.ComponentModel.DataAnnotations;

namespace healt_Center.Models
{
    public class admin
    {
        [Key]
        public int Id { get; set; }
       // public string? email { get; set; }
         
        public string? admin_name { get; set; }


      public string? admin_number { get; set; }
      public string? admin_address { get; set; }

        public string? admin_profile { get; set; }
        public string? admin_email { get; set; }
        public string? admin_password { get; set; }
    }
}
