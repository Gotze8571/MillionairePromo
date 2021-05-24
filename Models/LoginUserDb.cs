using MillionaireWinnerPicker.ADService;
using MillionaireWinnerPicker.DAL;
using MillionaireWinnerPicker.Models.AuditTrail;
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
        private readonly RoleDBContext context;
        public LoginUserDb()
        {
        }
        public bool ValidLogin(string UserId, string password, string machineName)
        {
             var obj = new AuthenticationService();

            var appId = ConfigurationManager.AppSettings["AppID"];
            var appKey = ConfigurationManager.AppSettings["AppKey"];
            string userGroup = "";
            bool isValidUser = false;
            int thisUserId = 0;

            //var ans = obj.GetUserAdFullDetails(UserId, password, appId, appKey);

            try
            {
                using (RoleDBContext roleDBContext = new RoleDBContext())
                {
                    //save roles if not exist
                    if (!roleDBContext.Roles.Any())
                    {
                        Role role = new Role();
                        role.RoleName = "GeneralReportGroup";
                        roleDBContext.Roles.Add(role);

                        Role role1 = new Role();
                        role1.RoleName = "LoanGroupAccess";
                        roleDBContext.Roles.Add(role1);
                        roleDBContext.SaveChanges();
                    }

                    var ans = obj.GetUserAdFullDetails(UserId, password, appId, appKey);
                    //logger.Info("Server Response" + ans);
                    if (ans != null && ans.Response.Equals("00"))
                    {
                        //get details for the first group
                        var groups = ConfigurationManager.AppSettings["GeneralGroup"];
                        var groupLoanHoliday = ConfigurationManager.AppSettings["GroupAccess"];

                        var groupSplit = groups.Split(';');
                        var groupSplitLoanHoliday = groupLoanHoliday.Split(';');


                        string userGroups = ans.Groups.ToLower();

                        //check if this user has been logged into the db
                        var exist = roleDBContext.UserIdentities.Where(x => x.StaffId == ans.StaffID).SingleOrDefault();

                       

                        if (exist == null)
                            {
                                //now save this user to the db
                                UserIdentity user = new UserIdentity();
                                user.UserName = UserId;
                                user.StaffId = ans.StaffID;
                                user.TimeStamp = DateTime.Now;
                                var d = roleDBContext.UserIdentities.Add(user);
                                thisUserId = d.UserId;
                                roleDBContext.SaveChanges();
                            }

                            else
                            {
                                thisUserId = exist.UserId;
                            }

                        foreach (var group in groupSplit)
                        {
                            var newGroup = group.Replace("and$", "&");

                            if (userGroups.Contains(newGroup.Trim().ToLower()))
                            {
                                userGroup = group;
                                //first check if this user has a role
                                var role = roleDBContext.UserRoles.Where(x => x.UserId == thisUserId).SingleOrDefault();
                                if (role == null)
                                {
                                    //assign role to this user
                                    UserRole user = new UserRole
                                    {
                                        RoleId = 1,
                                        UserId = thisUserId
                                    };

                                    roleDBContext.UserRoles.Add(user);
                                    roleDBContext.SaveChanges();
                                }
                                isValidUser = true;
                                break;
                            }
                        }

                        //if not in first group
                        if (!isValidUser)
                        {
                            foreach (var group in groupSplitLoanHoliday)
                            {
                                var newGroup = group.Replace("and$", "&");

                                if (userGroups.Contains(newGroup.Trim().ToLower()))
                                {
                                    userGroup = group;
                                    //first check if this user exist
                                    var role = roleDBContext.UserRoles.Where(x => x.UserId == thisUserId).SingleOrDefault();
                                    if (role == null)
                                    {
                                        //assign role to this user
                                        UserRole user = new UserRole
                                        {
                                            RoleId = 2, //LoanHolidayGroup
                                            UserId = thisUserId
                                        };

                                        roleDBContext.UserRoles.Add(user);
                                        roleDBContext.SaveChanges();

                                    }

                                    isValidUser = true;
                                    break;
                                }
                            }

                            logger.Info("Group: " + userGroup);
                            logger.Info("UserId:" + thisUserId);

                            Login login = new Login
                            {
                                Name = UserId,
                                Group = userGroup,
                                Date = DateTime.Now,
                                IPAddress = UserIPAddress.GetIPAddress(),
                                HostName = machineName
                            };

                            context.Logins.Add(login);
                            context.SaveChanges();

                            Session["pUser"] = UserId;
                            Session["admin"] = true.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info(ex.ToString());
            }

            return isValidUser;
        }
    }
}