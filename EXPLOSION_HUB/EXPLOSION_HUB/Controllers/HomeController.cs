using System.Web.Mvc;

namespace EXPLOSION_HUB.Controllers
{
	public class HomeController : Controller
	{
		[RequireHttps]
		public ActionResult Index()
		{
			return View();
		}
	}
}