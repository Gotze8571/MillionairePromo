using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MillionaireWinnerPicker.Models
{
    public class UserIdentity
    {
        [Key]
        public int UserId { get; set; }
        public string StaffId { get; set; }
        public string UserName { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}