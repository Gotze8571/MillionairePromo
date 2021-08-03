using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MillionaireWinnerPicker.DAL;
using MillionaireWinnerPicker.Models.AuditTrail;
//using DataAccessLayer.DataAccessLayer;
using Newtonsoft.Json;
using NLog;
using Promo.Picker.Core.BuisnessLogic;
using Promo.Picker.Core.BusinessObject;
using Promo.Picker.Core.BusinessObject.Collections;
using Promo.Picker.Core.DataAccess;
using Promo.Picker.Core.Extensions;
using Rotativa;

namespace MillionaireWinnerPicker.Controllers
{
    public class HomeController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        //DataAccessLayerEntities db;
        Promo.Picker.Core.BuisnessLogic.QualifiedMillionaireManager qualified = new Promo.Picker.Core.BuisnessLogic.QualifiedMillionaireManager();
        Promo.Picker.Core.DataAccess.QualifiedMillionaireDB db = new Promo.Picker.Core.DataAccess.QualifiedMillionaireDB();
        private readonly RoleDBContext context;

        //public HomeController()
        //{
        //    this.db = new DataAccessLayerEntities();
        //}

        [Authorize]
        public ActionResult Index()
        {
            string UserId = Session["UserId"] as string;
            string userGroup = Session["Group"] as string;

            //int NoOfEntiries = QualifiedMillionaireManager.NoOfEntries();
            //if (NoOfEntiries >= 0)
            //{
            //    return this.Json(JsonConvert.SerializeObject(NoOfEntiries), JsonRequestBehavior.AllowGet);
            //}
            //return View(NoOfEntiries);

            //Session["pUser"] = UserId;

            //Login loginUser = new Login
            //{
            //    Name = UserId,
            //    Group = userGroup,
            //    Date = DateTime.Now,
            //    IPAddress = "null",
            //    HostName = "null"
            //};

            //context.Logins.Add(loginUser);
            //context.SaveChanges();

            return View();
        }

        

