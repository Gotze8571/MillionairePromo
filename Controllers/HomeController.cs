using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using DataAccessLayer.DataAccessLayer;
using Newtonsoft.Json;
using Promo.Picker.Core.BuisnessLogic;
using Promo.Picker.Core.BusinessObject;
using Promo.Picker.Core.BusinessObject.Collections;
using Promo.Picker.Core.DataAccess;

namespace MillionaireWinnerPicker.Controllers
{
    public class HomeController : Controller
    {
        //DataAccessLayerEntities db;
        Promo.Picker.Core.BuisnessLogic.QualifiedMillionaireManager qualified = new Promo.Picker.Core.BuisnessLogic.QualifiedMillionaireManager();
        Promo.Picker.Core.DataAccess.QualifiedMillionaireDB db = new Promo.Picker.Core.DataAccess.QualifiedMillionaireDB();

        //public HomeController()
        //{
        //    this.db = new DataAccessLayerEntities();
        //}

        public ActionResult Index()
        {
            //int NoOfEntiries = QualifiedMillionaireManager.NoOfEntries();
            //if (NoOfEntiries >= 0)
            //{
            //    return this.Json(JsonConvert.SerializeObject(NoOfEntiries), JsonRequestBehavior.AllowGet);
            //}
            //return View(NoOfEntiries);

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
        public ActionResult GetQualifiedMillionairesWinnerByRegion(string regCode)
        {
            var qualifiedMillionairesList = QualifiedMillionaireManager.GetRegionWinnerList(regCode);
            return this.Json(JsonConvert.SerializeObject(qualifiedMillionairesList.ToList()), JsonRequestBehavior.AllowGet);
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
            return this.Json(JsonConvert.SerializeObject(NoOfEntiries), JsonRequestBehavior.AllowGet);
        }

        // Get: Region/Branch
        //public ActionResult AddRegionalList(QualifiedMillionaire regional)
        //{
        //    var regionlist = QualifiedMillionaireManager.GetMillionaireListByRegion(regional);
        //    return null;
        //}
        // Get: Branch/Zone
       
        public ActionResult SPLViewToPdf()
        {
            QualifiedMillionaireDB db = new QualifiedMillionaireDB();

            QualifiedMillionaireList list = new QualifiedMillionaireList();
            //list = db.;


            //logger.Info("SPL Report exported successfully");


            //return new PartialViewAsPdf("~/Views/Shared/ExportToPdfSPL.cshtml", list)
            //{
            //    //FileName = Server.MapPath("~/Content/Relato.pdf"),
            //    PageOrientation = Rotativa.Options.Orientation.Landscape,
            //    PageSize = Rotativa.Options.Size.A4
            //};
            return null;
        }
    }
}