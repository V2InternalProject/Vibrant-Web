using System.Web.Mvc;

namespace HRMS.Controllers
{
    public class AppraisalMasterController : Controller
    {
        private HRMSPageLevelAccess objpagelevel = new HRMSPageLevelAccess();

        // GET: AppraisalMaster
        public ActionResult AppraisalInitiate()
        {
            string PageName = "Initiate";
            objpagelevel.PageLevelAccess(PageName);
            return View();
        }

        public ActionResult ReviewInitiation()
        {
            string PageName = "Initiate";
            objpagelevel.PageLevelAccess(PageName);
            return View();
        }

        public ActionResult Initiated()
        {
            string PageName = "Initiate";
            objpagelevel.PageLevelAccess(PageName);
            return View();
        }

        public ActionResult FreezedList()
        {
            string PageName = "Initiate";
            objpagelevel.PageLevelAccess(PageName);
            return View();
        }
    }
}