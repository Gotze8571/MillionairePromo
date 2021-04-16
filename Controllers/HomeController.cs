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
            return View();
        }

        [HttpGet, ActionName("getbranchmillionaires")]
        public ActionResult GetQualifiedMillionairesByBranch()
        {
            //var qualifiedMillionairesList = this.db.QualifiedMillionaires_GetList();
            var qualifiedMillionairesList = QualifiedMillionaireManager.GetList();
            //var randomBranch = qualifiedMillionairesList.Select(x => x.)
            return this.Json(JsonConvert.SerializeObject(qualifiedMillionairesList.ToList()), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, ActionName("getregionalmillionaires")]
        public ActionResult GetQualifiedMillionairesByRegion()
        {
            //var qualifiedMillionairesList = this.db.QualifiedMillionaires_GetList();
            var qualifiedMillionairesList = QualifiedMillionaireManager.GetQualified1MillionList();
            return this.Json(JsonConvert.SerializeObject(qualifiedMillionairesList.ToList()), JsonRequestBehavior.AllowGet);
        }

        //[HttpPost, ActionName("admitwinner")]
        public ActionResult AdmitWinner(QualifiedMillionaire qualifiedMillionaireWinner)
        {
            try
            {
                //var qualifiedWinnerList = QualifiedMillionaireManager.AddNew(qualifiedMillionaireWinner);
                var qualifiedWinnerList = QualifiedMillionaireManager.AddNew(qualifiedMillionaireWinner);
                return this.Json(new { winner = qualifiedMillionaireWinner }, JsonRequestBehavior.AllowGet);
                //this.db.QualifiedMillionaire.Add(qualifiedMillionaireWinner);
                //this.db.SaveChanges();
            }
            catch (Exception ex)
            {
                //
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

        //[HttpGet, ActionName("viewmillionaires")]
        public ActionResult GetWinners()
        {
            //var qualifiedMillionairesList = this.db.QualifiedMillionaireWinners.ToList();
            var qualifiedMillionairesList = QualifiedMillionaireManager.GetList();
            return PartialView(qualifiedMillionairesList);
        }

        // Get no. of winners in a List.
        public ActionResult GetCountOfMillionaires(int entry)
        {
            var NoOfEntiries = QualifiedMillionaireManager.NoOfEntries(entry);
            return this.Json(JsonConvert.SerializeObject(NoOfEntiries), JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
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