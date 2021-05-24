using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MillionaireWinnerPicker.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
    //public class UserRole
    //{
    //    [Key]
    //    public int Id { get; set; }
    //    public int RoleId { get; set; }
    //    public int UserId { get; set; }

    //    public virtual Role Role { get; set; }
    //    public virtual UserIdentity User { get; set; }
    //}
}