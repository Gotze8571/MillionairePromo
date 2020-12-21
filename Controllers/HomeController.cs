using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DataAccessLayer.DataAccessLayer;
using Newtonsoft.Json;

namespace MillionaireWinnerPicker.Controllers
{
    public class HomeController : Controller
    {
        DataAccessLayerEntities db;

        public HomeController()
        {
            this.db = new DataAccessLayerEntities();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet, ActionName("getbranchmillionaires")]
        public ActionResult GetQualifiedMillionairesByBranch()
        {
            var qualifiedMillionairesList = this.db.QualifiedMillionaires_GetList();
            //var randomBranch = qualifiedMillionairesList.Select(x => x.)
            return this.Json(JsonConvert.SerializeObject(qualifiedMillionairesList.ToList()), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, ActionName("getregionalmillionaires")]
        public ActionResult GetQualifiedMillionairesByRegion()
        {
            var qualifiedMillionairesList = this.db.QualifiedMillionaires_GetList();
            return this.Json(JsonConvert.SerializeObject(qualifiedMillionairesList.ToList()), JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("admitwinner")]
        public ActionResult AdmitWinner(QualifiedMillionaireWinner qualifiedMillionaireWinner)
        {
            try
            {
                this.db.QualifiedMillionaireWinners.Add(qualifiedMillionaireWinner);
                this.db.SaveChanges();
            }
            catch (Exception ex)
            {
                //
            }
            return this.Json(new { winner = qualifiedMillionaireWinner }, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet, ActionName("viewmillionaires")]
        public ActionResult GetWinners()
        {
            var qualifiedMillionairesList = this.db.QualifiedMillionaireWinners.ToList();
            return PartialView(qualifiedMillionairesList);
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
    }
}