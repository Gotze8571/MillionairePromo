using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MillionaireWinnerPicker.Models
{
    
    public class LoginUserDb : System.Web.UI.Page
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public LoginUserDb()
        {
           
        }
        public bool ValidLogin(string UserId, string password, string hostname)
        {
            // var obj = new AuthenticationService();
            return false;
        }
    }
}