        [HttpGet, ActionName("getRegion")]
        public ActionResult GetQualifiedMillionairesByDistinctRegion()
        {
            var qualifiedMillionairesList = QualifiedMillionaireManager.GetRegionList().Select(c => new { Id = c.RegionCode, Name = c.RegionName }).ToList();
            return this.Json(JsonConvert.SerializeObject(qualifiedMillionairesList.ToList()), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, ActionName("getZone")]
        public ActionResult GetQualifiedMillionairesByZone(string regCode)
        {
            var qualifiedMillionairesList = QualifiedMillionaireManager.GetZoneList(regCode).Select(c => new { Id = c.ZoneCode, Name = c.ZoneName }).ToList();
            return this.Json(JsonConvert.SerializeObject(qualifiedMillionairesList.ToList()), JsonRequestBehavior.AllowGet);
        }


        [HttpGet, ActionName("getBranch")]
        public ActionResult GetQualifiedMillionairesByBranch(string zoneCode)
        {
            var qualifiedMillionairesList = QualifiedMillionaireManager.GetBranchList(zoneCode).Select(c => new { Id = c.BranchCode, Name = c.BranchName }).ToList();
            return this.Json(JsonConvert.SerializeObject(qualifiedMillionairesList.ToList()), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, ActionName("getzonemillionaires")]
        public ActionResult GetQualifiedMillionairesWinnerByZone(string zoneCode)
        {
            var qualifiedMillionairesList = QualifiedMillionaireManager.GetZoneWinnerList(zoneCode);
            return this.Json(JsonConvert.SerializeObject(qualifiedMillionairesList.ToList()), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, ActionName("getbranchmillionaires")]
        public ActionResult GetQualifiedMillionairesWinnerByBranch(string branchCode)
        {
            var qualifiedMillionairesList = QualifiedMillionaireManager.GetBranchWinnerList(branchCode);
            return this.Json(JsonConvert.SerializeObject(qualifiedMillionairesList.ToList()), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, ActionName("getregionalmillionaires")]
        public async Task<ActionResult> GetQualifiedMillionairesWinnerByRegion(string regCode)
        {
            var qualifiedMillionairesList =  QualifiedMillionaireManager.GetRegionWinnerList(regCode);
            return  this.Json(JsonConvert.SerializeObject(qualifiedMillionairesList.ToList()), JsonRequestBehavior.AllowGet);
        }


        [HttpGet, ActionName("getmillionaires")]
        public ActionResult GetQualifiedMillionairesWinner()
        {
            var qualifiedMillionairesList = QualifiedMillionaireManager.GetList();
            return this.Json(JsonConvert.SerializeObject(qualifiedMillionairesList.ToList()), JsonRequestBehavior.AllowGet);
        }


        [HttpPost, ActionName("admitwinner")]
        public ActionResult AdmitWinner(QualifiedMillionaire qualifiedMillionaireWinner)
        {
            try
            {
                var qualifiedWinnerList = QualifiedMillionaireManager.InsertWinners(qualifiedMillionaireWinner);
                
                return this.Json(new { winner = qualifiedMillionaireWinner }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        //[HttpPost, ActionName("admitwinner")]
        //public ActionResult AdmitWinner(QualifiedMillionaireWinner qualifiedMillionaireWinner)
        //{
        //    try
        //    {
        //        this.db.QualifiedMillionaireWinners.Add(qualifiedMillionaireWinner);
        //        this.db.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        //
        //    }
        //    return this.Json(new { winner = qualifiedMillionaireWinner }, JsonRequestBehavior.AllowGet);
        //}

       [HttpGet, ActionName("GetWinners")]
        public ActionResult GetWinners(int? page)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info(DateTime.Now);

            int pageNumber = (page ?? 1);
            const int pageSize = 20;

            try
            {
                var qualifiedMillionairesList = QualifiedMillionaireManager.GetList();
                logger.Info("Qualified Winners no of entries:" + qualifiedMillionairesList);
                return PartialView(qualifiedMillionairesList);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return null;
        }

        // Get no. of winners in a List.
        [HttpGet, ActionName("getnoOfEntries")]
        public ActionResult GetCountOfMillionaires()
        {
            var NoOfEntiries = QualifiedMillionaireManager.NoOfEntries();
            logger.Info("Qualified Winners no of entries:" + NoOfEntiries);
            return this.Json(JsonConvert.SerializeObject(NoOfEntiries), JsonRequestBehavior.AllowGet);

        }

        // Get: Region/Branch
        //public ActionResult AddRegionalList(QualifiedMillionaire regional)
        //{
        //    var regionlist = QualifiedMillionaireManager.GetMillionaireListByRegion(regional);
        //    return null;
        //}
        // Get: Branch/Zone
       
        public ActionResult PromoViewToPdf()
        {
            QualifiedMillionaireDB db = new QualifiedMillionaireDB();

            var qualifiedMillionairesList = QualifiedMillionaireManager.GetList();
            logger.Info("Qualified Winners Report exported successfully");
            return new PartialViewAsPdf("~/Views/Home/GetWinnersPdf.cshtml", qualifiedMillionairesList)
            {
                //FileName = Server.MapPath("~/Content/Relato.pdf"),
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                PageSize = Rotativa.Options.Size.A4
            };
            
            
        }

        public FileContentResult ExportToExcel()
        {
            //QualifiedMillionaireList list = new QualifiedMillionaireList();
            //QualifiedMillionaireManager reportManger = new QualifiedMillionaireManager();
            var list = QualifiedMillionaireManager.GetList();

            //SimbrellaLoanList list = new SimbrellaLoanList();
            //list = report.GetSimbrellaLoanDb(startDate, endDate, CustId);
            string[] columns = { "Id", "AccountNo", "AccountName", "MobileNo", "BranchCode", "PostedDate"};
            byte[] filecontent = ExcelExportHelper.ExportExcel(list, "", true, columns);
            logger.Info("Qualified Winners Report exported successfully");
            string userId = Session["UserId"] as string;
            //Export export = new Export
            //{
            //    ExportedDate = DateTime.Now,
            //    ReportName = "SimbrellaLoanOffer",
            //    LoginUser = userId
            //};
            //context.Exports.Add(export);
            //context.SaveChanges();
            return File(filecontent, ExcelExportHelper.ExcelContentType, "PromoWinner.xlsx");
            logger.Info("Qualified Winners Report exported successfully");
            //return null;
        }
    }
}