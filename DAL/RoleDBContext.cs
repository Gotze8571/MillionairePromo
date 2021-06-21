using MillionaireWinnerPicker.Models;
using MillionaireWinnerPicker.Models.AuditTrail;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace MillionaireWinnerPicker.DAL
{
    public class RoleDBContext : DbContext
    {
        public RoleDBContext() : base("name=PromoRoleDB")
        {
        }
        public DbSet<Login> Logins { get; set; }
        public DbSet<UserIdentity> UserIdentities { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Export> Exports { get; set; }
        public System.Data.Entity.DbSet<UserAd> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}