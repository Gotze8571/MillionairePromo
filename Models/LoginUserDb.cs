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

            var ans = obj.GetUserAdFullDetails(UserId, password, appId, appKey);

            try
            {
                if(ans != null && ans.Response.Equals("00"))
                {
                    return true;
                }
                else
                {
                    return false;
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