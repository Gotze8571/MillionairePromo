using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MillionaireWinnerPicker.Models
{
    public class UserAd
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
    }
}