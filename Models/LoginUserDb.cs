using MillionaireWinnerPicker.ADService;
using MillionaireWinnerPicker.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
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
             var obj = new AuthenticationService();

            var appId = ConfigurationManager.AppSettings["AppID"];
            var appKey = ConfigurationManager.AppSettings["AppKey"];
            bool isValidUser = false;
            int thisUserId = 0;

            try
            {
                if((UserId != "") && (password != ""))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return false;
        }
    }
}