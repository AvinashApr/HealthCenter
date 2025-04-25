using Microsoft.EntityFrameworkCore;

namespace healt_Center.Models
{
    public class Patient_DbContext : DbContext
    {
        public Patient_DbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<patient> patients { get; set; }
        public DbSet<admin> tbl_admin { get; set; }
        public DbSet<department> tbl_department { get; set; }
        public DbSet<AboutDoctor> tbl_About_Doctor { get; set; }
        public DbSet<branch> tbl_branch { get; set; }

    }
    
}
