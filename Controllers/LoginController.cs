using MillionaireWinnerPicker.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MillionaireWinnerPicker.Controllers
{
    
    public class LoginController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        // GET: Login
        [HttpGet]
        public ActionResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
       // [ValidateAntiForgeryToken]
        public ActionResult Index(User user, string returnUrl)
        {
            logger.Info("NEW LOGIN");

            if (ModelState.IsValid)
            {
                try
                {
                    LoginUserDb login = new LoginUserDb();

                    string machineName = System.Environment.MachineName;

                    //ViewBag.Message = hostName;
                    ViewBag.Message = machineName;
                    Session["hostName"] = machineName;
                    if (login.ValidLogin(user.UserId, user.Password, machineName))
                    //if (login.ValidLogin(user.UserId, user.Password, hostName))
                    {
                        FormsAuthentication.SetAuthCookie(user.UserId, true);

                        logger.Info("Signed in User: " + user.UserId);

                        string UserId = Session["user.UserId"] as string;

                        Session["user.UserId"] = user.UserId;
                        logger.Info("IP Address: " + UserIPAddress.GetIPAddress());
                        //logger.Info("IP Address: " + hostName);
                        logger.Info("IP Address: " + machineName);
                        ViewBag.Message = "hostName";

                        // return RedirectToLocal(returnUrl);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("message", "incorrect login details!");
                        ViewBag.Message = "Incorrect login details";
                        logger.Info("Incorrect login details");
                        // return RedirectToAction("Index", "Login");
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("message", "Unable to connect to server");
                    logger.Error(ex);
                }
            }
            else
            {
                ModelState.AddModelError("message", "Server not connected!");
                ViewBag.Message = "Server not connected!";
            }
            return View(user);

        }
    }
}