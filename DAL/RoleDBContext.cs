using MillionaireWinnerPicker.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MillionaireWinnerPicker.DAL
{
    public class RoleDBContext : DbContext
    {
        public RoleDBContext() : base("name=PromoRoleDB")
        {
        }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserIdentity> UserIdentities { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
    }
}