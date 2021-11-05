using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace gtv_tele.DataAccess
{
    public class AuthenticationDB : DbContext
    {
        public AuthenticationDB()
            : base("AuthenticationDB")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .Map(m =>
                {
                    m.ToTable("UserRoles");
                    m.MapLeftKey("UserId");
                    m.MapRightKey("RoleId");
                });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Menus> Menu { get; set; }
        public DbSet<MenusRoles> MenuRole { get; set; }
        public DbSet<SettingSender> SettingSender { get; set; }
        public DbSet<History_Read> History_Read { get; set; }
        public DbSet<Verify_Code> Verify_Code {  get; set; }
        public DbSet<Master_Upload_Detail> Master_Upload_Detail { get; set; }
        public DbSet<Master_Upload> Master_Upload { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<Body_Message> Body_Message { get; set; }
        public DbSet<VariableToBody> VariableToBody { get; set; }
        public DbSet<VariableToBodyDetail> VariableToBodyDetail { get; set; }
    }
}