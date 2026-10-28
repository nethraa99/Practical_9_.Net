using System.Web.Mvc;
using Practical_8_.Net.Models;

namespace Practical_8_.Net.Controllers
{
    public class PlacementController : Controller
    {
        private readonly PlacementDataAccess db = new PlacementDataAccess();

        // GET: /Placement/Index?filter=All
        public ActionResult Index(string filter = "All")
        {
            var model = new PlacementDashboardViewModel
            {
                Summary = db.GetPlacementSummary(),
                StudentsList = db.GetPlacementData(filter),
                CurrentFilter = filter
            };

            return View(model);
        }
    }
}