using Microsoft.EntityFrameworkCore;

namespace BackendApp.Models
{
    public class dbContext : DbContext
    {
        public dbContext()
        {
        }

        public dbContext(DbContextOptions<dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<AccountRole> AccountRole { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<TblAttachment> TblAttachment { get; set; }
        public virtual DbSet<TblMasterRole> TblMasterRole { get; set; }
        public virtual DbSet<TblSystemParameter> TblSystemParameter { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost;Initial Catalog=dbUpload;User ID=sa;Password=sapassword;multipleactiveresultsets=true;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        }
}
